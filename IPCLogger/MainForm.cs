﻿//#define Vpw4x

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcmHacking
{
    public partial class MainForm : MainFormBase
    {
        private LogProfileAndMath profileAndMath;
        private bool logging;
        private object loggingLock = new object();
        private bool logStopRequested;
        private string profileName;
        private TaskScheduler uiThreadScheduler;
        private static DateTime lastLogTime;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="message"></param>
        public override void AddUserMessage(string message)
        {
        }

        /// <summary>
        /// Add a message to the debug pane of the main window.
        /// </summary>
        public override void AddDebugMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("hh:mm:ss:fff");

            Task foreground = Task.Factory.StartNew(
                delegate ()
                {
                    try
                    {
                        this.debugLog.AppendText("[" + timestamp + "]  " + message + Environment.NewLine);
                    }
                    catch (ObjectDisposedException)
                    {
                        // This will happen if the window is closing. Just ignore it.
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                uiThreadScheduler);
        }

        public override void ResetLogs()
        {
            this.debugLog.Clear();
        }

        public override string GetAppNameAndVersion()
        {
            return "PCM Logger";
        }

        protected override void DisableUserInput()
        {
            this.selectButton.Enabled = false;
            this.selectProfileButton.Enabled = false;
            this.startStopLogging.Enabled = false;
        }

        protected override void EnableInterfaceSelection()
        {
            this.selectButton.Enabled = true;
        }

        protected override void EnableUserInput()
        {
            this.selectButton.Enabled = true;
            this.selectProfileButton.Enabled = true;
            this.startStopLogging.Enabled = true;
            this.startStopLogging.Focus();
        }

        protected override void NoDeviceSelected()
        {
            this.selectButton.Enabled = true;
            this.deviceDescription.Text = "No device selected";
        }

        protected override void ValidDeviceSelected(string deviceName)
        {
            this.deviceDescription.Text = deviceName;
        }

        /// <summary>
        /// Open the last device, if possible.
        /// </summary>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            this.uiThreadScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            await this.ResetDevice();
            string profilePath = LoggerConfiguration.ProfilePath;
            if (!string.IsNullOrEmpty(profilePath))
            {
                await this.LoadProfile(profilePath);
            }
            
            string logDirectory = LoggerConfiguration.LogDirectory;
            if (string.IsNullOrWhiteSpace(logDirectory))
            {
                logDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                LoggerConfiguration.LogDirectory = logDirectory;
            }

            this.logFilePath.Text = logDirectory;
        }

        /// <summary>
        /// Select which interface device to use. This opens the Device-Picker dialog box.
        /// </summary>
        protected async void selectButton_Click(object sender, EventArgs e)
        {
            await this.HandleSelectButtonClick();
            this.UpdateStartStopButtonState();
        }

        /// <summary>
        /// Select a logging profile.
        /// </summary>
        private async void selectProfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.AutoUpgradeEnabled = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = ".profile";
            dialog.Multiselect = false;
            dialog.ValidateNames = true;
            dialog.Filter = "Logging profiles (*.profile)|*.profile";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await this.LoadProfile(dialog.FileName);
            }
            else
            {
                this.profileAndMath = null;
                this.profileName = null;
            }

            this.UpdateStartStopButtonState();
        }

        /// <summary>
        /// Load the profile from the given path.
        /// </summary>
        private async Task LoadProfile(string path)
        {
            try
            {
                LogProfile profile;
                if (path.EndsWith(".json.profile"))
                {
                    using (Stream stream = File.OpenRead(path))
                    {
                        LogProfileReader reader = new LogProfileReader(stream);
                        profile = await reader.ReadAsync();
                    }

                    string newPath = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path)) + ".xml.profile";
                    using (Stream xml = File.OpenWrite(newPath))
                    {
                        LogProfileXmlWriter writer = new LogProfileXmlWriter(xml);
                        writer.Write(profile);
                    }
                }
                else if (path.EndsWith(".xml.profile"))
                {
                    using (Stream stream = File.OpenRead(path))
                    {
                        LogProfileXmlReader reader = new LogProfileXmlReader(stream);
                        profile = reader.Read();
                    }
                }
                else
                {
                    return;
                }

                this.profilePath.Text = path;
                this.profileName = Path.GetFileNameWithoutExtension(this.profilePath.Text);

                MathValueConfigurationLoader loader = new MathValueConfigurationLoader(this);
                loader.Initialize();
                this.profileAndMath = new LogProfileAndMath(profile, loader.Configuration);
                this.logValues.Text = string.Join(Environment.NewLine, this.profileAndMath.GetColumnNames());
                LoggerConfiguration.ProfilePath = path;
            }
            catch (Exception exception)
            {
                this.logValues.Text = exception.Message;
                this.AddDebugMessage(exception.ToString());
                this.profilePath.Text = "[no profile loaded]";
                this.profileName = null;
            }
        }

        /// <summary>
        /// Choose which directory to create log files in.
        /// </summary>
        private void setDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = LoggerConfiguration.LogDirectory;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                LoggerConfiguration.LogDirectory = dialog.SelectedPath;
                this.logFilePath.Text = dialog.SelectedPath;
            }
        }

        /// <summary>
        /// Open a File Explorer window in the log directory.
        /// </summary>
        private void openDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(LoggerConfiguration.LogDirectory);
        }

        /// <summary>
        /// Enable or disble the start/stop button.
        /// </summary>
        private void UpdateStartStopButtonState()
        {
            this.startStopLogging.Enabled = this.Vehicle != null && this.profileAndMath != null;
        }

        /// <summary>
        /// Start or stop logging.
        /// </summary>
        private void startStopLogging_Click(object sender, EventArgs e)
        {
            if (logging)
            {
                this.logStopRequested = true;
                this.startStopLogging.Enabled = false;
                this.startStopLogging.Text = "Start &Logging";
            }
            else
            {
                lock (loggingLock)
                {
                    if (this.profileAndMath == null)
                    {
                        this.logValues.Text = "Please select a log profile.";
                        return;
                    }

                    if (!logging)
                    {
                        logging = true;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(LoggingThread), null);
                        this.startStopLogging.Text = "Stop &Logging";
                    }
                }
            }
        }

        /// <summary>
        /// The loop that reads data from the PCM.
        /// </summary>
        private async void LoggingThread(object threadContext)
        {
            using (AwayMode lockScreenSuppressor = new AwayMode())
            {
                try
                {
                    string logFilePath = GenerateLogFilePath();

                    this.loggerProgress.Invoke(
                    (MethodInvoker)
                    delegate ()
                    {
                        this.loggerProgress.Value = 0;
                        this.loggerProgress.Visible = true;
                        this.logFilePath.Text = logFilePath;
                        this.setDirectory.Enabled = false;
						this.startStopLogging.Focus();
                    });

                    MathValueConfigurationLoader loader = new MathValueConfigurationLoader(this);
                    loader.Initialize();
                    Logger logger = new Logger(this.Vehicle, this.profileAndMath, loader.Configuration);
                    if (!await logger.StartLogging())
                    {
                        this.AddUserMessage("Unable to start logging.");
                        return;
                    }

#if Vpw4x
                if (!await this.Vehicle.VehicleSetVPW4x(VpwSpeed.FourX))
                {
                    this.AddUserMessage("Unable to switch to 4x.");
                    return;
                }
#endif
                    using (StreamWriter streamWriter = new StreamWriter(logFilePath))
                    {
                        LogFileWriter writer = new LogFileWriter(streamWriter);
                        IEnumerable<string> columnNames = this.profileAndMath.GetColumnNames();
                        await writer.WriteHeader(columnNames);

                        lastLogTime = DateTime.Now;

                        this.loggerProgress.Invoke(
                            (MethodInvoker)
                            delegate ()
                            {
                                this.loggerProgress.MarqueeAnimationSpeed = 150;
                                this.selectButton.Enabled = false;
                                this.selectProfileButton.Enabled = false;
                            });

                        while (!this.logStopRequested)
                        {
                            this.AddDebugMessage("Requesting row...");
                            IEnumerable<string> rowValues = await logger.GetNextRow();
                            if (rowValues == null)
                            {
                                continue;
                            }

                            // Write the data to disk on a background thread.
                            Task background = Task.Factory.StartNew(
                                delegate ()
                                {
                                    writer.WriteLine(rowValues);
                                });

                            // Display the data using a foreground thread.
                            Task foreground = Task.Factory.StartNew(
                                delegate ()
                                {
                                    string formattedValues = FormatValuesForTextBox(rowValues);
                                    this.logValues.Text = string.Join(Environment.NewLine, formattedValues);
                                },
                                CancellationToken.None,
                                TaskCreationOptions.None,
                                uiThreadScheduler);
                        }
                    }
                }
                catch (Exception exception)
                {
                    this.AddDebugMessage(exception.ToString());
                    this.AddUserMessage("Logging interrupted. " + exception.Message);
                    this.logValues.Invoke(
                        (MethodInvoker)
                        delegate ()
                        {
                            this.logValues.Text = "Logging interrupted. " + exception.Message;
							this.startStopLogging.Focus();
                        });
                }
                finally
                {
#if Vpw4x
                if (!await this.Vehicle.VehicleSetVPW4x(VpwSpeed.Standard))
                {
                    // Try twice...
                    await this.Vehicle.VehicleSetVPW4x(VpwSpeed.Standard);
                }
#endif
                    this.logStopRequested = false;
                    this.logging = false;
                    this.startStopLogging.Invoke(
                        (MethodInvoker)
                        delegate ()
                        {
                            this.loggerProgress.MarqueeAnimationSpeed = 0;
                            this.loggerProgress.Visible = false;
                            this.startStopLogging.Enabled = true;
                            this.startStopLogging.Text = "Start &Logging";
                            this.logFilePath.Text = LoggerConfiguration.LogDirectory;
                            this.setDirectory.Enabled = true;

                            this.selectButton.Enabled = true;
                            this.selectProfileButton.Enabled = true;
							this.startStopLogging.Focus();
                        });
                }
            }
        }

        /// <summary>
        /// Generate a file name for the current log file.
        /// </summary>
        private string GenerateLogFilePath()
        {
            string file = DateTime.Now.ToString("yyyyMMdd_HHmm") +
                "_" +
                this.profileName +
                ".csv";
            return Path.Combine(LoggerConfiguration.LogDirectory, file);
        }

        /// <summary>
        /// Create a string that will look reasonable in the UI's main text box.
        /// TODO: Use a grid instead.
        /// </summary>
        private string FormatValuesForTextBox(IEnumerable<string> rowValues)
        {
            StringBuilder builder = new StringBuilder();
            IEnumerator<string> rowValueEnumerator = rowValues.GetEnumerator();
            foreach(ParameterGroup group in this.profileAndMath.Profile.ParameterGroups)
            {
                foreach(ProfileParameter parameter in group.Parameters)
                {
                    rowValueEnumerator.MoveNext();
                    builder.Append(rowValueEnumerator.Current);
                    builder.Append('\t');
                    builder.Append(parameter.Conversion.Name);
                    builder.Append('\t');
                    builder.AppendLine(parameter.Name);
                }
            }

            foreach(MathValue mathValue in this.profileAndMath.MathValueProcessor.GetMathValues())
            {
                rowValueEnumerator.MoveNext();
                builder.Append(rowValueEnumerator.Current);
                builder.Append('\t');
                builder.Append(mathValue.Units);
                builder.Append('\t');
                builder.AppendLine(mathValue.Name);
            }

            DateTime now = DateTime.Now;
            builder.AppendLine((now - lastLogTime).TotalMilliseconds.ToString("0.00") + "\tms\tQuery time");
            lastLogTime = now;

            return builder.ToString();
        }
    }
}
