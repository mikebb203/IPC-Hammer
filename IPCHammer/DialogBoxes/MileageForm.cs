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
    public partial class MileageForm : Form
    {
        /// <summary>
        /// This will be copied into the text box when the dialog box appears.
        /// When the dialog closes, if the user provided a valid VIN it will
        /// be returned via this property. If they didn't, this will be null.
        /// </summary>
        public string Mileage { get; set; }
        public string Hours { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        public MileageForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load event handler.
        /// </summary>
        private void MileageForm_Load(object sender, EventArgs e)
        {
            this.mileageBox.Text = this.Mileage;
            this.hoursBox.Text = this.Hours;
            this.mileagetenthsBox.Text = "0";
            this.hourstenthsBox.Text = "0";
            this.mileageBox_TextChanged(null, null);
            this.mileagetenthsBox_TextChanged(null, null);
            this.hoursBox_TextChanged(null, null);
            this.hourstenthsBox_TextChanged(null, null);
        
        }

        /// <summary>
        /// OK button click handler.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (!this.IsLegalMiles())
            {
                return;
            }

            if (!this.IsLegalMilesTenths())
            {
                return;
            }

            if (!this.IsLegalHours())
            {
                return;
            }

            if (!this.IsLegalHoursTenths())
            {
                return;
            }
            this.Mileage = string.Concat(this.mileageBox.Text, this.mileagetenthsBox.Text);
            this.Hours = string.Concat(this.hoursBox.Text, hourstenthsBox.Text);
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel button click handler.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Mileage = null;

            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Validate the new VIN every time it changes.
        /// </summary>
        private void mileageBox_TextChanged(object sender, EventArgs e)
        {
          this.okButton.Enabled = this.IsLegalMiles();
        }

        private void mileagetenthsBox_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = this.IsLegalMilesTenths();
        }

        private void hoursBox_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = this.IsLegalHours();
        }

        private void hourstenthsBox_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = this.IsLegalHoursTenths();
        }
        /// <summary>
        /// Validate the VIN.
        /// </summary>
        private bool IsLegalMiles()
        {

            if (this.mileageBox.Text.Length >= 7)
            {
                this.prompt.Text = $"Mileage should be no more than 6. {this.mileageBox.Text.Length}.";
                return false;
            }

            if (!MileageForm.IsNumeric(this.mileageBox.Text))
            {
                this.prompt.Text = "The mileage must contain only numbers.";
                return false;
            }

            return true;
        }

        private bool IsLegalHours()
        {

            if (this.hoursBox.Text.Length >= 6)
            {
                this.prompt.Text = $"Hours should be no more than 4. {this.hoursBox.Text.Length}.";
                return false;
            }

            if (!MileageForm.IsNumeric(this.hoursBox.Text))
            {
                this.prompt.Text = "The hours must contain only numbers.";
                return false;
            }

            return true;
        }

        private bool IsLegalMilesTenths()
        {

            if (this.mileagetenthsBox.Text.Length >= 2)
            {
                this.prompt.Text = $"Mileage tenths should be no more than 1. {this.mileagetenthsBox.Text.Length}.";
                return false;
            }

            if (!MileageForm.IsNumeric(this.mileagetenthsBox.Text))
            {
                this.prompt.Text = "The mileage tenths must contain only numbers.";
                return false;
            }

            return true;
        }

        private bool IsLegalHoursTenths()
        {

            if (this.hourstenthsBox.Text.Length >= 2)
            {
                this.prompt.Text = $"Hours tenths should be no more than 1. {this.hourstenthsBox.Text.Length}.";
                return false;
            }

            if (!MileageForm.IsNumeric(this.hourstenthsBox.Text))
            {
                this.prompt.Text = "The hours tenths must contain only numbers.";
                return false;
            }

            return true;
        }
        /// <summary>
        /// Find out if the whole VIN string is alphanumeric.
        /// </summary>
        private static bool IsNumeric(string vin)
        {
            foreach(char c in vin)
            {
                if(!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

       
    }
}
