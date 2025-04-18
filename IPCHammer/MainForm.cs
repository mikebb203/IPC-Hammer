using J2534;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcmHacking
{
    public partial class MainForm : MainFormBase, ILogger
    {
        /// <summary>
        /// This will become the first half of the Window caption, and will 
        /// be printed to the user and debug logs each time a device is 
        /// initialized.
        /// </summary>
        private const string AppName = "IPC HAMMER";

        /// <summary>
        /// This becomes the second half of the window caption, is printed
        /// when devices are initialized, and is used to create links to the
        /// help.html and start.txt files.
        /// 
        /// If null, the build timestamp will be used.
        /// 
        /// If not null, use a number like "004" that matches a release branch.
        /// </summary>
        private const string AppVersion = "016.9";

        /// <summary>
        /// We had to move some operations to a background thread for the J2534 code as the DLL functions do not have an awaiter.
        /// </summary>
        private System.Threading.Thread BackgroundWorker = new System.Threading.Thread(delegate () { return; });

        /// <summary>
        /// This flag will initialized when a long-running operation begins. 
        /// It will be toggled if the user clicks the cancel button.
        /// Long-running operations can abort when this flag changes.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Indicates what type of write, if any, is in progress.
        /// </summary>
        private WriteType currentWriteType = WriteType.None;
        private WriteTypeIpc currentWriteTypeIpc = WriteTypeIpc.None;
        /// <summary>
        /// Initializes a new instance of the main window.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Wide enough for the CRC comparison table
            this.Width = 1000;

            // Golden ratio
            this.Height = 618;
        }

        /// <summary>
        /// Add a message to the main window.
        /// </summary>
        public override void AddUserMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("hh:mm:ss:fff");

            this.userLog.Invoke(
                (MethodInvoker)delegate ()
                {
                    this.userLog.AppendText("[" + timestamp + "]  " + message + Environment.NewLine);

                    // User messages are added to the debug log as well, so that the debug log has everything.
                    this.debugLog.AppendText("[" + timestamp + "]  " + message + Environment.NewLine);

                });
        }

        /// <summary>
        /// Add a message to the debug pane of the main window.
        /// </summary>
        public override void AddDebugMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("hh:mm:ss:fff");

            this.debugLog.Invoke(
                (MethodInvoker)delegate ()
                {
                    this.debugLog.AppendText("[" + timestamp + "]  " + message + Environment.NewLine);
                });
        }

        /// <summary>
        /// Reset the user and debug logs.
        /// </summary>
        public override void ResetLogs()
        {
            this.userLog.Invoke(
                (MethodInvoker)delegate ()
                {
                    this.userLog.Text = string.Empty;
                    this.debugLog.Text = string.Empty;
                });
        }

        /// <summary>
        /// Invoked when a device is selected but NOT successfully initalized.
        /// </summary>
        protected override void NoDeviceSelected()
        {
            this.deviceDescription.Text = "No device selected.";
        }

        /// <summary>
        /// Invoked when a device is selected and successfully initialized.
        /// </summary>
        protected override void ValidDeviceSelected(string deviceName)
        {
            this.deviceDescription.Text = deviceName;
        }

        /// <summary>
        /// Show the save-as dialog box (after a full read has completed).
        /// </summary>
        private string ShowSaveAsDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".bin";
            dialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
            dialog.FilterIndex = 0;
            dialog.OverwritePrompt = true;
            dialog.ValidateNames = true;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Show the file-open dialog box, so the user can choose the file to write to the flash.
        /// </summary>
        private string ShowOpenDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".bin";
            dialog.Filter = "Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
            dialog.FilterIndex = 0;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return dialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// Gets a string to use in the window caption and at the top of each log.
        /// </summary>
        public override string GetAppNameAndVersion()
        {
            string versionString = AppVersion;
            if (versionString == null)
            {
                DateTime localTime = Generated.BuildTime.ToLocalTime();
                versionString = String.Format(
                    "({0}, {1})",
                    localTime.ToShortDateString(),
                    localTime.ToShortTimeString());
            }

            return AppName + " " + versionString;
        }

        /// <summary>
        /// Called when the main window is being created.
        /// </summary>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = GetAppNameAndVersion();
                this.interfaceBox.Enabled = true;
                this.operationsBox.Enabled = true;

                // This will be enabled during full reads (but not writes)
                this.cancelButton.Enabled = false;

                // Load the dynamic content asynchronously.
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadStartMessage));
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadHelp));
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadCredits));

                await this.ResetDevice();

                this.MinimumSize = new Size(800, 600);

                menuItemEnable4xReadWrite.Checked = Configuration.Enable4xReadWrite;
            }
            catch (Exception exception)
            {
                this.AddUserMessage(exception.Message);
                this.AddDebugMessage(exception.ToString());
            }
        }

        /// <summary>
        /// The startup message is loaded after the window appears, so that it doesn't slow down app initialization.
        /// </summary>
        private async void LoadStartMessage(object unused)
        {
            ContentLoader loader = new ContentLoader("start.txt", AppVersion, Assembly.GetExecutingAssembly(), this);
            using (Stream content = await loader.GetContentStream())
            {
                try
                {
                    StreamReader reader = new StreamReader(content);
                    string message = reader.ReadToEnd();
                    this.AddUserMessage(message);
                }
                catch (Exception exception)
                {
                    this.AddDebugMessage("Unable to display startup message: " + exception.ToString());
                }
            }
        }

        /// <summary>
        /// The Help page is loaded after the window appears, so that it doesn't slow down app initialization.
        /// </summary>
        private async void LoadHelp(object unused)
        {
            ContentLoader loader = new ContentLoader("help.html", AppVersion, Assembly.GetExecutingAssembly(), this);
            Stream content = await loader.GetContentStream();
            this.helpWebBrowser.Invoke(
                (MethodInvoker)delegate ()
                {
                    try
                    {
                        this.helpWebBrowser.DocumentStream = content;
                    }
                    catch (Exception exception)
                    {
                        this.AddDebugMessage("Unable to load help content: " + exception.ToString());
                    }
                });
        }

        /// <summary>
        /// The credits page is loaded after the window appears, so that it doesn't slow down app initialization.
        /// </summary>
        private async void LoadCredits(object unused)
        {
            ContentLoader loader = new ContentLoader("credits.html", AppVersion, Assembly.GetExecutingAssembly(), this);
            Stream content = await loader.GetContentStream();
            this.helpWebBrowser.Invoke(
                (MethodInvoker)delegate ()
                {
                    try
                    {
                        this.creditsWebBrowser.DocumentStream = content;
                    }
                    catch (Exception exception)
                    {
                        this.AddDebugMessage("Unable load content for Credits tab: " + exception.ToString());
                    }
                });
        }

        /// <summary>
        /// Discourage users from closing the app during a write.
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.currentWriteType == WriteType.None)
            {
                return;
            }

            if (this.currentWriteType == WriteType.TestWrite)
            {
                return;
            }

            var choice = MessageBox.Show(
                this,
                "Closing IPC Hammer now could make your IPC unusable." + Environment.NewLine +
                "Are you sure you want to take that risk?",
                "IPC Hammer",
                MessageBoxButtons.YesNo);

            if (choice == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Disable buttons during a long-running operation (like reading or writing the flash).
        /// </summary>
        protected override void DisableUserInput()
        {
            this.interfaceBox.Enabled = false;

            // The operation buttons have to be enabled/disabled individually
            // (rather than via the parent GroupBox) because we sometimes want
            // to enable the re-initialize operation while the others are disabled.
            this.modifyVINToolStripMenuItem.Enabled = false;
            this.mileageCorrectionToolStripMenuItem.Enabled = false;
            this.readPropertiesButton.Enabled = false;
            this.adjustStepperCalibration.Enabled = false;
            this.writeCalibrationButton.Enabled = false;
            this.write1CalibrationButton.Enabled = false;
            this.IpctestButton.Enabled = false;
            this.reinitializeButton.Enabled = false;
            this.Modify_options.Enabled = false;
            this.Modify_options99.Enabled = false;
            this.menuItemEnable4xReadWrite.Enabled = false;
            this.Checksum_test.Enabled = false;
            this.testipc99.Enabled = false;
            this.TestIPCTB.Enabled = false;
            this.ModifyOptionsTB.Enabled = false;
        }

        /// <summary>
        /// Enable the buttons when a long-running operation completes.
        /// </summary>
        protected override void EnableUserInput()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.interfaceBox.Enabled = true;

                // The operation buttons have to be enabled/disabled individually
                // (rather than via the parent GroupBox) because we sometimes want
                // to enable the re-initialize operation while the others are disabled.
                this.modifyVINToolStripMenuItem.Enabled = false;
                this.mileageCorrectionToolStripMenuItem.Enabled = true;
                this.readPropertiesButton.Enabled = true;
                this.adjustStepperCalibration.Enabled = true;
                this.writeCalibrationButton.Enabled = true;
                this.write1CalibrationButton.Enabled = true;
                this.IpctestButton.Enabled = true;
                this.reinitializeButton.Enabled = true;
                this.Modify_options.Enabled = true;
                this.Checksum_test.Enabled = true;
                this.Modify_options99.Enabled = true;
                this.menuItemEnable4xReadWrite.Enabled = true;
                this.testipc99.Enabled = true;
                this.TestIPCTB.Enabled = true;
                this.ModifyOptionsTB.Enabled = true;
            });
        }

        protected override void EnableInterfaceSelection()
        {
            this.interfaceBox.Enabled = true;
        }

        /// <summary>
        /// Enable/Disable 4x
        /// </summary>
        private void enable4xReadWrite_Click(object sender, EventArgs e)
        {
            menuItemEnable4xReadWrite.Checked = Configuration.Enable4xReadWrite ^= true;
        }

        /// <summary>
        /// Select which interface device to use. This opens the Device-Picker dialog box.
        /// </summary>
        private async void selectButton_Click(object sender, EventArgs e)
        {
            await this.HandleSelectButtonClick();
        }

        /// <summary>
        /// Reset the current interface device.
        /// </summary>
        private async void reinitializeButton_Click(object sender, EventArgs e)
        {
            await this.InitializeCurrentDevice();
        }

        /// <summary>
        /// Read the VIN, OS, etc.
        /// </summary>
        private async void readPropertiesButton_Click(object sender, EventArgs e)
        {
            if (this.Vehicle == null)
            {
                // This shouldn't be possible - it would mean the buttons 
                // were enabled when they shouldn't be.
                return;
            }

            try
            {
                this.DisableUserInput();
                await this.Vehicle.SuppressChatter();
                this.Vehicle.ClearDeviceMessageQueue();

                var vinResponse = await this.Vehicle.QueryVin();
                if (vinResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("VIN query failed: " + vinResponse.Status.ToString());


                }
                else
                {
                    this.AddUserMessage("VIN: " + vinResponse.Value);
                }
                var osResponse = await this.Vehicle.QueryOperatingSystemId(CancellationToken.None);
                if (osResponse.Status == ResponseStatus.Success)
                {
                    this.AddUserMessage("OS ID: " + osResponse.Value.ToString());
                }
                else
                {
                    this.AddUserMessage("OS ID query failed: " + osResponse.Status.ToString());
                }

                
                PcmInfo info = new PcmInfo(osResponse.Value);
                
                var calResponse = await this.Vehicle.QueryCalibrationId();
                if (calResponse.Status == ResponseStatus.Success)
                {
                    this.AddUserMessage("Calibration ID: " + calResponse.Value.ToString());
                }
                else
                {
                    this.AddUserMessage("Calibration ID query failed: " + calResponse.Status.ToString());
                }

                var hardwareResponse = await this.Vehicle.QueryHardwareId();
                if (hardwareResponse.Status == ResponseStatus.Success)
                {
                    this.AddUserMessage("Hardware ID: " + hardwareResponse.Value.ToString());
                }
                else
                {
                    this.AddUserMessage("Hardware ID query failed: " + hardwareResponse.Status.ToString());
                }

                this.AddUserMessage(info.Description.ToString());
                if (info.LatestOS == null)
                {

                }
                else
                {
                    this.AddUserMessage(info.LatestOS.ToString());
                }

                var serialResponse = await this.Vehicle.QuerySerial();
                if (serialResponse.Status == ResponseStatus.Success)
                {
                    this.AddUserMessage("Serial Number: " + serialResponse.Value.ToString());
                }
                else
                {
                    this.AddUserMessage("Serial Number query failed: " + serialResponse.Status.ToString());
                }

                var bccResponse = await this.Vehicle.QueryBCC();
                if (bccResponse.Status == ResponseStatus.Success)
                {
                    this.AddUserMessage("Broad Cast Code: " + bccResponse.Value.ToString());
                }
                else
                {
                    this.AddUserMessage("BCC query failed: " + bccResponse.Status.ToString());
                }

                var mecResponse = await this.Vehicle.QueryMEC();
                if (mecResponse.Status == ResponseStatus.Success)
                {
                    this.AddUserMessage("MEC: " + mecResponse.Value.ToString());
                }
                else
                {
                    this.AddUserMessage("MEC query failed: " + mecResponse.Status.ToString());
                }
            }
            catch (Exception exception)
            {
                this.AddUserMessage(exception.Message);
                this.AddDebugMessage(exception.ToString());
            }
            finally
            {
                this.EnableUserInput();
            }
        }

        /// <summary>
        /// Update the VIN.
        /// </summary>
        private async void modifyVinButton_Click(object sender, EventArgs e)
        {
            try
            {
                Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(CancellationToken.None);
                if (osidResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("Operating system query failed: " + osidResponse.Status);
                    return;
                }

                PcmInfo info = new PcmInfo(osidResponse.Value);

                var vinResponse = await this.Vehicle.QueryVin();
                if (vinResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("VIN query failed: " + vinResponse.Status.ToString());
                    return;
                }

                DialogBoxes.VinForm vinForm = new DialogBoxes.VinForm();
                vinForm.Vin = vinResponse.Value;
                DialogResult dialogResult = vinForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                    if (!unlocked)
                    {
                        this.AddUserMessage("Unable to unlock PCM.");
                        return;
                    }

                    Response<bool> vinmodified = await this.Vehicle.UpdateVin(vinForm.Vin.Trim());
                    if (vinmodified.Value)
                    {
                        this.AddUserMessage("VIN successfully updated to " + vinForm.Vin);
                        MessageBox.Show("VIN updated to " + vinForm.Vin + " successfully.", "Good news.", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Unable to change the VIN to " + vinForm.Vin + ". Error: " + vinmodified.Status, "Bad news.", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddUserMessage("VIN change failed: " + exception.ToString());
            }
        }

        /// <summary>
        /// Read the entire contents of the flash.
        /// </summary>
        private void readFullContentsButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                BackgroundWorker = new System.Threading.Thread(() => readFullContents_BackgroundThread());
                BackgroundWorker.IsBackground = true;
                BackgroundWorker.Start();
            }
        }

        /// <summary>
        /// Write OS and Calibration.
        /// </summary>
        private void writeCalibrationButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                DialogResult result = MessageBox.Show(
                    "This software is still new, and it is not as reliable as commercial software." + Environment.NewLine +
                    "The IPC can be rendered unusuable, and special tools may be needed to make the IPC work again." + Environment.NewLine +
                    "If your IPC stops working, will that make your life difficult?",
                    "Answer carefully...",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    this.AddUserMessage("Please try again with a less important IPC.");
                }
                else
                {
                    BackgroundWorker = new System.Threading.Thread(() => writeIpc_BackgroundThread(WriteTypeIpc.Ipc1));
                    BackgroundWorker.IsBackground = true;
                    BackgroundWorker.Start();
                }
            }
        }

        /// <summary>
        /// Write1 Calibration.
        /// </summary>
        private void write1CalibrationButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                DialogResult result = MessageBox.Show(
                    "This software is still new, and it is not as reliable as commercial software." + Environment.NewLine +
                    "The IPC can be rendered unusuable, and special tools may be needed to make the IPC work again." + Environment.NewLine +
                    "If your IPC stops working, will that make your life difficult?",
                    "Answer carefully...",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    this.AddUserMessage("Please try again with a less important IPC.");
                }
                else
                {
                    BackgroundWorker = new System.Threading.Thread(() => writeIpc_BackgroundThread(WriteTypeIpc.Ipc));
                    BackgroundWorker.IsBackground = true;
                    BackgroundWorker.Start();
                }
            }
        }
        /// <summary>
        /// Test IPC functions
        /// </summary>
        private async void IpctestButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {

                if (this.Vehicle == null)
                {
                    // This shouldn't be possible - it would mean the buttons 
                    // were enabled when they shouldn't be.
                    return;
                }

                try
                {
                    this.DisableUserInput();
                    this.AddUserMessage("IPC test gauges.");
                    await this.Vehicle.SweepGauges();
                    System.Threading.Thread.Sleep(750);

                    this.AddUserMessage("IPC test lights.");
                    await this.Vehicle.LEDson();
                    System.Threading.Thread.Sleep(750);

                    await this.Vehicle.Displayon();
                    this.AddUserMessage("IPC test display pixels.");
                    System.Threading.Thread.Sleep(750);

                    this.AddUserMessage("IPC test prndl display.");
                    await this.Vehicle.PRNDL03();
                }
                catch (Exception exception)
                {
                    this.AddUserMessage(exception.Message);
                    this.AddDebugMessage(exception.ToString());
                }
                finally
                {
                    this.EnableUserInput();
                }
            }
        }

        /// <summary>
        /// Write the parameter blocks (VIN, problem history, etc)
        /// </summary>
        private void writeParametersButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                DialogResult result = MessageBox.Show(
                    "This software is still new, and it is not as reliable as commercial software." + Environment.NewLine +
                    "The PCM can be rendered unusuable, and special tools may be needed to make the PCM work again." + Environment.NewLine +
                    "If your PCM stops working, will that make your life difficult?",
                    "Answer carefully...",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                if (result == DialogResult.Yes)
                {
                    this.AddUserMessage("Please try again with a less important PCM.");
                }
                else
                {
                    BackgroundWorker = new System.Threading.Thread(() => write_BackgroundThread(WriteType.Parameters));
                    BackgroundWorker.IsBackground = true;
                    BackgroundWorker.Start();
                }
            }
        }

        /// <summary>
        /// Write Os, Calibration and Boot.
        /// </summary>
        private void writeOSCalibrationBootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                DialogResult result = MessageBox.Show(
                    "Changing the operating system can render the PCM unusable." + Environment.NewLine +
                    "Special tools may be needed to make the PCM work again." + Environment.NewLine +
                    "Are you sure you really want to take that risk?",
                    "This is dangerous.",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    this.AddUserMessage("You have made a wise choice.");
                }
                else
                {
                    BackgroundWorker = new System.Threading.Thread(() => write_BackgroundThread(WriteType.OsPlusCalibrationPlusBoot));
                    BackgroundWorker.IsBackground = true;
                    BackgroundWorker.Start();
                }
            }
        }

        /// <summary>
        /// Write Full flash (Clone)
        /// </summary>
        private void writeFullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                DialogResult result = MessageBox.Show(
                    "Changing the operating system can render the PCM unusable." + Environment.NewLine +
                    "Special tools may be needed to make the PCM work again." + Environment.NewLine +
                    "Are you sure you really want to take that risk?",
                    "This is dangerous.",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    this.AddUserMessage("You have made a wise choice.");
                }
                else
                {
                    BackgroundWorker = new System.Threading.Thread(() => write_BackgroundThread(WriteType.Full));
                    BackgroundWorker.IsBackground = true;
                    BackgroundWorker.Start();
                }
            }
        }

        /// <summary>
        /// Compare block CRCs of a file and the PCM.
        /// </summary>
        private void quickComparisonButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                BackgroundWorker = new System.Threading.Thread(() => write_BackgroundThread(WriteType.Compare));
                BackgroundWorker.IsBackground = true;
                BackgroundWorker.Start();
            }
        }

        private void testWriteButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                BackgroundWorker = new System.Threading.Thread(() => write_BackgroundThread(WriteType.TestWrite));
                BackgroundWorker.IsBackground = true;
                BackgroundWorker.Start();
            }
        }

        /// <summary>
        /// Test something in a kernel.
        /// </summary>
        private void testKernelButton_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {
                BackgroundWorker = new System.Threading.Thread(() => exitKernel_BackgroundThread());
                BackgroundWorker.IsBackground = true;
                BackgroundWorker.Start();
            }
        }

        /// <summary>
        /// Set the cancelOperation flag, so that an ongoing operation can be aborted.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if ((this.currentWriteType != WriteType.None) && (this.currentWriteType != WriteType.TestWrite))
            {
                var choice = MessageBox.Show(
                    this,
                    "Canceling now could make your PCM unusable." + Environment.NewLine +
                    "Are you sure you want to take that risk?",
                    "PCM Hammer",
                    MessageBoxButtons.YesNo);

                if (choice == DialogResult.No)
                {
                    return;
                }
            }

            this.AddUserMessage("Cancel button clicked.");
            this.cancellationTokenSource?.Cancel();
        }


        private async void adjustStepperCalibration_Click(object sender, EventArgs e)
        {
            try
            {
                Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(CancellationToken.None);
                if (osidResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("Operating system query failed: " + osidResponse.Status);
                    return;
                }

                PcmInfo info = new PcmInfo(osidResponse.Value);

                var stepperResponse = await this.Vehicle.QueryStepper();
  
                if (stepperResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("Stepper Calibration query failed: " + stepperResponse.Status.ToString());
                    return;
                }

                DialogBoxes.StepperForm stepperForm = new DialogBoxes.StepperForm(this.Vehicle, this);


                stepperForm.Stepper = stepperResponse.Value;

                DialogResult dialogResult = stepperForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {

                }
            }
            catch (Exception exception)
            {
                this.AddUserMessage("Stepper Calibration change failed: " + exception.ToString());
            }


        }

        private async void Checksum_test_Click(object sender, EventArgs e)
        {
            string path = null;
            this.Invoke((MethodInvoker)delegate ()
            {
                this.DisableUserInput();
                this.cancelButton.Enabled = true;

                path = this.ShowOpenDialog();

                if (path == null)
                {
                    return;
                }

            });


            if (path == null)
            {
                this.AddUserMessage("Checksum test canceled.");
                return;
            }

            this.AddUserMessage(path);
            byte[] image;
            using (Stream stream = File.OpenRead(path))
            {
                image = new byte[stream.Length];
                int bytesRead = await stream.ReadAsync(image, 0, (int)stream.Length);
                if (bytesRead != stream.Length)
                {
                    // If this happens too much, we should try looping rather than reading the whole file in one shot.
                    this.AddUserMessage("Unable to load file.");
                    return;
                }
            }

            // Sanity checks. 
            FileValidator validator = new FileValidator(image, this);
            if (!validator.ValidateChecksums())
            {
                
                this.AddUserMessage("This file is corrupt. It would render your IPC unusable.");
                this.EnableUserInput();
                return;
            }

            this.EnableUserInput();



        }

        /// <summary>
        /// Read the entire contents of the flash.
        /// </summary>
        private async void readFullContents_BackgroundThread()
        {
            using (new AwayMode())
            {
                try
                {
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        this.DisableUserInput();
                        this.cancelButton.Enabled = true;
                    });

                    if (this.Vehicle == null)
                    {
                        // This shouldn't be possible - it would mean the buttons 
                        // were enabled when they shouldn't be.
                        return;
                    }

                    // Get the path to save the image to.
                    string path = "";
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        path = this.ShowSaveAsDialog();

                        if (path == null)
                        {
                            return;
                        }

                        this.AddUserMessage("Will save to " + path);

                        DelayDialogBox dialogBox = new DelayDialogBox();
                        DialogResult dialogResult = dialogBox.ShowDialog(this);
                        if (dialogResult == DialogResult.Cancel)
                        {
                            path = null;
                            return;
                        }
                    });

                    if (path == null)
                    {
                        this.AddUserMessage("Read canceled.");
                        return;
                    }

                    this.cancellationTokenSource = new CancellationTokenSource();

                    this.AddUserMessage("Querying operating system of current PCM.");
                    Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(this.cancellationTokenSource.Token);
                    if (osidResponse.Status != ResponseStatus.Success)
                    {
                        this.AddUserMessage("Operating system query failed, will retry: " + osidResponse.Status);
                        await this.Vehicle.ExitKernel();

                        osidResponse = await this.Vehicle.QueryOperatingSystemId(this.cancellationTokenSource.Token);
                        if (osidResponse.Status != ResponseStatus.Success)
                        {
                            this.AddUserMessage("Operating system query failed: " + osidResponse.Status);
                        }
                    }

                    PcmInfo info;
                    if (osidResponse.Status == ResponseStatus.Success)
                    {
                        // Look up the information about this PCM, based on the OSID;
                        this.AddUserMessage("OSID: " + osidResponse.Value);
                        info = new PcmInfo(osidResponse.Value);
                    }
                    else
                    {
                        this.AddUserMessage("Unable to get operating system ID. Will assume this can be unlocked with the default seed/key algorithm.");
                        info = new PcmInfo(0);
                    }

                    await this.Vehicle.SuppressChatter();

                    bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                    if (!unlocked)
                    {
                        this.AddUserMessage("Unlock was not successful.");
                        return;
                    }

                    this.AddUserMessage("Unlock succeeded.");

                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    // Do the actual reading.
                    DateTime start = DateTime.Now;

                    CKernelReader reader = new CKernelReader(
                        this.Vehicle,
                        this);

                    Response<Stream> readResponse = await reader.ReadContents(cancellationTokenSource.Token);

                    this.AddUserMessage("Elapsed time " + DateTime.Now.Subtract(start));
                    if (readResponse.Status != ResponseStatus.Success)
                    {
                        this.AddUserMessage("Read failed, " + readResponse.Status.ToString());
                        return;
                    }

                    // Save the contents to the path that the user provided.
                    bool success = false;
                    do
                    {
                        try
                        {
                            this.AddUserMessage("Saving contents to " + path);

                            readResponse.Value.Position = 0;

                            using (Stream output = File.Open(path, FileMode.Create))
                            {
                                await readResponse.Value.CopyToAsync(output);
                            }

                            success = true;
                        }
                        catch (IOException exception)
                        {
                            this.AddUserMessage("Unable to save file: " + exception.Message);
                            this.AddDebugMessage(exception.ToString());

                            this.Invoke((MethodInvoker)delegate () { path = this.ShowSaveAsDialog(); });
                            if (path == null)
                            {
                                this.AddUserMessage("Save canceled.");
                                return;
                            }
                        }
                    } while (!success);
                }
                catch (Exception exception)
                {
                    this.AddUserMessage("Read failed: " + exception.ToString());
                }
                finally
                {
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        this.EnableUserInput();
                        this.cancelButton.Enabled = false;
                    });

                    // The token / token-source can only be cancelled once, so we need to make sure they won't be re-used.
                    this.cancellationTokenSource = null;
                }
            }
        }

        /// <summary>
        /// Write changes to the PCM's flash memory.
        /// </summary>
        private async void write_BackgroundThread(WriteType writeType)
        {
            using (new AwayMode())
            {
                try
                {
                    this.currentWriteType = writeType;

                    if (this.Vehicle == null)
                    {
                        // This shouldn't be possible - it would mean the buttons 
                        // were enabled when they shouldn't be.
                        return;
                    }

                    this.cancellationTokenSource = new CancellationTokenSource();

                    string path = null;
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        this.DisableUserInput();
                        this.cancelButton.Enabled = true;

                        path = this.ShowOpenDialog();

                        if (path == null)
                        {
                            return;
                        }

                        DelayDialogBox dialogBox = new DelayDialogBox();
                        DialogResult dialogResult = dialogBox.ShowDialog(this);
                        if (dialogResult == DialogResult.Cancel)
                        {
                            path = null;
                            return;
                        }
                    });


                    if (path == null)
                    {
                        this.AddUserMessage(
                            writeType == WriteType.TestWrite ?
                                "Test write canceled." :
                                "Write canceled.");
                        return;
                    }

                    this.AddUserMessage(path);

                    byte[] image;
                    using (Stream stream = File.OpenRead(path))
                    {
                        image = new byte[stream.Length];
                        int bytesRead = await stream.ReadAsync(image, 0, (int)stream.Length);
                        if (bytesRead != stream.Length)
                        {
                            // If this happens too much, we should try looping rather than reading the whole file in one shot.
                            this.AddUserMessage("Unable to load file.");
                            return;
                        }
                    }

                    // Sanity checks. 
                    FileValidator validator = new FileValidator(image, this);
                    if (!validator.IsValid())
                    {
                        this.AddUserMessage("This file is corrupt. It would render your PCM unusable.");
                        return;
                    }

                    UInt32 kernelVersion = 0;
                    bool needUnlock;
                    int keyAlgorithm = 0;
                    bool shouldHalt;
                    bool needToCheckOperatingSystem =
                        (writeType != WriteType.OsPlusCalibrationPlusBoot) &&
                        (writeType != WriteType.Full) &&
                        (writeType != WriteType.TestWrite);

                    this.AddUserMessage("Requesting operating system ID...");
                    Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(this.cancellationTokenSource.Token);
                    if (osidResponse.Status == ResponseStatus.Success)
                    {
                        PcmInfo info = new PcmInfo(osidResponse.Value);
                        keyAlgorithm = info.KeyAlgorithm;
                        needUnlock = true;

                        if (!validator.IsSameOperatingSystem(osidResponse.Value))
                        {
                            Utility.ReportOperatingSystems(validator.GetOsidFromImage(), osidResponse.Value, writeType, this, out shouldHalt);
                            if (shouldHalt)
                            {
                                return;
                            }
                        }

                        needToCheckOperatingSystem = false;
                    }
                    else
                    {
                        if (this.cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            return;
                        }

                        this.AddUserMessage("Operating system request failed, checking for a live kernel...");

                        kernelVersion = await this.Vehicle.GetKernelVersion();
                        if (kernelVersion == 0)
                        {
                            this.AddUserMessage("Checking for recovery mode...");
                            bool recoveryMode = await this.Vehicle.IsInRecoveryMode();

                            if (recoveryMode)
                            {
                                this.AddUserMessage("PCM is in recovery mode.");
                                needUnlock = true;
                            }
                            else
                            {
                                this.AddUserMessage("PCM is not responding to OSID, kernel version, or recovery mode checks.");
                                this.AddUserMessage("Unlock may not work, but we'll try...");
                                needUnlock = true;
                            }
                        }
                        else
                        {
                            needUnlock = false;

                            this.AddUserMessage("Kernel version: " + kernelVersion.ToString("X8"));

                            this.AddUserMessage("Asking kernel for the PCM's operating system ID...");

                            if (needToCheckOperatingSystem)
                            {
                                osidResponse = await this.Vehicle.QueryOperatingSystemIdFromKernel(this.cancellationTokenSource.Token);
                                if (osidResponse.Status != ResponseStatus.Success)
                                {
                                    // The kernel seems broken. This shouldn't happen, but if it does, halt.
                                    this.AddUserMessage("The kernel did not respond to operating system ID query.");
                                    return;
                                }

                                Utility.ReportOperatingSystems(validator.GetOsidFromImage(), osidResponse.Value, writeType, this, out shouldHalt);
                                if (shouldHalt)
                                {
                                    return;
                                }
                            }

                            needToCheckOperatingSystem = false;
                        }
                    }

                    await this.Vehicle.SuppressChatter();

                    if (needUnlock)
                    {

                        bool unlocked = await this.Vehicle.UnlockEcu(keyAlgorithm);
                        if (!unlocked)
                        {
                            this.AddUserMessage("Unlock was not successful.");
                            return;
                        }

                        this.AddUserMessage("Unlock succeeded.");
                    }

                    DateTime start = DateTime.Now;

                    CKernelWriter writer = new CKernelWriter(
                        this.Vehicle,
                        new Protocol(),
                        writeType,
                        this);

                    await writer.Write(
                        image,
                        kernelVersion,
                        validator,
                        needToCheckOperatingSystem,
                        this.cancellationTokenSource.Token);

                    this.AddUserMessage("Elapsed time " + DateTime.Now.Subtract(start));
                }
                catch (IOException exception)
                {
                    this.AddUserMessage(exception.ToString());
                }
                finally
                {
                    this.currentWriteType = WriteType.None;

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        this.EnableUserInput();
                        this.cancelButton.Enabled = false;
                    });

                    // The token / token-source can only be cancelled once, so we need to make sure they won't be re-used.
                    this.cancellationTokenSource = null;
                }
            }
        }

        /// <summary>
        /// Write changes to the IPC's flash memory.
        /// </summary>
        private async void writeIpc_BackgroundThread(WriteTypeIpc writeTypeIpc)
        {

            using (new AwayMode())
            {

                try
                {
                    this.currentWriteTypeIpc = writeTypeIpc;


                    if (this.Vehicle == null)
                    {
                        // This shouldn't be possible - it would mean the buttons 
                        // were enabled when they shouldn't be.
                        return;
                    }

                    this.cancellationTokenSource = new CancellationTokenSource();

                    string path = null;
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        this.DisableUserInput();
                        this.cancelButton.Enabled = true;

                        path = this.ShowOpenDialog();

                        if (path == null)
                        {
                            return;
                        }

                       
                    });


                    if (path == null)
                    {
                        this.AddUserMessage("Write canceled.");
                        return;
                    }

                    this.AddUserMessage(path);

                    byte[] image;
                    using (Stream stream = File.OpenRead(path))
                    {
                        image = new byte[stream.Length];
                        int bytesRead = await stream.ReadAsync(image, 0, (int)stream.Length);
                        if (bytesRead != stream.Length)
                        {
                            // If this happens too much, we should try looping rather than reading the whole file in one shot.
                            this.AddUserMessage("Unable to load file.");
                            return;
                        }
                    }
                    switch (writeTypeIpc)
                    {
                        case WriteTypeIpc.Ipc1:  /// OS + Calibration
                            int calid = 0;
                            if (image.Length == 16 * 1024)
                            {
                                calid += image[0x004] << 24;
                                calid += image[0x005] << 16;
                                calid += image[0x006] << 8;
                                calid += image[0x007] << 0;
                            }
                            else
                            {
                                this.AddUserMessage("Calibration file not 16k.");
                                return;
                            }
                            OsInfo info = new OsInfo(calid);
                            
                                                        
                            string filename = info.OsID;
                            string dir = Path.GetDirectoryName(path);
                            path = Path.Combine(dir, filename);

                            byte[] imageos;
                            using (Stream stream = File.OpenRead(path))
                            {
                                imageos = new byte[stream.Length];
                                int bytesRead = await stream.ReadAsync(imageos, 0, (int)stream.Length);
                                if (bytesRead != stream.Length)
                                {
                                    // If this happens too much, we should try looping rather than reading the whole file in one shot.
                                    this.AddUserMessage("Unable to load OS file.");
                                    return;
                                }
                            }

                            var s = new MemoryStream();
                            s.Write(imageos, 0, imageos.Length);
                            s.Write(image, 0, image.Length);
                            var imageoscal = s.ToArray();
                            
                            image = imageoscal;
                            break;

                        case WriteTypeIpc.Ipc:  /// Calibration

                            int calidcal = 0;

                            calidcal += image[0x004] << 24;
                            calidcal += image[0x005] << 16;
                            calidcal += image[0x006] << 8;
                            calidcal += image[0x007] << 0;

                            OsInfo infocal = new OsInfo(calidcal);

                            DialogBoxes.A7k140mphForm a7k140mphForm = new DialogBoxes.A7k140mphForm();
                            DialogResult dialog1Result = a7k140mphForm.ShowDialog();
                            if (dialog1Result == DialogResult.Cancel)
                            {
                                path = null;
                                return;
                            }

                            if (a7k140mphForm.Tach || a7k140mphForm.Speedo)
                            {
                                switch (calidcal)
                                {
                                    ///2003 cluster
                                    case 15104985:
                                    case 15104986:
                                    case 15104987:
                                    case 15104988:
                                    case 15104989:
                                    case 15104990:
                                    case 15104991:
                                    case 15104992:
                                    case 15104993:
                                    case 15104994:
                                    case 15104995:
                                    case 15104996:
                                    case 15104997:
                                    case 15104998:
                                    case 15104999:
                                        if (a7k140mphForm.Tach == true)
                                        {
                                            image[0x252] = 0xD3;
                                            image[0x254] = 0x60;
                                            image[0x255] = 0x6D;
                                        }
                                        if (a7k140mphForm.Speedo == true)
                                        {
                                            image[0x1F2] = 0xB2;
                                            image[0x1F3] = 0x04;
                                            image[0x1F4] = 0xA7;
                                            image[0x1F5] = 0x70;
                                        }
                                        break;

                                    ///2004 cluster
                                    case 15130763:
                                    case 15130764:
                                    case 15130765:
                                    case 15130766:
                                    case 15130767:
                                    case 15130768:
                                    case 15130769:
                                    case 15130770:
                                    case 15130771:
                                    case 15130772:
                                    case 15130773:
                                    case 15130774:
                                    case 15130775:
                                    case 15130776:
                                    case 15130777:
                                    case 15130778:
                                    case 15130779:
                                    case 15130780:
                                    case 15130781:
                                        if (a7k140mphForm.Tach == true)
                                        {
                                            image[0x270] = 0xD3;
                                            image[0x272] = 0x60;
                                            image[0x273] = 0x6D;
                                        }
                                        if (a7k140mphForm.Speedo == true)
                                        {
                                            image[0x210] = 0xB2;
                                            image[0x211] = 0x04;
                                            image[0x212] = 0xA7;
                                            image[0x213] = 0x70;
                                        }
                                        break;

                                    ///2005 cluster
                                    case 15224110:
                                    case 15224111:
                                    case 15224112:
                                    case 15224113:
                                    case 15224114:
                                    case 15224115:
                                    case 15224116:
                                    case 15224117:
                                    case 15224118:
                                    case 15224119:
                                    case 15224120:
                                    case 15224121:
                                    case 15224122:
                                    case 15224123:
                                    case 15224124:
                                    case 15224125:
                                    case 15224126:
                                    case 15224127:
                                    case 15224128:
                                    case 15224129:
                                    case 15224130:
                                    case 15224131:
                                    case 15224132:
                                        if (a7k140mphForm.Tach == true)
                                        {
                                            image[0x2BA] = 0xD3;
                                            image[0x2BC] = 0x60;
                                            image[0x2BD] = 0x6D;
                                        }
                                        
                                        break;

                                    ///2005 cluster 2003-2004 truck
                                    case 15787053:
                                    case 15787054:
                                    case 15787055:
                                    case 15787056:
                                    case 15787057:
                                    case 15787058:
                                    case 15787059:
                                    case 15787060:
                                    case 15787061:
                                    case 15787062:
                                    case 15787063:
                                    case 15787064:
                                    case 15787065:
                                    case 15787066:
                                    case 15787067:
                                    case 15787068:
                                    case 15787069:
                                    case 15787070:
                                    case 15787071:
                                    case 15787072:
                                    case 15787073:
                                        if (a7k140mphForm.Tach == true)
                                        {
                                            image[0x2B8] = 0xD3;
                                            image[0x2BA] = 0x60;
                                            image[0x2BB] = 0x6D;
                                        }
                                        
                                        break;

                                    ///2006 cluster
                                    case 15105974:
                                    case 15105975:
                                    case 15105976:
                                    case 15105978:
                                    case 15105979:
                                    case 15105980:
                                    case 15105981:
                                    case 15105982:
                                    case 15105983:
                                    case 15105986:
                                        if (a7k140mphForm.Tach == true)
                                        {
                                            image[0x2BA] = 0xD3;
                                            image[0x2BC] = 0x60;
                                            image[0x2BD] = 0x6D;
                                        }
                                        
                                        break;


                                    ///2007 cluster
                                    case 15287333:
                                    case 15287334:
                                    case 15287335:
                                    case 15287336:
                                    case 15287337:
                                    case 15287338:
                                    case 15287339:
                                    case 15287340:
                                    case 15287341:
                                    case 15287342:
                                    case 15287343:
                                    case 15287345:
                                    case 15287346:
                                    case 15287347:
                                    case 15287348:
                                    case 15287349:
                                    case 15287350:
                                    case 15287352:
                                    case 15287353:
                                        if (a7k140mphForm.Tach == true)
                                        {
                                            image[0x2BE] = 0xD3;
                                            image[0x2C0] = 0x60;
                                            image[0x2C1] = 0x6D;
                                            
                                        }

                                        break;

                                }

                                FileValidator1 validator1 = new FileValidator1(image, this);
                                if(!validator1.IsValid())
                                {

                                    this.AddUserMessage("This file is corrupt.It would render your IPC unusable.");
                                    return;
                                }

                            }
                            break;
                        default:
                            throw new InvalidDataException("Unsuppported operation type: " + writeTypeIpc.ToString());
                    }

                    DelayDialogBox dialogBox = new DelayDialogBox();
                    DialogResult dialogResult = dialogBox.ShowDialog(this);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        path = null;
                        return;
                    }

                    // Sanity checks. 
                    FileValidator validator = new FileValidator(image, this);
                    if (!validator.IsValid())
                    {
                        this.AddUserMessage("This file is corrupt. It would render your IPC unusable.");
                        return;
                    }

                    UInt32 kernelVersion = 0;
                    bool needUnlock = false;
                    int keyAlgorithm = 0;
                    bool needToCheckOperatingSystem = true;

                    this.AddUserMessage("Requesting hardware ID...");
                    Response<uint> hardwareidResponse = await this.Vehicle.QueryOperatingSystemId(this.cancellationTokenSource.Token);
                    if (hardwareidResponse.Status == ResponseStatus.Success)
                    {

                        PcmInfo info = new PcmInfo(hardwareidResponse.Value);
                        keyAlgorithm = info.KeyAlgorithm;
                        needUnlock = true;

                    }

                    await this.Vehicle.SuppressChatter();

                    if (needUnlock)
                    {

                        bool unlocked = await this.Vehicle.UnlockEcu(keyAlgorithm);
                        if (!unlocked)
                        {
                            this.AddUserMessage("Unlock was not successful.");
                            return;
                        }

                        this.AddUserMessage("Unlock succeeded.");
                    }

                    DateTime start = DateTime.Now;




                    CKernelWriterIpc writer = new CKernelWriterIpc(
                        this.Vehicle,
                        new Protocol(),
                        writeTypeIpc,
                        this);

                    await writer.Write(
                        image,
                        kernelVersion,
                        validator,
                        needToCheckOperatingSystem,
                        this.cancellationTokenSource.Token);

                    this.AddUserMessage("Elapsed time " + DateTime.Now.Subtract(start));
                }
                catch (IOException exception)
                {
                    this.AddUserMessage(exception.ToString());
                }
                finally
                {
                    this.currentWriteTypeIpc = WriteTypeIpc.None;

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        this.EnableUserInput();
                        this.cancelButton.Enabled = false;
                    });

                    // The token / token-source can only be cancelled once, so we need to make sure they won't be re-used.
                    this.cancellationTokenSource = null;
                }
            }
        }
        /// <summary>
        /// From the user's perspective, this is for exiting the kernel, in 
        /// case it remains running after an aborted operation.
        /// 
        /// From the developer's perspective, this is for testing, debugging,
        /// and investigating kernel features that are development.
        /// </summary>
        private async void exitKernel_BackgroundThread()
        {
            try
            {
                if (this.Vehicle == null)
                {
                    // This shouldn't be possible - it would mean the buttons 
                    // were enabled when they shouldn't be.
                    return;
                }

                this.Invoke((MethodInvoker)delegate ()
                {
                    this.DisableUserInput();
                    this.cancelButton.Enabled = true;
                });

                this.cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    await this.Vehicle.ExitKernel(true, false, this.cancellationTokenSource.Token, null);
                }
                catch (IOException exception)
                {
                    this.AddUserMessage(exception.ToString());
                }
            }
            finally
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    this.EnableUserInput();
                    this.cancelButton.Enabled = false;
                });

                // The token / token-source can only be cancelled once, so we need to make sure they won't be re-used.
                this.cancellationTokenSource = null;
            }
        }

        private async void Modify_options_Click(object sender, EventArgs e)
        {
            try
            {
                Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(CancellationToken.None);
                if (osidResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("Operating system query failed: " + osidResponse.Status);
                    return;
                }

                PcmInfo info = new PcmInfo(osidResponse.Value);

                var optionResponse = await this.Vehicle.QueryIPCoptions();
                if (optionResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("IPC options query failed: " + optionResponse.Status.ToString());
                    return;
                }

                uint osidResponse1 = 0307;

                DialogBoxes.OptionsForm optionForm = new DialogBoxes.OptionsForm(this.Vehicle, this, osidResponse1);
                optionForm.Option = optionResponse.Value;
                DialogResult dialogResult = optionForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                    if (!unlocked)
                    {
                        this.AddUserMessage("Unable to unlock PCM.");
                        return;
                    }

                    Response<bool> optionsmodified = await this.Vehicle.UpdateOptions(optionForm.Option.Trim());
                    if (optionsmodified.Value)
                    {
                        this.AddUserMessage("Options successfully updated to " + optionForm.Option);
                        MessageBox.Show("Options updated to " + optionForm.Option + " successfully.", "Good news.", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Unable to change the Options to " + optionForm.Option + ". Error: " + optionsmodified.Status, "Bad news.", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddUserMessage("Options change failed: " + exception.ToString());
            }

            var option1Response = await this.Vehicle.QueryIPCoptions();
            if (option1Response.Status != ResponseStatus.Success)
            {
                this.AddUserMessage("IPC options query failed: " + option1Response.Status.ToString());
                return;
            }

        }

        private async void mileageCorrectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.DisableUserInput();
                
               

                
                DialogBoxes.MileageForm mileageForm = new DialogBoxes.MileageForm();
                DialogResult dialogResult = mileageForm.ShowDialog();

                

                if (dialogResult == DialogResult.OK)
                {
                    String Milesmodified = mileageForm.Mileage.Insert(mileageForm.Mileage.Length - 1, ".");
                    String Hoursmodified = mileageForm.Hours.Insert(mileageForm.Hours.Length - 1, ".");
                    uint model = mileageForm.Model;

                    if (model == 0307)
                    {
                        Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(CancellationToken.None);
                        if (osidResponse.Status != ResponseStatus.Success)
                        {
                            this.AddUserMessage("Operating system query failed: " + osidResponse.Status);
                            this.EnableUserInput();
                            return;
                        }

                        PcmInfo info = new PcmInfo(osidResponse.Value);

                        if (info.Description == "03 GMT800 IPC")
                        {
                            if (mileageForm.Hours.Length > 1)
                            {
                                string path = null;
                                int address = 0x00C000;
                                string filename = info.Kernelname;
                                string dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
                                string exeDirectory = Path.GetDirectoryName(dir);
                                path = Path.Combine(exeDirectory, filename);


                                byte[] mileagekernel;
                                using (Stream stream = File.OpenRead(path))
                                {
                                    mileagekernel = new byte[stream.Length];
                                    int bytesRead = await stream.ReadAsync(mileagekernel, 0, (int)stream.Length);
                                    if (bytesRead != stream.Length)
                                    {
                                        /// If this happens too much, we should try looping rather than reading the whole file in one shot.
                                        this.AddUserMessage("Unable to load kernel.");
                                        return;
                                    }
                                }

                                ushort seedValue = await this.Vehicle.GetSeed();


                                ushort seedResult = (ushort)(seedValue >> 2 | seedValue << 14);

                                uint inthours = uint.Parse(mileageForm.Hours, System.Globalization.NumberStyles.Integer);
                                byte[] ihours = BitConverter.GetBytes(seedResult);
                                byte[] bhours = BitConverter.GetBytes(inthours);

                                mileagekernel[0x17C] = ihours[1];
                                mileagekernel[0x17D] = ihours[0];

                                mileagekernel[0x183] = bhours[2];
                                mileagekernel[0x181] = bhours[0];
                                mileagekernel[0x17F] = bhours[1];

                                bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                                if (!unlocked)
                                {
                                    this.AddUserMessage("Unable to unlock IPC.");
                                    return;
                                }



                                this.AddUserMessage("Changing Hours to " + Hoursmodified);
                                CKernelWriterIpc writer = new CKernelWriterIpc(
                                    this.Vehicle,
                                    new Protocol(),
                                    currentWriteTypeIpc,
                                    this);

                                bool writeresponse = await writer.WriteKernel(
                                    mileagekernel,address,model,
                                    CancellationToken.None);

                                if (writeresponse != true)
                                {
                                    MessageBox.Show("Unable to change the Hours", "", MessageBoxButtons.OK);
                                    this.EnableUserInput();
                                    return;

                                }
                                this.AddUserMessage("Hours successfully updated to " + Hoursmodified);
                                this.AddUserMessage("Delaying to allow IPC to reboot.");
                                await Task.Delay(11000);

                            }

                            if (mileageForm.Mileage.Length > 1)
                            {
                                this.AddUserMessage("Changing Mileage to " + Milesmodified);
                                bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                                if (!unlocked)
                                {
                                    this.AddUserMessage("Unable to unlock IPC.");
                                    this.EnableUserInput();
                                    return;
                                }

                                Response<bool> mileagemodified = await this.Vehicle.UpdateMileage(mileageForm.Mileage.Trim());
                                if (mileagemodified.Value)
                                {

                                    this.AddUserMessage("Mileage successfully updated to " + Milesmodified);
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Unable to change the Mileage to " + Milesmodified + ". Error: " + mileagemodified.Status, "Bad news.", MessageBoxButtons.OK);
                                }
                            }

                        }




                        if (info.Description != "03 GMT800 IPC")
                        {


                            if (mileageForm.Mileage.Length > 1)
                            {
                                this.AddUserMessage("Changing Mileage to " + Milesmodified);
                                bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                                if (!unlocked)
                                {
                                    this.AddUserMessage("Unable to unlock IPC.");
                                    this.EnableUserInput();
                                    return;
                                }

                                Response<bool> mileagemodified = await this.Vehicle.UpdateMileage(mileageForm.Mileage.Trim());
                                if (mileagemodified.Value)
                                {
                                    this.AddUserMessage("Mileage successfully updated to " + Milesmodified);
                                    MessageBox.Show("Mileage updated to " + Milesmodified + " successfully.", "Good news.", MessageBoxButtons.OK);
                                }
                                else
                                {
                                    MessageBox.Show("Unable to change the Mileage to " + Milesmodified + ". Error: " + mileagemodified.Status, "Bad news.", MessageBoxButtons.OK);
                                }
                            }

                            if (mileageForm.Hours.Length > 1)
                            {
                                this.AddUserMessage("Changing Hours to " + Hoursmodified);
                                Response<bool> mileagemodified = await this.Vehicle.UpdateHours(mileageForm.Hours.Trim());
                                if (mileagemodified.Value)
                                {
                                    this.AddUserMessage("Hours successfully updated to " + Hoursmodified);
                                    MessageBox.Show("Hours updated to " + Hoursmodified + " successfully.", "Good news.", MessageBoxButtons.OK);
                                }
                                else
                                {
                                    MessageBox.Show("Unable to change the Hours to " + Hoursmodified + ". Error: " + mileagemodified.Status, "Bad news.", MessageBoxButtons.OK);
                                }
                            }
                        }
                    
                    }

                    if (model == 9902)
                    {
                        string path = null;
                        int address = 0x002200;
                        string filename = "9902kernel.bin";
                        string dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        string exeDirectory = Path.GetDirectoryName(dir);
                        path = Path.Combine(exeDirectory, filename);


                        byte[] mileagekernel;
                        using (Stream stream = File.OpenRead(path))
                        {
                            mileagekernel = new byte[stream.Length];
                            int bytesRead = await stream.ReadAsync(mileagekernel, 0, (int)stream.Length);
                            if (bytesRead != stream.Length)
                            {
                                /// If this happens too much, we should try looping rather than reading the whole file in one shot.
                                this.AddUserMessage("Unable to load kernel.");
                                this.EnableUserInput();
                                return;
                            }
                        }

                        if (mileageForm.Hours.Length > 1)
                        {
                            

                            
                            uint inthours = uint.Parse(mileageForm.Hours, System.Globalization.NumberStyles.Integer);
                            
                            byte[] bhours = BitConverter.GetBytes(inthours);


                            mileagekernel[0xDB] = bhours[0];
                            mileagekernel[0xDA] = bhours[1];
                            mileagekernel[0xD9] = bhours[2];

                            mileagekernel[0xDF] = 0x01;



                            this.AddUserMessage("Changing Hours to " + Hoursmodified);
                            

                        }
                        if (mileageForm.Mileage.Length > 1)
                        {
                            Milesmodified = mileageForm.Mileage.Remove(mileageForm.Mileage.Length - 1);
                            uint intmiles = uint.Parse(Milesmodified, System.Globalization.NumberStyles.Integer);
                            
                            intmiles = intmiles * 2;
                            
                            byte[] bmiles = BitConverter.GetBytes(intmiles);


                            mileagekernel[0xEB] = bmiles[0];
                            mileagekernel[0xEA] = bmiles[0];
                            mileagekernel[0xE9] = bmiles[0];
                            mileagekernel[0xE8] = bmiles[0];
                            mileagekernel[0xE7] = bmiles[0];
                            mileagekernel[0xE6] = bmiles[0];
                            mileagekernel[0xE5] = bmiles[0];
                            mileagekernel[0xE4] = bmiles[0];
                            mileagekernel[0xE3] = bmiles[0];
                            mileagekernel[0xE2] = bmiles[0];

                            mileagekernel[0xE1] = bmiles[1];
                            mileagekernel[0xE0] = bmiles[2];

                            mileagekernel[0xDE] = 0x01;



                            this.AddUserMessage("Changing Miles to " + Milesmodified);
                        }

                        bool unlocked = await this.Vehicle.UnlockEcu(0x14);
                        if (!unlocked)
                        {
                            this.AddUserMessage("Unable to unlock IPC.");
                            this.EnableUserInput();
                            return;
                        }
                        CKernelWriterIpc writer = new CKernelWriterIpc(
                                    this.Vehicle,
                                    new Protocol(),
                                    currentWriteTypeIpc,
                                    this);

                        bool writeresponse = await writer.WriteKernel(
                            mileagekernel,address,model,
                            CancellationToken.None);
                        if (writeresponse != true)
                        {
                            MessageBox.Show("Unable to change the Mileage/Hours", "", MessageBoxButtons.OK);
                            this.EnableUserInput();
                            return;
                        }
                        if (mileageForm.Mileage.Length > 1)
                        {
                            this.AddUserMessage("Milage successfully updated to " + Milesmodified);

                        }
                        if (mileageForm.Hours.Length > 1)
                        {
                            this.AddUserMessage("Hours successfully updated to " + Hoursmodified);
                        
                        }
                       

                    }

                    if (model == 0209)
                    {
                        if (mileageForm.Mileage.Length > 1)
                        {
                            this.AddUserMessage("Changing Mileage to " + Milesmodified);
                            bool unlocked = await this.Vehicle.UnlockEcu(0x14);
                            if (!unlocked)
                            {
                                this.AddUserMessage("Unable to unlock IPC.");
                                this.EnableUserInput();
                                return;
                            }

                            Response<bool> mileagemodified = await this.Vehicle.UpdateMileage(mileageForm.Mileage.Trim());
                            if (mileagemodified.Value)
                            {
                                this.AddUserMessage("Mileage successfully updated to " + Milesmodified);
                               
                            }
                            else
                            {
                                MessageBox.Show("Unable to change the Mileage to " + Milesmodified + ". Error: " + mileagemodified.Status, "Bad news.", MessageBoxButtons.OK);
                            }
                        }
                    }
                }

                this.EnableUserInput();
            
            }
            catch (Exception exception)
            {
                this.AddUserMessage("Mileage or Hours change failed: " + exception.ToString());
            }
        }

        private async void Modify_options99_Click(object sender, EventArgs e)
        {
            try
            {
                uint osidResponse = 9902;
                

                PcmInfo info = new PcmInfo(osidResponse);

                var optionResponse = await this.Vehicle.QueryIPCoptions99();
                if (optionResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("IPC options query failed: " + optionResponse.Status.ToString());
                    this.EnableUserInput();
                    return;
                }

                DialogBoxes.OptionsForm optionForm = new DialogBoxes.OptionsForm(this.Vehicle, this, osidResponse);
                optionForm.Option = optionResponse.Value;
                DialogResult dialogResult = optionForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                    if (!unlocked)
                    {
                        this.AddUserMessage("Unable to unlock IPC.");
                        this.EnableUserInput();
                        return;
                    }

                    Response<bool> optionsmodified = await this.Vehicle.UpdateOptions99(optionForm.Option.Trim());
                    if (optionsmodified.Value)
                    {
                        this.AddUserMessage("Options successfully updated to " + optionForm.Option);
                        MessageBox.Show("Options updated to " + optionForm.Option + " successfully.", "Good news.", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Unable to change the Options to " + optionForm.Option + ". Error: " + optionsmodified.Status, "Bad news.", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddUserMessage("Options change failed: " + exception.ToString());
            }

            var option1Response = await this.Vehicle.QueryIPCoptions99();
            if (option1Response.Status != ResponseStatus.Success)
            {
                this.AddUserMessage("IPC options query failed: " + option1Response.Status.ToString());
                this.EnableUserInput();
                return;
            }

        }

        private async void testipc99_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {

                if (this.Vehicle == null)
                {
                    // This shouldn't be possible - it would mean the buttons 
                    // were enabled when they shouldn't be.
                    return;
                }

                try
                {
                    this.DisableUserInput();

                    this.AddUserMessage("IPC test lights.");
                    await this.Vehicle.Lights99();

                    this.AddUserMessage("IPC test prndl display.");
                    await this.Vehicle.PRNDL99();
                    
                    this.AddUserMessage("IPC test gauges.");
                    await this.Vehicle.SweepGauges9950();
                    System.Threading.Thread.Sleep(1500);
                    await this.Vehicle.SweepGauges99100();
                    System.Threading.Thread.Sleep(1500);
                    await this.Vehicle.SweepGauges9950();
                    System.Threading.Thread.Sleep(1500);
                    await this.Vehicle.SweepGauges9900();
                    
                    
                }
                catch (Exception exception)
                {
                    this.AddUserMessage(exception.Message);
                    this.AddDebugMessage(exception.ToString());
                }
                finally
                {
                    this.EnableUserInput();
                }
            }
        }

        private async void TestIPCTB_Click(object sender, EventArgs e)
        {
            if (!BackgroundWorker.IsAlive)
            {

                if (this.Vehicle == null)
                {
                    // This shouldn't be possible - it would mean the buttons 
                    // were enabled when they shouldn't be.
                    return;
                }

                try
                {
                    this.DisableUserInput();
                    this.AddUserMessage("IPC test gauges.");
                    await this.Vehicle.SweepGauges();
                    System.Threading.Thread.Sleep(750);

                    this.AddUserMessage("IPC test lights.");
                    await this.Vehicle.LEDson();
                    System.Threading.Thread.Sleep(750);

                    await this.Vehicle.Displayon();
                    this.AddUserMessage("IPC test display pixels.");
                    System.Threading.Thread.Sleep(750);

                    this.AddUserMessage("IPC test prndl display.");
                    await this.Vehicle.PRNDL03();
                }
                catch (Exception exception)
                {
                    this.AddUserMessage(exception.Message);
                    this.AddDebugMessage(exception.ToString());
                }
                finally
                {
                    this.EnableUserInput();
                }
            }
        }

        private async void ModifyOptionsTB_Click(object sender, EventArgs e)
        {
            try
            {
                uint osidResponse = 0209;


                PcmInfo info = new PcmInfo(osidResponse);

                var optionResponse = await this.Vehicle.QueryIPCoptions();
                if (optionResponse.Status != ResponseStatus.Success)
                {
                    this.AddUserMessage("IPC options query failed: " + optionResponse.Status.ToString());
                    return;
                }

                DialogBoxes.OptionsForm optionForm = new DialogBoxes.OptionsForm(this.Vehicle, this, osidResponse);
                optionForm.Option = optionResponse.Value;
                DialogResult dialogResult = optionForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
                    if (!unlocked)
                    {
                        this.AddUserMessage("Unable to unlock PCM.");
                        return;
                    }

                    Response<bool> optionsmodified = await this.Vehicle.UpdateOptions(optionForm.Option.Trim());
                    if (optionsmodified.Value)
                    {
                        this.AddUserMessage("Options successfully updated to " + optionForm.Option);
                        MessageBox.Show("Options updated to " + optionForm.Option + " successfully.", "Good news.", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Unable to change the Options to " + optionForm.Option + ". Error: " + optionsmodified.Status, "Bad news.", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception exception)
            {
                this.AddUserMessage("Options change failed: " + exception.ToString());
            }

            var option1Response = await this.Vehicle.QueryIPCoptions();
            if (option1Response.Status != ResponseStatus.Success)
            {
                this.AddUserMessage("IPC options query failed: " + option1Response.Status.ToString());
                return;
            }
        }
    }
}
