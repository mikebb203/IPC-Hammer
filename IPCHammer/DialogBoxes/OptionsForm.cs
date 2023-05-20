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
        private uint osidResponse;
        protected Vehicle Vehicle { get { return this.vehicle; } }
        private readonly ILogger logger;
        

        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsForm(Vehicle vehicle, ILogger logger, uint osidResponse)
        {
            InitializeComponent();
            this.vehicle = vehicle;
            this.logger = logger;
            this.osidResponse = osidResponse;
        }


        /// <summary>
        /// Load event handler.
        /// </summary>
        private void OptionsForm_Load(object sender, EventArgs e)
        {

            this.textBox1.Text = this.Option;
            ///var option1 = Option.Substring(0, 2);
            ///var option2 = Option.Substring(2, 2);
            ///byte option1byte = byte.Parse(option1, System.Globalization.NumberStyles.HexNumber);
            ///byte option2byte = byte.Parse(option2, System.Globalization.NumberStyles.HexNumber);
            ///this.textBox2.Text = option1byte.ToString("X2");
            ///this.textBox3.Text = option2byte.ToString("X2");
            


            ///BitArray option1bits = new BitArray(BitConverter.GetBytes(option1byte).ToArray());
            ///BitArray option2bits = new BitArray(BitConverter.GetBytes(option2byte).ToArray());

            ///checkBox1.Checked = option1bits[7];
            ///checkBox2.Checked = option1bits[6];
            ///checkBox3.Checked = option1bits[5];
            ///checkBox4.Checked = option1bits[4];
            ///checkBox5.Checked = option1bits[3];
            ///checkBox6.Checked = option1bits[2];
            ///checkBox7.Checked = option1bits[1];
            ///checkBox8.Checked = option1bits[0];
            
            ///checkBox9.Checked = option2bits[7];
            ///checkBox10.Checked = option2bits[6];
            ///checkBox11.Checked = option2bits[5];
            ///checkBox12.Checked = option2bits[4];
            ///checkBox13.Checked = option2bits[3];
            ///checkBox14.Checked = option2bits[2];
            ///checkBox15.Checked = option2bits[1];
            ///checkBox16.Checked = option2bits[0];

            switch (osidResponse)
            {
                case 0307:
                    label1.Text = "";
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    label8.Text = "";
                    label9.Text = "05-07 only Checked PRN Unchecked PRNDL";
                    label10.Text = "";
                    label11.Text = "Checked KM Unchecked Miles";
                    label12.Text = "05-07 only Checked 140mph Unchecked 120mph";
                    label13.Text = "Checked 5k tach Unchecked 6k tach";
                    label14.Text = "Checked = prndl off Unchecked = prndl on";
                    label15.Text = "";
                    label16.Text = "Checked = no Trans Temp";
                    break;

                case 9902:
                    label1.Text = "";
                    label2.Text = "";
                    label3.Text = "checked without Trans temp";
                    label4.Text = "";
                    label5.Text = "unchecked 100mph checked 120mph";
                    label6.Text = "";
                    label7.Text = "";
                    label8.Text = "";
                    label9.Text = "";
                    label10.Text = "unchecked KM checked Miles";
                    label11.Text = "";
                    label12.Text = "checked = Trans Temp Gauge";
                    label13.Text = "unchecked 5k Tach checked 6k Tach";
                    label14.Text = "";
                    label15.Text = "";
                    label16.Text = "";
                    break;

                case 0209:
                    label1.Text = "";
                    label2.Text = "";
                    label3.Text = "";
                    label4.Text = "";
                    label5.Text = "";
                    label6.Text = "";
                    label7.Text = "";
                    label8.Text = "";
                    label9.Text = "";
                    label10.Text = "";
                    label11.Text = "checked KM unchecked miles";
                    label12.Text = "checked DIC unchecked NonDIC";
                    label13.Text = "";
                    label14.Text = "";
                    label15.Text = "";
                    label16.Text = "06+ checked = 140MPH/8k tach";
                    break;
            }
            
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
            BitArray optioncheckbits = new BitArray(8); 
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;
            
            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox1.Checked;
            optioncheckbits[6] = checkBox2.Checked;
            optioncheckbits[5] = checkBox3.Checked;
            optioncheckbits[4] = checkBox4.Checked;
            optioncheckbits[3] = checkBox5.Checked;
            optioncheckbits[2] = checkBox6.Checked;
            optioncheckbits[1] = checkBox7.Checked;
            optioncheckbits[0] = checkBox8.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox2.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }
        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            BitArray optioncheckbits = new BitArray(8);
            optioncheckbits[7] = checkBox9.Checked;
            optioncheckbits[6] = checkBox10.Checked;
            optioncheckbits[5] = checkBox11.Checked;
            optioncheckbits[4] = checkBox12.Checked;
            optioncheckbits[3] = checkBox13.Checked;
            optioncheckbits[2] = checkBox14.Checked;
            optioncheckbits[1] = checkBox15.Checked;
            optioncheckbits[0] = checkBox16.Checked;

            byte[] bytes = new byte[1];
            optioncheckbits.CopyTo(bytes, 0);
            byte optioncheckbyte;
            optioncheckbyte = bytes[0];
            this.textBox3.Text = optioncheckbyte.ToString("X2");
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox1.TextLength == 4)
            {
                var option1 = this.textBox1.Text.Substring(0, 2);
                var option2 = this.textBox1.Text.Substring(2, 2);
                byte option1byte = byte.Parse(option1, System.Globalization.NumberStyles.HexNumber);
                byte option2byte = byte.Parse(option2, System.Globalization.NumberStyles.HexNumber);
                this.textBox2.Text = option1byte.ToString("X2");
                this.textBox3.Text = option2byte.ToString("X2");

                BitArray option1bits = new BitArray(BitConverter.GetBytes(option1byte).ToArray());
                BitArray option2bits = new BitArray(BitConverter.GetBytes(option2byte).ToArray());

                checkBox1.Checked = option1bits[7];
                checkBox2.Checked = option1bits[6];
                checkBox3.Checked = option1bits[5];
                checkBox4.Checked = option1bits[4];
                checkBox5.Checked = option1bits[3];
                checkBox6.Checked = option1bits[2];
                checkBox7.Checked = option1bits[1];
                checkBox8.Checked = option1bits[0];

                checkBox9.Checked = option2bits[7];
                checkBox10.Checked = option2bits[6];
                checkBox11.Checked = option2bits[5];
                checkBox12.Checked = option2bits[4];
                checkBox13.Checked = option2bits[3];
                checkBox14.Checked = option2bits[2];
                checkBox15.Checked = option2bits[1];
                checkBox16.Checked = option2bits[0];
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = string.Concat(this.textBox2.Text, this.textBox3.Text);
        }
    }
}
