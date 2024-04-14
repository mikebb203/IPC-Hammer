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

            
            
            var speedometerl = Stepper.Substring(0, 3);
            var speedometeru = Stepper.Substring(3, 3);
            var tachometerl = Stepper.Substring(6, 3);
            var tachometeru = Stepper.Substring(9, 3);
            var fuell = Stepper.Substring(12, 2);
            var fuelu = Stepper.Substring(14, 2);
            var coolantl = Stepper.Substring(16, 2);
            var coolantu = Stepper.Substring(18, 2);
            var voltl = Stepper.Substring(20, 2);
            var voltu = Stepper.Substring(22, 2);
            var oill = Stepper.Substring(24, 2);
            var oilu = Stepper.Substring(26, 2);
            var transl = Stepper.Substring(28, 2);
            var transu = Stepper.Substring(30, 2);
            int speed1int = int.Parse(speedometerl, System.Globalization.NumberStyles.HexNumber);
            int tachint = int.Parse(tachometerl, System.Globalization.NumberStyles.HexNumber);
            int oilint = int.Parse(oill, System.Globalization.NumberStyles.HexNumber);
            int voltint = int.Parse(voltl, System.Globalization.NumberStyles.HexNumber);
            int fuelint = int.Parse(fuell, System.Globalization.NumberStyles.HexNumber);
            int coolantint = int.Parse(coolantl, System.Globalization.NumberStyles.HexNumber);
            int transint = int.Parse(transl, System.Globalization.NumberStyles.HexNumber);
            int speed1uint = int.Parse(speedometeru, System.Globalization.NumberStyles.HexNumber);
            int tachuint = int.Parse(tachometeru, System.Globalization.NumberStyles.HexNumber);
            int oiluint = int.Parse(oilu, System.Globalization.NumberStyles.HexNumber);
            int voltuint = int.Parse(voltu, System.Globalization.NumberStyles.HexNumber);
            int fueluint = int.Parse(fuelu, System.Globalization.NumberStyles.HexNumber);
            int coolantuint = int.Parse(coolantu, System.Globalization.NumberStyles.HexNumber);
            int transuint = int.Parse(transu, System.Globalization.NumberStyles.HexNumber);

            ///   this.speedometerBar.Value = speed1int;
            this.speedometerlowerBox.Text = speed1int.ToString("X3");
            this.speedometerupperBox.Text = speed1uint.ToString("X3");
            ///   this.tachBar.Value = tachint;
            this.tachometerlowerBox.Text = tachint.ToString("X3");
            this.tachometerupperBox.Text = tachuint.ToString("X3");
            ///   this.oilBar.Value = oilint;
            this.oillowerBox.Text = oilint.ToString("X2");
            this.oilupperBox.Text = oiluint.ToString("X2");
            ///   this.voltBar.Value = voltint;
            this.voltlowerBox.Text = voltint.ToString("X2");
            this.voltupperBox.Text = voltuint.ToString("X2");
            ///   this.fuelBar.Value = fuelint;
            this.fuellowerBox.Text = fuelint.ToString("X2");
            this.fuelupperBox.Text = fueluint.ToString("X2");
            ///   this.coolantBar.Value = coolantint;
            this.coolantlowerBox.Text = coolantint.ToString("X2");
            this.coolantupperBox.Text = coolantuint.ToString("X2");
            ///   this.transBar.Value = transint;
            this.transmissionlowerBox.Text = transint.ToString("X2");
            this.transmissionupperBox.Text = transuint.ToString("X2");
        
        }

        /// <summary>
        /// OK button click handler.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            
            this.Stepper = null;
            this.DialogResult = DialogResult.OK;
        }



        ///   private void speedometer_Scroll(object sender, EventArgs e)
        ///   {

        ///       int speed1int = this.speedometerBar.Value ;
        ///       this.speedometerBox.Text = speed1int.ToString("X6");
        ///   }

        ///   private void tachBar_Scroll(object sender, EventArgs e)
        ///   {

        ///    int tachint = this.tachBar.Value;
        ///       this.tachometerBox.Text = tachint.ToString("X6");
        ///   }

        ///     private void oilBar_Scroll(object sender, EventArgs e)
        ///   {

        ///       int oilint = this.oilBar.Value;
        ///       this.oilBox.Text = oilint.ToString("X4");
        ///   }

        ///       private void voltBar_Scroll(object sender, EventArgs e)
        ///   {

        ///       int voltint = this.voltBar.Value;
        ///       this.voltBox.Text = voltint.ToString("X4");
        ///   }

        ///       private void fuelBar_Scroll(object sender, EventArgs e)
        ///   {

        ///       int fuelint = this.fuelBar.Value;
        ///       this.fuelBox.Text = fuelint.ToString("X4");
        ///   }

        ///private void coolantBar_Scroll(object sender, EventArgs e)
        ///   {

        ///       int coolantint = this.coolantBar.Value;
        ///       this.coolantBox.Text = coolantint.ToString("X4");
        ///   }

        ///private void transBar_Scroll(object sender, EventArgs e)
        ///   {

        ///       int transint = this.transBar.Value;
        ///       this.transmissionBox.Text = transint.ToString("X4");
        ///   }

        private void speedometerlowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var speedint1 = int.Parse(speedometerlowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.speedometerBar.Value = speedint1;
        }

        private void tachometerlowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var tachint1 = int.Parse(tachometerlowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.tachBar.Value = tachint1;
        }

        private void oillowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var oilint1 = int.Parse(oillowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.oilBar.Value = oilint1;
        }

        private void voltlowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var voltint1 = int.Parse(voltlowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.voltBar.Value = voltint1;
        }

        private void fuellowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var fuelint1 = int.Parse(fuellowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.fuelBar.Value = fuelint1;
        }

        private void coolantlowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var coolantint1 = int.Parse(coolantlowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.coolantBar.Value = coolantint1;
        }

        private void transmissionlowerBox_TextChanged(object sender, EventArgs e)
        {
            ///var transmissionint1 = int.Parse(transmissionlowerBox.Text, System.Globalization.NumberStyles.HexNumber);
            ///   this.transBar.Value = transmissionint1;
        }

        private async void updatecal_Click(object sender, EventArgs e)
        {

            var Speedometer = string.Concat(speedometerlowerBox.Text, speedometerupperBox.Text);
            var Tachometer = string.Concat(tachometerlowerBox.Text, tachometerupperBox.Text);
            var Oil = string.Concat(oillowerBox.Text, oilupperBox.Text);
            var Volt = string.Concat(voltlowerBox.Text, voltupperBox.Text);
            var Fuel = string.Concat(fuellowerBox.Text, fuelupperBox.Text);
            var Coolant = string.Concat(coolantlowerBox.Text, coolantupperBox.Text);
            var Trans = string.Concat(transmissionlowerBox.Text, transmissionupperBox.Text);

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

        private async void TestSweep_Click(object sender, EventArgs e)
        {
            
            await this.Vehicle.SweepGauges();
        }

        private void speedometerupperBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void tachometerupperBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void oilupperBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void voltupperBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void fuelupperBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void coolantupperBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void transmissionupperBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
        