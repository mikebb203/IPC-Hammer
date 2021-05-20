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
            
            this.mileageBox_TextChanged(null, null);
        }

        /// <summary>
        /// OK button click handler.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (!this.IsLegal())
            {
                return;
            }

            this.Mileage = this.mileageBox.Text;

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
          this.okButton.Enabled = this.IsLegal();
        }

        /// <summary>
        /// Validate the VIN.
        /// </summary>
        private bool IsLegal()
        {

            if (this.mileageBox.Text.Length >= 8)
            {
                this.prompt.Text = $"Mileage should be no more than 7 digits including tenths digit. {this.mileageBox.Text.Length}.";
                return false;
            }

            if (!MileageForm.IsNumeric(this.mileageBox.Text))
            {
                this.prompt.Text = "The mileage must contain only numbers.";
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
