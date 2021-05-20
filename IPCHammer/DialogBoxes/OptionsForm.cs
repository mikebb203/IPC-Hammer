using System;
using System.Collections;
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
    public partial class OptionsForm : Form
    {
        /// <summary>
        /// This will be copied into the text box when the dialog box appears.
        /// When the dialog closes, if the user provided a valid VIN it will
        /// be returned via this property. If they didn't, this will be null.
        /// </summary>
        public string Option { get; set; }
        private Vehicle vehicle;
        protected Vehicle Vehicle { get { return this.vehicle; } }
        private readonly ILogger logger;
      

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsForm(Vehicle vehicle, ILogger logger)
        {
            InitializeComponent();
            this.vehicle = vehicle;
            this.logger = logger;
        }

        
        /// <summary>
        /// Load event handler.
        /// </summary>
        private void OptionsForm_Load(object sender, EventArgs e)
        {
            
            this.textBox1.Text = this.Option;
            var option1 = Option.Substring(0, 2);
            var option2 = Option.Substring(2, 2);
            byte option1byte = byte.Parse(option1, System.Globalization.NumberStyles.HexNumber);
            byte option2byte = byte.Parse(option2, System.Globalization.NumberStyles.HexNumber);
            this.textBox2.Text = option1byte.ToString("X2");
            this.textBox3.Text = option2byte.ToString("X2");
            

        }

        private void OK_Click(object sender, EventArgs e)
        {
            //this.Option = null;
            this.Option = this.textBox1.Text;
            this.DialogResult = DialogResult.OK;

        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Option = null;

            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
       
        }


    }
}
