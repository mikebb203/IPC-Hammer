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
    public partial class A7k140mphForm : Form
    {
        /// <summary>
        /// This will be copied into the text box when the dialog box appears.
        /// When the dialog closes, if the user provided a valid VIN it will
        /// be returned via this property. If they didn't, this will be null.
        /// </summary>
        public Boolean Tach { get; set; }
        public Boolean Speedo { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        public A7k140mphForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load event handler.
        /// </summary>
        private void A7k140mphForm_Load(object sender, EventArgs e)
        {
                
            
        }

        /// <summary>
        /// OK button click handler.
        /// </summary>
        private void okButton_Click(object sender, EventArgs e)
        {

            this.Tach = this.Tach;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel button click handler.
        /// </summary>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            

            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (this.checkBox1.Checked == true)
            {
                this.Tach = true;
            }
            else
            {
                this.Tach = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

            if (this.checkBox2.Checked == true)
            {
                this.Speedo = true;
            }
            else
            {
                this.Speedo = false;
            }
        }
    }
}
