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
            this.speedometerBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tachometerBox = new System.Windows.Forms.TextBox();
            this.oilBox = new System.Windows.Forms.TextBox();
            this.voltBox = new System.Windows.Forms.TextBox();
            this.fuelBox = new System.Windows.Forms.TextBox();
            this.coolantBox = new System.Windows.Forms.TextBox();
            this.transmissionBox = new System.Windows.Forms.TextBox();
            this.speedometerBar = new System.Windows.Forms.TrackBar();
            this.tachBar = new System.Windows.Forms.TrackBar();
            this.oilBar = new System.Windows.Forms.TrackBar();
            this.voltBar = new System.Windows.Forms.TrackBar();
            this.fuelBar = new System.Windows.Forms.TrackBar();
            this.coolantBar = new System.Windows.Forms.TrackBar();
            this.transBar = new System.Windows.Forms.TrackBar();
            this.updatecal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.speedometerBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tachBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.coolantBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Speedometer:";
            // 
            // speedometerBox
            // 
            this.speedometerBox.Location = new System.Drawing.Point(86, 11);
            this.speedometerBox.Margin = new System.Windows.Forms.Padding(2);
            this.speedometerBox.MaxLength = 6;
            this.speedometerBox.Name = "speedometerBox";
            this.speedometerBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.speedometerBox.Size = new System.Drawing.Size(81, 20);
            this.speedometerBox.TabIndex = 1;
            this.speedometerBox.TextChanged += new System.EventHandler(this.speedometerBox_TextChanged);
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
            this.label2.Location = new System.Drawing.Point(9, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tachometer:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 113);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Oil Pressure:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 163);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Volt:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 213);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Fuel Level:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 263);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Coolant Temp:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 313);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Trans Temp:";
            // 
            // tachometerBox
            // 
            this.tachometerBox.Location = new System.Drawing.Point(86, 63);
            this.tachometerBox.Margin = new System.Windows.Forms.Padding(2);
            this.tachometerBox.MaxLength = 6;
            this.tachometerBox.Name = "tachometerBox";
            this.tachometerBox.Size = new System.Drawing.Size(81, 20);
            this.tachometerBox.TabIndex = 11;
            this.tachometerBox.TextChanged += new System.EventHandler(this.tachometerBox_TextChanged);
            // 
            // oilBox
            // 
            this.oilBox.Location = new System.Drawing.Point(86, 113);
            this.oilBox.Margin = new System.Windows.Forms.Padding(2);
            this.oilBox.MaxLength = 6;
            this.oilBox.Name = "oilBox";
            this.oilBox.Size = new System.Drawing.Size(81, 20);
            this.oilBox.TabIndex = 12;
            this.oilBox.TextChanged += new System.EventHandler(this.oilBox_TextChanged);
            // 
            // voltBox
            // 
            this.voltBox.Location = new System.Drawing.Point(86, 163);
            this.voltBox.Margin = new System.Windows.Forms.Padding(2);
            this.voltBox.MaxLength = 6;
            this.voltBox.Name = "voltBox";
            this.voltBox.Size = new System.Drawing.Size(81, 20);
            this.voltBox.TabIndex = 13;
            this.voltBox.TextChanged += new System.EventHandler(this.voltBox_TextChanged);
            // 
            // fuelBox
            // 
            this.fuelBox.Location = new System.Drawing.Point(86, 213);
            this.fuelBox.Margin = new System.Windows.Forms.Padding(2);
            this.fuelBox.MaxLength = 6;
            this.fuelBox.Name = "fuelBox";
            this.fuelBox.Size = new System.Drawing.Size(81, 20);
            this.fuelBox.TabIndex = 14;
            this.fuelBox.TextChanged += new System.EventHandler(this.fuelBox_TextChanged);
            // 
            // coolantBox
            // 
            this.coolantBox.Location = new System.Drawing.Point(86, 263);
            this.coolantBox.Margin = new System.Windows.Forms.Padding(2);
            this.coolantBox.MaxLength = 6;
            this.coolantBox.Name = "coolantBox";
            this.coolantBox.Size = new System.Drawing.Size(81, 20);
            this.coolantBox.TabIndex = 15;
            this.coolantBox.TextChanged += new System.EventHandler(this.coolantBox_TextChanged);
            // 
            // transmissionBox
            // 
            this.transmissionBox.Location = new System.Drawing.Point(86, 313);
            this.transmissionBox.Margin = new System.Windows.Forms.Padding(2);
            this.transmissionBox.MaxLength = 6;
            this.transmissionBox.Name = "transmissionBox";
            this.transmissionBox.Size = new System.Drawing.Size(81, 20);
            this.transmissionBox.TabIndex = 16;
            this.transmissionBox.TextChanged += new System.EventHandler(this.transmissionBox_TextChanged);
            // 
            // speedometerBar
            // 
            this.speedometerBar.Location = new System.Drawing.Point(186, 11);
            this.speedometerBar.Maximum = 16777215;
            this.speedometerBar.Name = "speedometerBar";
            this.speedometerBar.Size = new System.Drawing.Size(333, 45);
            this.speedometerBar.TabIndex = 17;
            this.speedometerBar.Scroll += new System.EventHandler(this.speedometer_Scroll);
            // 
            // tachBar
            // 
            this.tachBar.Location = new System.Drawing.Point(186, 63);
            this.tachBar.Maximum = 16777215;
            this.tachBar.Name = "tachBar";
            this.tachBar.Size = new System.Drawing.Size(333, 45);
            this.tachBar.TabIndex = 18;
            this.tachBar.Scroll += new System.EventHandler(this.tachBar_Scroll);
            // 
            // oilBar
            // 
            this.oilBar.Location = new System.Drawing.Point(186, 113);
            this.oilBar.Maximum = 65535;
            this.oilBar.Name = "oilBar";
            this.oilBar.Size = new System.Drawing.Size(333, 45);
            this.oilBar.TabIndex = 19;
            this.oilBar.Scroll += new System.EventHandler(this.oilBar_Scroll);
            // 
            // voltBar
            // 
            this.voltBar.Location = new System.Drawing.Point(186, 163);
            this.voltBar.Maximum = 65535;
            this.voltBar.Name = "voltBar";
            this.voltBar.Size = new System.Drawing.Size(333, 45);
            this.voltBar.TabIndex = 20;
            this.voltBar.Scroll += new System.EventHandler(this.voltBar_Scroll);
            // 
            // fuelBar
            // 
            this.fuelBar.Location = new System.Drawing.Point(186, 213);
            this.fuelBar.Maximum = 65535;
            this.fuelBar.Name = "fuelBar";
            this.fuelBar.Size = new System.Drawing.Size(333, 45);
            this.fuelBar.TabIndex = 21;
            this.fuelBar.Scroll += new System.EventHandler(this.fuelBar_Scroll);
            // 
            // coolantBar
            // 
            this.coolantBar.Location = new System.Drawing.Point(186, 263);
            this.coolantBar.Maximum = 65535;
            this.coolantBar.Name = "coolantBar";
            this.coolantBar.Size = new System.Drawing.Size(333, 45);
            this.coolantBar.TabIndex = 22;
            this.coolantBar.Scroll += new System.EventHandler(this.coolantBar_Scroll);
            // 
            // transBar
            // 
            this.transBar.Location = new System.Drawing.Point(186, 313);
            this.transBar.Maximum = 65535;
            this.transBar.Name = "transBar";
            this.transBar.Size = new System.Drawing.Size(333, 45);
            this.transBar.TabIndex = 23;
            this.transBar.Scroll += new System.EventHandler(this.transBar_Scroll);
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
            // StepperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 402);
            this.Controls.Add(this.updatecal);
            this.Controls.Add(this.transBar);
            this.Controls.Add(this.coolantBar);
            this.Controls.Add(this.fuelBar);
            this.Controls.Add(this.voltBar);
            this.Controls.Add(this.oilBar);
            this.Controls.Add(this.tachBar);
            this.Controls.Add(this.speedometerBar);
            this.Controls.Add(this.transmissionBox);
            this.Controls.Add(this.coolantBox);
            this.Controls.Add(this.fuelBox);
            this.Controls.Add(this.voltBox);
            this.Controls.Add(this.oilBox);
            this.Controls.Add(this.tachometerBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.speedometerBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StepperForm";
            this.Text = "Adjust Stepper Calibration";
            this.Load += new System.EventHandler(this.StepperForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.speedometerBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tachBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oilBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.voltBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fuelBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.coolantBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox speedometerBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tachometerBox;
        private System.Windows.Forms.TextBox oilBox;
        private System.Windows.Forms.TextBox voltBox;
        private System.Windows.Forms.TextBox fuelBox;
        private System.Windows.Forms.TextBox coolantBox;
        private System.Windows.Forms.TextBox transmissionBox;
        private System.Windows.Forms.TrackBar speedometerBar;
        private System.Windows.Forms.TrackBar tachBar;
        private System.Windows.Forms.TrackBar oilBar;
        private System.Windows.Forms.TrackBar voltBar;
        private System.Windows.Forms.TrackBar fuelBar;
        private System.Windows.Forms.TrackBar coolantBar;
        private System.Windows.Forms.TrackBar transBar;
        private System.Windows.Forms.Button updatecal;
    }
}