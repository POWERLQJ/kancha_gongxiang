﻿namespace WGController32_CSharp
{
    partial class dfrmTCPIPConfigure
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dfrmTCPIPConfigure));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtf_ControllerSN = new System.Windows.Forms.TextBox();
            this.txtf_MACAddr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtf_IP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtf_mask = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtf_gateway = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.optDhcp = new System.Windows.Forms.RadioButton();
            this.optSetIP = new System.Windows.Forms.RadioButton();
            this.grpIP = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudCycle = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPortShort = new System.Windows.Forms.TextBox();
            this.txtHostIP = new System.Windows.Forms.TextBox();
            this.label197 = new System.Windows.Forms.Label();
            this.chkEditDateServer = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkUploadInputStatusChange = new System.Windows.Forms.CheckBox();
            this.grpIP.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycle)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // txtf_ControllerSN
            // 
            resources.ApplyResources(this.txtf_ControllerSN, "txtf_ControllerSN");
            this.txtf_ControllerSN.Name = "txtf_ControllerSN";
            this.txtf_ControllerSN.ReadOnly = true;
            this.txtf_ControllerSN.TabStop = false;
            // 
            // txtf_MACAddr
            // 
            resources.ApplyResources(this.txtf_MACAddr, "txtf_MACAddr");
            this.txtf_MACAddr.Name = "txtf_MACAddr";
            this.txtf_MACAddr.ReadOnly = true;
            this.txtf_MACAddr.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // txtf_IP
            // 
            resources.ApplyResources(this.txtf_IP, "txtf_IP");
            this.txtf_IP.Name = "txtf_IP";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Name = "label4";
            // 
            // txtf_mask
            // 
            resources.ApplyResources(this.txtf_mask, "txtf_mask");
            this.txtf_mask.Name = "txtf_mask";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Name = "label5";
            // 
            // txtf_gateway
            // 
            resources.ApplyResources(this.txtf_gateway, "txtf_gateway");
            this.txtf_gateway.Name = "txtf_gateway";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // optDhcp
            // 
            resources.ApplyResources(this.optDhcp, "optDhcp");
            this.optDhcp.Name = "optDhcp";
            this.optDhcp.TabStop = true;
            this.optDhcp.UseVisualStyleBackColor = true;
            // 
            // optSetIP
            // 
            resources.ApplyResources(this.optSetIP, "optSetIP");
            this.optSetIP.Name = "optSetIP";
            this.optSetIP.TabStop = true;
            this.optSetIP.UseVisualStyleBackColor = true;
            this.optSetIP.CheckedChanged += new System.EventHandler(this.optSetIP_CheckedChanged);
            // 
            // grpIP
            // 
            this.grpIP.Controls.Add(this.txtf_IP);
            this.grpIP.Controls.Add(this.label3);
            this.grpIP.Controls.Add(this.txtf_mask);
            this.grpIP.Controls.Add(this.label4);
            this.grpIP.Controls.Add(this.label5);
            this.grpIP.Controls.Add(this.txtf_gateway);
            resources.ApplyResources(this.grpIP, "grpIP");
            this.grpIP.Name = "grpIP";
            this.grpIP.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkUploadInputStatusChange);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.nudCycle);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtPortShort);
            this.groupBox2.Controls.Add(this.txtHostIP);
            this.groupBox2.Controls.Add(this.label197);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Name = "label9";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Name = "label7";
            // 
            // nudCycle
            // 
            this.nudCycle.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.nudCycle, "nudCycle");
            this.nudCycle.Maximum = new decimal(new int[] {
            253,
            0,
            0,
            0});
            this.nudCycle.Name = "nudCycle";
            this.nudCycle.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Name = "label8";
            // 
            // txtPortShort
            // 
            this.txtPortShort.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.txtPortShort, "txtPortShort");
            this.txtPortShort.Name = "txtPortShort";
            // 
            // txtHostIP
            // 
            this.txtHostIP.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.txtHostIP, "txtHostIP");
            this.txtHostIP.Name = "txtHostIP";
            // 
            // label197
            // 
            resources.ApplyResources(this.label197, "label197");
            this.label197.ForeColor = System.Drawing.Color.Black;
            this.label197.Name = "label197";
            // 
            // chkEditDateServer
            // 
            resources.ApplyResources(this.chkEditDateServer, "chkEditDateServer");
            this.chkEditDateServer.BackColor = System.Drawing.Color.Transparent;
            this.chkEditDateServer.Checked = true;
            this.chkEditDateServer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEditDateServer.ForeColor = System.Drawing.Color.Black;
            this.chkEditDateServer.Name = "chkEditDateServer";
            this.chkEditDateServer.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkUploadInputStatusChange
            // 
            resources.ApplyResources(this.chkUploadInputStatusChange, "chkUploadInputStatusChange");
            this.chkUploadInputStatusChange.BackColor = System.Drawing.Color.Transparent;
            this.chkUploadInputStatusChange.ForeColor = System.Drawing.Color.Black;
            this.chkUploadInputStatusChange.Name = "chkUploadInputStatusChange";
            this.chkUploadInputStatusChange.UseVisualStyleBackColor = false;
            // 
            // dfrmTCPIPConfigure
            // 
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chkEditDateServer);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.optSetIP);
            this.Controls.Add(this.txtf_ControllerSN);
            this.Controls.Add(this.optDhcp);
            this.Controls.Add(this.txtf_MACAddr);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dfrmTCPIPConfigure";
            this.Load += new System.EventHandler(this.dfrmTCPIPConfigure_Load);
            this.grpIP.ResumeLayout(false);
            this.grpIP.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCycle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtf_ControllerSN;
        private System.Windows.Forms.TextBox txtf_MACAddr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtf_IP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtf_mask;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtf_gateway;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton optDhcp;
        private System.Windows.Forms.RadioButton optSetIP;
        private System.Windows.Forms.GroupBox grpIP;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudCycle;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPortShort;
        private System.Windows.Forms.TextBox txtHostIP;
        private System.Windows.Forms.Label label197;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkEditDateServer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkUploadInputStatusChange;
    }
}
