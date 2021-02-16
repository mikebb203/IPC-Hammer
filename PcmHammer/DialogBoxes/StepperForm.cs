using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public string Speedometer { get; set; }
        public string Tachometer { get; set; }
        public string Oil { get; set; }
        public string Fuel { get; set; }
        public string Coolant { get; set; }
        public string Volt { get; set; }
        public string Trans { get; set; }




        /// <summary>
        /// Constructor.
        /// </summary>
        public StepperForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load event handler.
        /// </summary>
        private void StepperForm_Load(object sender, EventArgs e)
        {
                        
            this.speedometerBox.Text = this.Speedometer;
            this.tachometerBox.Text = this.Tachometer;
            this.oilBox.Text = this.Oil;
            this.voltBox.Text = this.Volt;
            this.fuelBox.Text = this.Fuel;
            this.coolantBox.Text = this.Coolant;
            this.transmissionBox.Text = this.Trans;

        }

        /// <summary>
        /// OK button click handler.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {

            this.Speedometer = this.speedometerBox.Text;
            this.Tachometer = this.tachometerBox.Text;
            this.Oil = this.oilBox.Text;
            this.Volt = this.voltBox.Text;
            this.Fuel = this.fuelBox.Text;
            this.Coolant = this.coolantBox.Text;
            this.Trans = this.transmissionBox.Text;

            Stepper = string.Concat(this.Speedometer, this.Tachometer, this.Oil, this.Fuel, this.Coolant, this.Volt, this.Trans);

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel button click handler.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Speedometer = null;
            this.Tachometer = null;
            this.Oil = null;
            this.Volt = null;
            this.Fuel = null;
            this.Coolant = null;
            this.Trans = null;

            this.DialogResult = DialogResult.Cancel;
        }
    }

}
        