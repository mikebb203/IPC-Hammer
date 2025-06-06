﻿namespace PcmHacking

{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.interfaceBox = new System.Windows.Forms.GroupBox();
            this.reinitializeButton = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            this.deviceDescription = new System.Windows.Forms.Label();
            this.operationsBox = new System.Windows.Forms.GroupBox();
            this.TestIPCTB = new System.Windows.Forms.Button();
            this.ModifyOptionsTB = new System.Windows.Forms.Button();
            this.testipc99 = new System.Windows.Forms.Button();
            this.Modify_options99 = new System.Windows.Forms.Button();
            this.Checksum_test = new System.Windows.Forms.Button();
            this.Modify_options = new System.Windows.Forms.Button();
            this.adjustStepperCalibration = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.writeCalibrationButton = new System.Windows.Forms.Button();
            this.write1CalibrationButton = new System.Windows.Forms.Button();
            this.IpctestButton = new System.Windows.Forms.Button();
            this.readPropertiesButton = new System.Windows.Forms.Button();
            this.tabs = new System.Windows.Forms.TabControl();
            this.resultsTab = new System.Windows.Forms.TabPage();
            this.userLog = new System.Windows.Forms.TextBox();
            this.helpTab = new System.Windows.Forms.TabPage();
            this.helpWebBrowser = new System.Windows.Forms.WebBrowser();
            this.creditsTab = new System.Windows.Forms.TabPage();
            this.creditsWebBrowser = new System.Windows.Forms.WebBrowser();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.debugLog = new System.Windows.Forms.TextBox();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyVINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mileageCorrectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemEnable4xReadWrite = new System.Windows.Forms.ToolStripMenuItem();
            this.interfaceBox.SuspendLayout();
            this.operationsBox.SuspendLayout();
            this.tabs.SuspendLayout();
            this.resultsTab.SuspendLayout();
            this.helpTab.SuspendLayout();
            this.creditsTab.SuspendLayout();
            this.debugTab.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // interfaceBox
            // 
            this.interfaceBox.Controls.Add(this.reinitializeButton);
            this.interfaceBox.Controls.Add(this.selectButton);
            this.interfaceBox.Controls.Add(this.deviceDescription);
            this.interfaceBox.Location = new System.Drawing.Point(12, 32);
            this.interfaceBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.interfaceBox.Name = "interfaceBox";
            this.interfaceBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.interfaceBox.Size = new System.Drawing.Size(299, 114);
            this.interfaceBox.TabIndex = 0;
            this.interfaceBox.TabStop = false;
            this.interfaceBox.Text = "Device";
            // 
            // reinitializeButton
            // 
            this.reinitializeButton.Location = new System.Drawing.Point(5, 76);
            this.reinitializeButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reinitializeButton.Name = "reinitializeButton";
            this.reinitializeButton.Size = new System.Drawing.Size(288, 31);
            this.reinitializeButton.TabIndex = 2;
            this.reinitializeButton.Text = "Re-&Initialize Device";
            this.reinitializeButton.UseVisualStyleBackColor = true;
            this.reinitializeButton.Click += new System.EventHandler(this.reinitializeButton_Click);
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(5, 39);
            this.selectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(288, 31);
            this.selectButton.TabIndex = 1;
            this.selectButton.Text = "&Select Device";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // deviceDescription
            // 
            this.deviceDescription.Location = new System.Drawing.Point(5, 20);
            this.deviceDescription.Name = "deviceDescription";
            this.deviceDescription.Size = new System.Drawing.Size(285, 16);
            this.deviceDescription.TabIndex = 0;
            this.deviceDescription.Text = "Device name will be displayed here";
            // 
            // operationsBox
            // 
            this.operationsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.operationsBox.Controls.Add(this.TestIPCTB);
            this.operationsBox.Controls.Add(this.ModifyOptionsTB);
            this.operationsBox.Controls.Add(this.testipc99);
            this.operationsBox.Controls.Add(this.Modify_options99);
            this.operationsBox.Controls.Add(this.Checksum_test);
            this.operationsBox.Controls.Add(this.Modify_options);
            this.operationsBox.Controls.Add(this.adjustStepperCalibration);
            this.operationsBox.Controls.Add(this.cancelButton);
            this.operationsBox.Controls.Add(this.writeCalibrationButton);
            this.operationsBox.Controls.Add(this.write1CalibrationButton);
            this.operationsBox.Controls.Add(this.IpctestButton);
            this.operationsBox.Controls.Add(this.readPropertiesButton);
            this.operationsBox.Location = new System.Drawing.Point(12, 151);
            this.operationsBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationsBox.Name = "operationsBox";
            this.operationsBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.operationsBox.Size = new System.Drawing.Size(299, 466);
            this.operationsBox.TabIndex = 1;
            this.operationsBox.TabStop = false;
            this.operationsBox.Text = "Operations";
            // 
            // TestIPCTB
            // 
            this.TestIPCTB.Location = new System.Drawing.Point(8, 310);
            this.TestIPCTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TestIPCTB.Name = "TestIPCTB";
            this.TestIPCTB.Size = new System.Drawing.Size(284, 28);
            this.TestIPCTB.TabIndex = 15;
            this.TestIPCTB.Text = "Test IPC TB/Envoy ";
            this.TestIPCTB.UseVisualStyleBackColor = true;
            this.TestIPCTB.Click += new System.EventHandler(this.TestIPCTB_Click);
            // 
            // ModifyOptionsTB
            // 
            this.ModifyOptionsTB.Location = new System.Drawing.Point(8, 346);
            this.ModifyOptionsTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ModifyOptionsTB.Name = "ModifyOptionsTB";
            this.ModifyOptionsTB.Size = new System.Drawing.Size(285, 28);
            this.ModifyOptionsTB.TabIndex = 14;
            this.ModifyOptionsTB.Text = "Modify Options TB/Envoy";
            this.ModifyOptionsTB.UseVisualStyleBackColor = true;
            this.ModifyOptionsTB.Click += new System.EventHandler(this.ModifyOptionsTB_Click);
            // 
            // testipc99
            // 
            this.testipc99.Location = new System.Drawing.Point(7, 239);
            this.testipc99.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.testipc99.Name = "testipc99";
            this.testipc99.Size = new System.Drawing.Size(284, 28);
            this.testipc99.TabIndex = 13;
            this.testipc99.Text = "Test IPC 99-02";
            this.testipc99.UseVisualStyleBackColor = true;
            this.testipc99.Click += new System.EventHandler(this.testipc99_Click);
            // 
            // Modify_options99
            // 
            this.Modify_options99.Location = new System.Drawing.Point(7, 274);
            this.Modify_options99.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Modify_options99.Name = "Modify_options99";
            this.Modify_options99.Size = new System.Drawing.Size(285, 28);
            this.Modify_options99.TabIndex = 12;
            this.Modify_options99.Text = "Modify Options 99-02";
            this.Modify_options99.UseVisualStyleBackColor = true;
            this.Modify_options99.Click += new System.EventHandler(this.Modify_options99_Click);
            // 
            // Checksum_test
            // 
            this.Checksum_test.Location = new System.Drawing.Point(5, 390);
            this.Checksum_test.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Checksum_test.Name = "Checksum_test";
            this.Checksum_test.Size = new System.Drawing.Size(288, 31);
            this.Checksum_test.TabIndex = 11;
            this.Checksum_test.Text = "Checksum Test";
            this.Checksum_test.UseVisualStyleBackColor = true;
            this.Checksum_test.Click += new System.EventHandler(this.Checksum_test_Click);
            // 
            // Modify_options
            // 
            this.Modify_options.Location = new System.Drawing.Point(5, 201);
            this.Modify_options.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Modify_options.Name = "Modify_options";
            this.Modify_options.Size = new System.Drawing.Size(288, 31);
            this.Modify_options.TabIndex = 8;
            this.Modify_options.Text = "Modify Options 03-07";
            this.Modify_options.UseVisualStyleBackColor = true;
            this.Modify_options.Click += new System.EventHandler(this.Modify_options_Click);
            // 
            // adjustStepperCalibration
            // 
            this.adjustStepperCalibration.Location = new System.Drawing.Point(5, 165);
            this.adjustStepperCalibration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.adjustStepperCalibration.Name = "adjustStepperCalibration";
            this.adjustStepperCalibration.Size = new System.Drawing.Size(288, 31);
            this.adjustStepperCalibration.TabIndex = 7;
            this.adjustStepperCalibration.Text = "Adjust Stepper Calibration 03-07";
            this.adjustStepperCalibration.UseVisualStyleBackColor = true;
            this.adjustStepperCalibration.Click += new System.EventHandler(this.adjustStepperCalibration_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(5, 426);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(288, 31);
            this.cancelButton.TabIndex = 10;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // writeCalibrationButton
            // 
            this.writeCalibrationButton.Location = new System.Drawing.Point(5, 58);
            this.writeCalibrationButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.writeCalibrationButton.Name = "writeCalibrationButton";
            this.writeCalibrationButton.Size = new System.Drawing.Size(288, 31);
            this.writeCalibrationButton.TabIndex = 4;
            this.writeCalibrationButton.Text = "&Write OS + Calibration";
            this.writeCalibrationButton.UseVisualStyleBackColor = true;
            this.writeCalibrationButton.Click += new System.EventHandler(this.writeCalibrationButton_Click);
            // 
            // write1CalibrationButton
            // 
            this.write1CalibrationButton.Location = new System.Drawing.Point(5, 94);
            this.write1CalibrationButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.write1CalibrationButton.Name = "write1CalibrationButton";
            this.write1CalibrationButton.Size = new System.Drawing.Size(288, 31);
            this.write1CalibrationButton.TabIndex = 5;
            this.write1CalibrationButton.Text = "Write IPC Calibration";
            this.write1CalibrationButton.UseVisualStyleBackColor = true;
            this.write1CalibrationButton.Click += new System.EventHandler(this.write1CalibrationButton_Click);
            // 
            // IpctestButton
            // 
            this.IpctestButton.Location = new System.Drawing.Point(5, 129);
            this.IpctestButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IpctestButton.Name = "IpctestButton";
            this.IpctestButton.Size = new System.Drawing.Size(288, 31);
            this.IpctestButton.TabIndex = 6;
            this.IpctestButton.Text = "Test IPC 03-07";
            this.IpctestButton.UseVisualStyleBackColor = true;
            this.IpctestButton.Click += new System.EventHandler(this.IpctestButton_Click);
            // 
            // readPropertiesButton
            // 
            this.readPropertiesButton.Location = new System.Drawing.Point(5, 22);
            this.readPropertiesButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.readPropertiesButton.Name = "readPropertiesButton";
            this.readPropertiesButton.Size = new System.Drawing.Size(288, 31);
            this.readPropertiesButton.TabIndex = 0;
            this.readPropertiesButton.Text = "Read &Properties";
            this.readPropertiesButton.UseVisualStyleBackColor = true;
            this.readPropertiesButton.Click += new System.EventHandler(this.readPropertiesButton_Click);
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.resultsTab);
            this.tabs.Controls.Add(this.helpTab);
            this.tabs.Controls.Add(this.creditsTab);
            this.tabs.Controls.Add(this.debugTab);
            this.tabs.Location = new System.Drawing.Point(317, 32);
            this.tabs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(811, 586);
            this.tabs.TabIndex = 2;
            // 
            // resultsTab
            // 
            this.resultsTab.Controls.Add(this.userLog);
            this.resultsTab.Location = new System.Drawing.Point(4, 25);
            this.resultsTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resultsTab.Name = "resultsTab";
            this.resultsTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.resultsTab.Size = new System.Drawing.Size(803, 557);
            this.resultsTab.TabIndex = 0;
            this.resultsTab.Text = "Results";
            this.resultsTab.UseVisualStyleBackColor = true;
            // 
            // userLog
            // 
            this.userLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.userLog.Location = new System.Drawing.Point(5, 6);
            this.userLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userLog.Multiline = true;
            this.userLog.Name = "userLog";
            this.userLog.ReadOnly = true;
            this.userLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.userLog.Size = new System.Drawing.Size(791, 542);
            this.userLog.TabIndex = 0;
            // 
            // helpTab
            // 
            this.helpTab.Controls.Add(this.helpWebBrowser);
            this.helpTab.Location = new System.Drawing.Point(4, 25);
            this.helpTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.helpTab.Name = "helpTab";
            this.helpTab.Size = new System.Drawing.Size(803, 557);
            this.helpTab.TabIndex = 2;
            this.helpTab.Text = "Help";
            this.helpTab.UseVisualStyleBackColor = true;
            // 
            // helpWebBrowser
            // 
            this.helpWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.helpWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.helpWebBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.helpWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.helpWebBrowser.Name = "helpWebBrowser";
            this.helpWebBrowser.Size = new System.Drawing.Size(803, 557);
            this.helpWebBrowser.TabIndex = 0;
            // 
            // creditsTab
            // 
            this.creditsTab.Controls.Add(this.creditsWebBrowser);
            this.creditsTab.Location = new System.Drawing.Point(4, 25);
            this.creditsTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.creditsTab.Name = "creditsTab";
            this.creditsTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.creditsTab.Size = new System.Drawing.Size(803, 557);
            this.creditsTab.TabIndex = 3;
            this.creditsTab.Text = "Credits";
            this.creditsTab.UseVisualStyleBackColor = true;
            // 
            // creditsWebBrowser
            // 
            this.creditsWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.creditsWebBrowser.Location = new System.Drawing.Point(4, 4);
            this.creditsWebBrowser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.creditsWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.creditsWebBrowser.Name = "creditsWebBrowser";
            this.creditsWebBrowser.Size = new System.Drawing.Size(795, 549);
            this.creditsWebBrowser.TabIndex = 1;
            // 
            // debugTab
            // 
            this.debugTab.Controls.Add(this.debugLog);
            this.debugTab.Location = new System.Drawing.Point(4, 25);
            this.debugTab.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.debugTab.Name = "debugTab";
            this.debugTab.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.debugTab.Size = new System.Drawing.Size(803, 557);
            this.debugTab.TabIndex = 1;
            this.debugTab.Text = "Debug Log";
            this.debugTab.UseVisualStyleBackColor = true;
            // 
            // debugLog
            // 
            this.debugLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugLog.Location = new System.Drawing.Point(3, 2);
            this.debugLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.debugLog.Multiline = true;
            this.debugLog.Name = "debugLog";
            this.debugLog.ReadOnly = true;
            this.debugLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.debugLog.Size = new System.Drawing.Size(797, 553);
            this.debugLog.TabIndex = 0;
            // 
            // menuStripMain
            // 
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemTools,
            this.menuItemOptions});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1139, 28);
            this.menuStripMain.TabIndex = 3;
            this.menuStripMain.Text = "Main Menu";
            // 
            // menuItemTools
            // 
            this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyVINToolStripMenuItem,
            this.mileageCorrectionToolStripMenuItem});
            this.menuItemTools.Name = "menuItemTools";
            this.menuItemTools.Size = new System.Drawing.Size(58, 24);
            this.menuItemTools.Text = "&Tools";
            // 
            // modifyVINToolStripMenuItem
            // 
            this.modifyVINToolStripMenuItem.Name = "modifyVINToolStripMenuItem";
            this.modifyVINToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.modifyVINToolStripMenuItem.Text = "&Change VIN";
            this.modifyVINToolStripMenuItem.Click += new System.EventHandler(this.modifyVinButton_Click);
            // 
            // mileageCorrectionToolStripMenuItem
            // 
            this.mileageCorrectionToolStripMenuItem.Name = "mileageCorrectionToolStripMenuItem";
            this.mileageCorrectionToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.mileageCorrectionToolStripMenuItem.Text = "Mileage + Hours Correction";
            this.mileageCorrectionToolStripMenuItem.Click += new System.EventHandler(this.mileageCorrectionToolStripMenuItem_Click);
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemEnable4xReadWrite});
            this.menuItemOptions.Name = "menuItemOptions";
            this.menuItemOptions.Size = new System.Drawing.Size(75, 24);
            this.menuItemOptions.Text = "&Options";
            // 
            // menuItemEnable4xReadWrite
            // 
            this.menuItemEnable4xReadWrite.Name = "menuItemEnable4xReadWrite";
            this.menuItemEnable4xReadWrite.Size = new System.Drawing.Size(265, 26);
            this.menuItemEnable4xReadWrite.Text = "Enable &4x Communication";
            this.menuItemEnable4xReadWrite.Click += new System.EventHandler(this.enable4xReadWrite_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.readPropertiesButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 631);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.operationsBox);
            this.Controls.Add(this.interfaceBox);
            this.Controls.Add(this.menuStripMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "IPC Hammer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.interfaceBox.ResumeLayout(false);
            this.operationsBox.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.resultsTab.ResumeLayout(false);
            this.resultsTab.PerformLayout();
            this.helpTab.ResumeLayout(false);
            this.creditsTab.ResumeLayout(false);
            this.debugTab.ResumeLayout(false);
            this.debugTab.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox interfaceBox;
        private System.Windows.Forms.GroupBox operationsBox;
        private System.Windows.Forms.Button writeCalibrationButton;
        private System.Windows.Forms.Button write1CalibrationButton;
        private System.Windows.Forms.Button IpctestButton;
        private System.Windows.Forms.Button readPropertiesButton;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage resultsTab;
        private System.Windows.Forms.TextBox userLog;
        private System.Windows.Forms.TabPage debugTab;
        private System.Windows.Forms.TextBox debugLog;
        private System.Windows.Forms.Button reinitializeButton;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Label deviceDescription;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TabPage helpTab;
        private System.Windows.Forms.WebBrowser helpWebBrowser;
        private System.Windows.Forms.TabPage creditsTab;
        private System.Windows.Forms.WebBrowser creditsWebBrowser;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem menuItemOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItemEnable4xReadWrite;
        private System.Windows.Forms.ToolStripMenuItem menuItemTools;
        private System.Windows.Forms.ToolStripMenuItem modifyVINToolStripMenuItem;
        private System.Windows.Forms.Button adjustStepperCalibration;
        private System.Windows.Forms.Button Modify_options;
        private System.Windows.Forms.Button Checksum_test;
        private System.Windows.Forms.ToolStripMenuItem mileageCorrectionToolStripMenuItem;
        private System.Windows.Forms.Button Modify_options99;
        private System.Windows.Forms.Button testipc99;
        private System.Windows.Forms.Button TestIPCTB;
        private System.Windows.Forms.Button ModifyOptionsTB;
    }
}

