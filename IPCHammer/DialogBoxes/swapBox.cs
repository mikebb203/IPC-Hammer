using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PcmHacking.DialogBoxes
{
    public partial class swapBox : Form
    {
        private Vehicle vehicle;
        protected Vehicle Vehicle { get { return this.vehicle; } }
        private readonly ILogger logger;
        public swapBox(Vehicle vehicle, ILogger logger)
        {
            InitializeComponent();
            this.vehicle = vehicle;
            this.logger = logger;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string input = battCal.Text;
            if (battCal == null)
            {
                MessageBox.Show("battCal TextBox is null");
                return;
            }
            string text = battCal.Text.Trim();
            if (!float.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out float value))
            {
                MessageBox.Show($"Invalid float value: '{text}'");
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);

            await this.Vehicle.Swapmute();
            await Task.Delay(1000);
            await this.Vehicle.Swapmagic();
            await Task.Delay(200);
            await this.Vehicle.Swapopt(0x02, bytes[0], bytes[1], bytes[2], bytes[3]);
        
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string input = oilCal.Text;
            if (oilCal == null)
            {
                MessageBox.Show("battCal TextBox is null");
                return;
            }
            string text = oilCal.Text.Trim();
            if (!float.TryParse(
                text,
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out float value))
            {
                MessageBox.Show($"Invalid float value: '{text}'");
                return;
            }
            byte[] bytes = BitConverter.GetBytes(value);

            await this.Vehicle.Swapmute();
            await Task.Delay(1000);
            await this.Vehicle.Swapmagic();
            await Task.Delay(200);
            await this.Vehicle.Swapopt(0x03, bytes[0], bytes[1], bytes[2], bytes[3]);

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await this.Vehicle.Swapmute();
            await Task.Delay(1000);
            await this.Vehicle.Swapmagic();
            await Task.Delay(200);
            await this.Vehicle.Swapopt(0x99, 0x43, 0x41, 0x4C, 0x31); // save to flash
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            CheckBox[] boxes =
{
    checkBox1,
    checkBox2,
    checkBox3,
    checkBox4,
    checkBox5,
    checkBox6,
    checkBox7,
    checkBox8
};

            byte flags = 0;

            for (int i = 0; i < boxes.Length; i++)
            {
                if (boxes[i].Checked)
                    flags |= (byte)(1 << i);
            }
            await this.Vehicle.Swapmute();
            await Task.Delay(1000);
            await this.Vehicle.Swapmagic();
            await Task.Delay(200);
            await this.Vehicle.Swapopt(0x04, flags, 0x00, 0x00, 0x00); // save to flash
        }
    }
}
