﻿namespace PcmHacking
{
    partial class PcmExplorerMainForm
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.statusTab = new System.Windows.Forms.TabPage();
            this.userLog = new System.Windows.Forms.TextBox();
            this.debugTab = new System.Windows.Forms.TabPage();
            this.debugLog = new System.Windows.Forms.TextBox();
            this.deviceDescription = new System.Windows.Forms.Label();
            this.selectButton = new System.Windows.Forms.Button();
            this.testPid = new System.Windows.Forms.Button();
            this.pid = new System.Windows.Forms.TextBox();
            this.message = new System.Windows.Forms.TextBox();
            this.sendMessage = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.statusTab.SuspendLayout();
            this.debugTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.statusTab);
            this.tabs.Controls.Add(this.debugTab);
            this.tabs.Location = new System.Drawing.Point(16, 205);
            this.tabs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(973, 334);
            this.tabs.TabIndex = 8;
            // 
            // statusTab
            // 
            this.statusTab.Controls.Add(this.userLog);
            this.statusTab.Location = new System.Drawing.Point(4, 25);
            this.statusTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.statusTab.Name = "statusTab";
            this.statusTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.statusTab.Size = new System.Drawing.Size(965, 305);
            this.statusTab.TabIndex = 0;
            this.statusTab.Text = "Status";
            this.statusTab.UseVisualStyleBackColor = true;
            // 
            // userLog
            // 
            this.userLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userLog.Location = new System.Drawing.Point(4, 4);
            this.userLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.userLog.Multiline = true;
            this.userLog.Name = "userLog";
            this.userLog.ReadOnly = true;
            this.userLog.Size = new System.Drawing.Size(957, 297);
            this.userLog.TabIndex = 0;
            // 
            // debugTab
            // 
            this.debugTab.Controls.Add(this.debugLog);
            this.debugTab.Location = new System.Drawing.Point(4, 25);
            this.debugTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.debugTab.Name = "debugTab";
            this.debugTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.debugTab.Size = new System.Drawing.Size(965, 386);
            this.debugTab.TabIndex = 1;
            this.debugTab.Text = "Debug";
            this.debugTab.UseVisualStyleBackColor = true;
            // 
            // debugLog
            // 
            this.debugLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugLog.Location = new System.Drawing.Point(4, 4);
            this.debugLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.debugLog.Multiline = true;
            this.debugLog.Name = "debugLog";
            this.debugLog.ReadOnly = true;
            this.debugLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugLog.Size = new System.Drawing.Size(957, 378);
            this.debugLog.TabIndex = 0;
            // 
            // deviceDescription
            // 
            this.deviceDescription.AutoSize = true;
            this.deviceDescription.Location = new System.Drawing.Point(312, 22);
            this.deviceDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.deviceDescription.Name = "deviceDescription";
            this.deviceDescription.Size = new System.Drawing.Size(114, 17);
            this.deviceDescription.TabIndex = 7;
            this.deviceDescription.Text = "[selected device]";
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(16, 15);
            this.selectButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(288, 31);
            this.selectButton.TabIndex = 6;
            this.selectButton.Text = "&Select Device";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // testPid
            // 
            this.testPid.Location = new System.Drawing.Point(16, 53);
            this.testPid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.testPid.Name = "testPid";
            this.testPid.Size = new System.Drawing.Size(288, 31);
            this.testPid.TabIndex = 9;
            this.testPid.Text = "&Test Pid";
            this.testPid.UseVisualStyleBackColor = true;
            this.testPid.Click += new System.EventHandler(this.testPid_Click);
            // 
            // pid
            // 
            this.pid.Location = new System.Drawing.Point(312, 57);
            this.pid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pid.Name = "pid";
            this.pid.Size = new System.Drawing.Size(167, 22);
            this.pid.TabIndex = 10;
            // 
            // message
            // 
            this.message.Location = new System.Drawing.Point(312, 96);
            this.message.Margin = new System.Windows.Forms.Padding(4);
            this.message.Multiline = true;
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(455, 126);
            this.message.TabIndex = 12;
            // 
            // sendMessage
            // 
            this.sendMessage.Location = new System.Drawing.Point(16, 92);
            this.sendMessage.Margin = new System.Windows.Forms.Padding(4);
            this.sendMessage.Name = "sendMessage";
            this.sendMessage.Size = new System.Drawing.Size(288, 31);
            this.sendMessage.TabIndex = 11;
            this.sendMessage.Text = "&Send Message";
            this.sendMessage.UseVisualStyleBackColor = true;
            this.sendMessage.Click += new System.EventHandler(this.sendMessage_Click);
            // 
            // PcmExplorerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 554);
            this.Controls.Add(this.message);
            this.Controls.Add(this.sendMessage);
            this.Controls.Add(this.pid);
            this.Controls.Add(this.testPid);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.deviceDescription);
            this.Controls.Add(this.selectButton);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PcmExplorerMainForm";
            this.Text = "PCM Explorer";
            this.Load += new System.EventHandler(this.PcmExplorerMainForm_Load);
            this.tabs.ResumeLayout(false);
            this.statusTab.ResumeLayout(false);
            this.statusTab.PerformLayout();
            this.debugTab.ResumeLayout(false);
            this.debugTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage statusTab;
        private System.Windows.Forms.TextBox userLog;
        private System.Windows.Forms.TabPage debugTab;
        private System.Windows.Forms.TextBox debugLog;
        private System.Windows.Forms.Label deviceDescription;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button testPid;
        private System.Windows.Forms.TextBox pid;
        private System.Windows.Forms.TextBox message;
        private System.Windows.Forms.Button sendMessage;
    }
}

