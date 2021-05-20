using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcmHacking.DialogBoxes
{
    /// <summary>
    /// Prompt the user to enter a valid VIN.
    /// </summary>
    public partial class StepperForm : Form
    {

        /// <summary>
        /// This will be copied into the text box when the dialog box appears.
        /// When the dialog closes, if the user provided a valid VIN it will
        /// be returned via this property. If they didn't, this will be null.
        /// </summary>
        public string Stepper { get; set; }
        private Vehicle vehicle;
        protected Vehicle Vehicle { get { return this.vehicle; } }
        private readonly ILogger logger;


        /// <summary>
        /// Constructor.
        /// </summary>
        public StepperForm(Vehicle vehicle, ILogger logger)
        {
            InitializeComponent();
            this.vehicle = vehicle;
            this.logger = logger;
        }

        /// <summary>
        /// Load event handler.
        /// </summary>
      
        private void StepperForm_Load(object sender, EventArgs e)
        {

            
            
            var speedometer = Stepper.Substring(0, 6);
            var tachometer = Stepper.Substring(6, 6);
            var fuel = Stepper.Substring(12, 4);
            var coolant = Stepper.Substring(16, 4);
            var volt = Stepper.Substring(20, 4);
            var oil = Stepper.Substring(24, 4);
            var trans = Stepper.Substring(28, 4);
            int speed1int = int.Parse(speedometer, System.Globalization.NumberStyles.HexNumber);
            int tachint = int.Parse(tachometer, System.Globalization.NumberStyles.HexNumber);
            int oilint = int.Parse(oil, System.Globalization.NumberStyles.HexNumber);
            int voltint = int.Parse(volt, System.Globalization.NumberStyles.HexNumber);
            int fuelint = int.Parse(fuel, System.Globalization.NumberStyles.HexNumber);
            int coolantint = int.Parse(coolant, System.Globalization.NumberStyles.HexNumber);
            int transint = int.Parse(trans, System.Globalization.NumberStyles.HexNumber);


            this.speedometerBar.Value = speed1int;
            this.speedometerBox.Text = speed1int.ToString("X6");
            this.tachBar.Value = tachint;
            this.tachometerBox.Text = tachint.ToString("X6");
            this.oilBar.Value = oilint;
            this.oilBox.Text = oilint.ToString("X4");
            this.voltBar.Value = voltint;
            this.voltBox.Text = voltint.ToString("X4");
            this.fuelBar.Value = fuelint;
            this.fuelBox.Text = fuelint.ToString("X4");
            this.coolantBar.Value = coolantint;
            this.coolantBox.Text = coolantint.ToString("X4");
            this.transBar.Value = transint;
            this.transmissionBox.Text = transint.ToString("X4");

        }

        /// <summary>
        /// OK button click handler.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            
            this.Stepper = null;
            this.DialogResult = DialogResult.OK;
        }

        

        private void speedometer_Scroll(object sender, EventArgs e)
        {
            
            int speed1int = this.speedometerBar.Value ;
            this.speedometerBox.Text = speed1int.ToString("X6");
        }

        private void tachBar_Scroll(object sender, EventArgs e)
        {
            
            int tachint = this.tachBar.Value;
            this.tachometerBox.Text = tachint.ToString("X6");
        }

        private void oilBar_Scroll(object sender, EventArgs e)
        {
            
            int oilint = this.oilBar.Value;
            this.oilBox.Text = oilint.ToString("X4");
        }

        private void voltBar_Scroll(object sender, EventArgs e)
        {
            
            int voltint = this.voltBar.Value;
            this.voltBox.Text = voltint.ToString("X4");
        }

        private void fuelBar_Scroll(object sender, EventArgs e)
        {
            
            int fuelint = this.fuelBar.Value;
            this.fuelBox.Text = fuelint.ToString("X4");
        }

        private void coolantBar_Scroll(object sender, EventArgs e)
        {
            
            int coolantint = this.coolantBar.Value;
            this.coolantBox.Text = coolantint.ToString("X4");
        }

        private void transBar_Scroll(object sender, EventArgs e)
        {
            
            int transint = this.transBar.Value;
            this.transmissionBox.Text = transint.ToString("X4");
        }

        private void speedometerBox_TextChanged(object sender, EventArgs e)
        {
            var speedint1 = int.Parse(speedometerBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.speedometerBar.Value = speedint1;
        }

        private void tachometerBox_TextChanged(object sender, EventArgs e)
        {
            var tachint1 = int.Parse(tachometerBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.tachBar.Value = tachint1;
        }

        private void oilBox_TextChanged(object sender, EventArgs e)
        {
            var oilint1 = int.Parse(oilBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.oilBar.Value = oilint1;
        }

        private void voltBox_TextChanged(object sender, EventArgs e)
        {
            var voltint1 = int.Parse(voltBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.voltBar.Value = voltint1;
        }

        private void fuelBox_TextChanged(object sender, EventArgs e)
        {
            var fuelint1 = int.Parse(fuelBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.fuelBar.Value = fuelint1;
        }

        private void coolantBox_TextChanged(object sender, EventArgs e)
        {
            var coolantint1 = int.Parse(coolantBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.coolantBar.Value = coolantint1;
        }

        private void transmissionBox_TextChanged(object sender, EventArgs e)
        {
            var transmissionint1 = int.Parse(transmissionBox.Text, System.Globalization.NumberStyles.HexNumber);
            this.transBar.Value = transmissionint1;
        }

        private async void updatecal_Click(object sender, EventArgs e)
        {

            var Speedometer = speedometerBox.Text;
            var Tachometer = tachometerBox.Text;
            var Oil = oilBox.Text;
            var Volt = voltBox.Text;
            var Fuel = fuelBox.Text;
            var Coolant = coolantBox.Text;
            var Trans = transmissionBox.Text;

            Stepper = string.Concat(Speedometer, Tachometer, Fuel, Coolant, Volt, Oil, Trans);

            Response<uint> osidResponse = await this.Vehicle.QueryOperatingSystemId(CancellationToken.None);
            if (osidResponse.Status != ResponseStatus.Success)
            {
                this.logger.AddUserMessage("Operating system query failed: " + osidResponse.Status);
                return;
            }

            PcmInfo info = new PcmInfo(osidResponse.Value);

            bool unlocked = await this.Vehicle.UnlockEcu(info.KeyAlgorithm);
            if (!unlocked)
            {
                this.logger.AddUserMessage("Unable to unlock PCM.");
                return;
            }

            Response<bool> vinmodified = await this.Vehicle.UpdateStepperCals(Stepper.Trim());
            if (vinmodified.Value)
            {
                this.logger.AddUserMessage("Stepper Calibration successfully updated to " + Stepper);
                MessageBox.Show("Stepper Calibration updated to " + Stepper + " successfully.", "Good news.", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Unable to change the Stepper Calibration to " + Stepper + ". Error: " + vinmodified.Status, "Bad news.", MessageBoxButtons.OK);
            }



        }

       

    }
}
        