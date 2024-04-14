namespace PcmHacking.DialogBoxes
{
    partial class StepperForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.speedometerlowerBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tachometerlowerBox = new System.Windows.Forms.TextBox();
            this.oillowerBox = new System.Windows.Forms.TextBox();
            this.voltlowerBox = new System.Windows.Forms.TextBox();
            this.fuellowerBox = new System.Windows.Forms.TextBox();
            this.coolantlowerBox = new System.Windows.Forms.TextBox();
            this.transmissionlowerBox = new System.Windows.Forms.TextBox();
            this.updatecal = new System.Windows.Forms.Button();
            this.TestSweep = new System.Windows.Forms.Button();
            this.speedometerupperBox = new System.Windows.Forms.TextBox();
            this.tachometerupperBox = new System.Windows.Forms.TextBox();
            this.oilupperBox = new System.Windows.Forms.TextBox();
            this.voltupperBox = new System.Windows.Forms.TextBox();
            this.fuelupperBox = new System.Windows.Forms.TextBox();
            this.coolantupperBox = new System.Windows.Forms.TextBox();
            this.transmissionupperBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Speedometer:";
            // 
            // speedometerlowerBox
            // 
            this.speedometerlowerBox.Location = new System.Drawing.Point(86, 31);
            this.speedometerlowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.speedometerlowerBox.MaxLength = 3;
            this.speedometerlowerBox.Name = "speedometerlowerBox";
            this.speedometerlowerBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.speedometerlowerBox.Size = new System.Drawing.Size(81, 20);
            this.speedometerlowerBox.TabIndex = 1;
            this.speedometerlowerBox.TextChanged += new System.EventHandler(this.speedometerlowerBox_TextChanged);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(444, 368);
            this.okButton.Margin = new System.Windows.Forms.Padding(2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 24);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "&OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tachometer:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 97);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Oil Pressure:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 129);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Volt:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 161);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Fuel Level:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 193);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Coolant Temp:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 225);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Trans Temp:";
            // 
            // tachometerlowerBox
            // 
            this.tachometerlowerBox.Location = new System.Drawing.Point(86, 63);
            this.tachometerlowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.tachometerlowerBox.MaxLength = 3;
            this.tachometerlowerBox.Name = "tachometerlowerBox";
            this.tachometerlowerBox.Size = new System.Drawing.Size(81, 20);
            this.tachometerlowerBox.TabIndex = 11;
            this.tachometerlowerBox.TextChanged += new System.EventHandler(this.tachometerlowerBox_TextChanged);
            // 
            // oillowerBox
            // 
            this.oillowerBox.Location = new System.Drawing.Point(86, 95);
            this.oillowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.oillowerBox.MaxLength = 2;
            this.oillowerBox.Name = "oillowerBox";
            this.oillowerBox.Size = new System.Drawing.Size(81, 20);
            this.oillowerBox.TabIndex = 12;
            this.oillowerBox.TextChanged += new System.EventHandler(this.oillowerBox_TextChanged);
            // 
            // voltlowerBox
            // 
            this.voltlowerBox.Location = new System.Drawing.Point(86, 127);
            this.voltlowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.voltlowerBox.MaxLength = 2;
            this.voltlowerBox.Name = "voltlowerBox";
            this.voltlowerBox.Size = new System.Drawing.Size(81, 20);
            this.voltlowerBox.TabIndex = 13;
            this.voltlowerBox.TextChanged += new System.EventHandler(this.voltlowerBox_TextChanged);
            // 
            // fuellowerBox
            // 
            this.fuellowerBox.Location = new System.Drawing.Point(86, 159);
            this.fuellowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.fuellowerBox.MaxLength = 2;
            this.fuellowerBox.Name = "fuellowerBox";
            this.fuellowerBox.Size = new System.Drawing.Size(81, 20);
            this.fuellowerBox.TabIndex = 14;
            this.fuellowerBox.TextChanged += new System.EventHandler(this.fuellowerBox_TextChanged);
            // 
            // coolantlowerBox
            // 
            this.coolantlowerBox.Location = new System.Drawing.Point(86, 191);
            this.coolantlowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.coolantlowerBox.MaxLength = 2;
            this.coolantlowerBox.Name = "coolantlowerBox";
            this.coolantlowerBox.Size = new System.Drawing.Size(81, 20);
            this.coolantlowerBox.TabIndex = 15;
            this.coolantlowerBox.TextChanged += new System.EventHandler(this.coolantlowerBox_TextChanged);
            // 
            // transmissionlowerBox
            // 
            this.transmissionlowerBox.Location = new System.Drawing.Point(86, 223);
            this.transmissionlowerBox.Margin = new System.Windows.Forms.Padding(2);
            this.transmissionlowerBox.MaxLength = 2;
            this.transmissionlowerBox.Name = "transmissionlowerBox";
            this.transmissionlowerBox.Size = new System.Drawing.Size(81, 20);
            this.transmissionlowerBox.TabIndex = 16;
            this.transmissionlowerBox.TextChanged += new System.EventHandler(this.transmissionlowerBox_TextChanged);
            // 
            // updatecal
            // 
            this.updatecal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.updatecal.Location = new System.Drawing.Point(14, 368);
            this.updatecal.Margin = new System.Windows.Forms.Padding(2);
            this.updatecal.Name = "updatecal";
            this.updatecal.Size = new System.Drawing.Size(116, 24);
            this.updatecal.TabIndex = 24;
            this.updatecal.Text = "Update Stepper Cal.";
            this.updatecal.UseVisualStyleBackColor = true;
            this.updatecal.Click += new System.EventHandler(this.updatecal_Click);
            // 
            // TestSweep
            // 
            this.TestSweep.Location = new System.Drawing.Point(152, 368);
            this.TestSweep.Name = "TestSweep";
            this.TestSweep.Size = new System.Drawing.Size(116, 24);
            this.TestSweep.TabIndex = 25;
            this.TestSweep.Text = "Test Sweep Gauges";
            this.TestSweep.UseVisualStyleBackColor = true;
            this.TestSweep.Click += new System.EventHandler(this.TestSweep_Click);
            // 
            // speedometerupperBox
            // 
            this.speedometerupperBox.Location = new System.Drawing.Point(187, 31);
            this.speedometerupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.speedometerupperBox.MaxLength = 3;
            this.speedometerupperBox.Name = "speedometerupperBox";
            this.speedometerupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.speedometerupperBox.Size = new System.Drawing.Size(81, 20);
            this.speedometerupperBox.TabIndex = 26;
            this.speedometerupperBox.TextChanged += new System.EventHandler(this.speedometerupperBox_TextChanged);
            // 
            // tachometerupperBox
            // 
            this.tachometerupperBox.Location = new System.Drawing.Point(187, 63);
            this.tachometerupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.tachometerupperBox.MaxLength = 3;
            this.tachometerupperBox.Name = "tachometerupperBox";
            this.tachometerupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tachometerupperBox.Size = new System.Drawing.Size(81, 20);
            this.tachometerupperBox.TabIndex = 27;
            this.tachometerupperBox.TextChanged += new System.EventHandler(this.tachometerupperBox_TextChanged);
            // 
            // oilupperBox
            // 
            this.oilupperBox.Location = new System.Drawing.Point(187, 95);
            this.oilupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.oilupperBox.MaxLength = 2;
            this.oilupperBox.Name = "oilupperBox";
            this.oilupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.oilupperBox.Size = new System.Drawing.Size(81, 20);
            this.oilupperBox.TabIndex = 28;
            this.oilupperBox.TextChanged += new System.EventHandler(this.oilupperBox_TextChanged);
            // 
            // voltupperBox
            // 
            this.voltupperBox.Location = new System.Drawing.Point(187, 127);
            this.voltupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.voltupperBox.MaxLength = 2;
            this.voltupperBox.Name = "voltupperBox";
            this.voltupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.voltupperBox.Size = new System.Drawing.Size(81, 20);
            this.voltupperBox.TabIndex = 29;
            this.voltupperBox.TextChanged += new System.EventHandler(this.voltupperBox_TextChanged);
            // 
            // fuelupperBox
            // 
            this.fuelupperBox.Location = new System.Drawing.Point(187, 159);
            this.fuelupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.fuelupperBox.MaxLength = 2;
            this.fuelupperBox.Name = "fuelupperBox";
            this.fuelupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fuelupperBox.Size = new System.Drawing.Size(81, 20);
            this.fuelupperBox.TabIndex = 30;
            this.fuelupperBox.TextChanged += new System.EventHandler(this.fuelupperBox_TextChanged);
            // 
            // coolantupperBox
            // 
            this.coolantupperBox.Location = new System.Drawing.Point(187, 191);
            this.coolantupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.coolantupperBox.MaxLength = 2;
            this.coolantupperBox.Name = "coolantupperBox";
            this.coolantupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.coolantupperBox.Size = new System.Drawing.Size(81, 20);
            this.coolantupperBox.TabIndex = 31;
            this.coolantupperBox.TextChanged += new System.EventHandler(this.coolantupperBox_TextChanged);
            // 
            // transmissionupperBox
            // 
            this.transmissionupperBox.Location = new System.Drawing.Point(187, 223);
            this.transmissionupperBox.Margin = new System.Windows.Forms.Padding(2);
            this.transmissionupperBox.MaxLength = 2;
            this.transmissionupperBox.Name = "transmissionupperBox";
            this.transmissionupperBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.transmissionupperBox.Size = new System.Drawing.Size(81, 20);
            this.transmissionupperBox.TabIndex = 32;
            this.transmissionupperBox.TextChanged += new System.EventHandler(this.transmissionupperBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(86, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 33;
            this.label8.Text = "Lower limit";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(187, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Upper limit";
            // 
            // StepperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 402);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.transmissionupperBox);
            this.Controls.Add(this.coolantupperBox);
            this.Controls.Add(this.fuelupperBox);
            this.Controls.Add(this.voltupperBox);
            this.Controls.Add(this.oilupperBox);
            this.Controls.Add(this.tachometerupperBox);
            this.Controls.Add(this.speedometerupperBox);
            this.Controls.Add(this.TestSweep);
            this.Controls.Add(this.updatecal);
            this.Controls.Add(this.transmissionlowerBox);
            this.Controls.Add(this.coolantlowerBox);
            this.Controls.Add(this.fuellowerBox);
            this.Controls.Add(this.voltlowerBox);
            this.Controls.Add(this.oillowerBox);
            this.Controls.Add(this.tachometerlowerBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.speedometerlowerBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StepperForm";
            this.Text = "Adjust Stepper Calibration";
            this.Load += new System.EventHandler(this.StepperForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox speedometerlowerBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tachometerlowerBox;
        private System.Windows.Forms.TextBox oillowerBox;
        private System.Windows.Forms.TextBox voltlowerBox;
        private System.Windows.Forms.TextBox fuellowerBox;
        private System.Windows.Forms.TextBox coolantlowerBox;
        private System.Windows.Forms.TextBox transmissionlowerBox;
        private System.Windows.Forms.Button updatecal;
        private System.Windows.Forms.Button TestSweep;
        private System.Windows.Forms.TextBox speedometerupperBox;
        private System.Windows.Forms.TextBox tachometerupperBox;
        private System.Windows.Forms.TextBox oilupperBox;
        private System.Windows.Forms.TextBox voltupperBox;
        private System.Windows.Forms.TextBox fuelupperBox;
        private System.Windows.Forms.TextBox coolantupperBox;
        private System.Windows.Forms.TextBox transmissionupperBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}