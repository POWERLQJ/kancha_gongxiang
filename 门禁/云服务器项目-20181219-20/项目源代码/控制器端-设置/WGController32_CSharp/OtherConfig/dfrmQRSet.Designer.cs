namespace WGController32_CSharp.OtherConfig
{
    partial class dfrmQRSet
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnQRRestore = new System.Windows.Forms.Button();
            this.btnQR1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkCardWithCRCCheck = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUploadPrivilege = new System.Windows.Forms.Button();
            this.txtQRData = new System.Windows.Forms.TextBox();
            this.btnCreateQRData = new System.Windows.Forms.Button();
            this.txtCardNO = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dateEndHMS1 = new System.Windows.Forms.DateTimePicker();
            this.dtpDeactivate = new System.Windows.Forms.DateTimePicker();
            this.dtpActivate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnClearQRPassword = new System.Windows.Forms.Button();
            this.btnSetQRPassword = new System.Windows.Forms.Button();
            this.txtQRPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBoxQr = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQr)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(35, 86);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(414, 383);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnQRRestore);
            this.tabPage1.Controls.Add(this.btnQR1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(406, 357);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "QR 二维码[透传]";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnQRRestore
            // 
            this.btnQRRestore.Location = new System.Drawing.Point(15, 94);
            this.btnQRRestore.Name = "btnQRRestore";
            this.btnQRRestore.Size = new System.Drawing.Size(194, 23);
            this.btnQRRestore.TabIndex = 10;
            this.btnQRRestore.Text = "停用 透传";
            this.btnQRRestore.UseVisualStyleBackColor = true;
            this.btnQRRestore.Click += new System.EventHandler(this.btnQRFunction_Click);
            // 
            // btnQR1
            // 
            this.btnQR1.Location = new System.Drawing.Point(15, 37);
            this.btnQR1.Name = "btnQR1";
            this.btnQR1.Size = new System.Drawing.Size(194, 23);
            this.btnQR1.TabIndex = 8;
            this.btnQR1.Text = "启用 透传";
            this.btnQR1.UseVisualStyleBackColor = true;
            this.btnQR1.Click += new System.EventHandler(this.btnQRFunction_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkCardWithCRCCheck);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.btnUploadPrivilege);
            this.tabPage2.Controls.Add(this.txtQRData);
            this.tabPage2.Controls.Add(this.btnCreateQRData);
            this.tabPage2.Controls.Add(this.txtCardNO);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.dateEndHMS1);
            this.tabPage2.Controls.Add(this.dtpDeactivate);
            this.tabPage2.Controls.Add(this.dtpActivate);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.btnClearQRPassword);
            this.tabPage2.Controls.Add(this.btnSetQRPassword);
            this.tabPage2.Controls.Add(this.txtQRPassword);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(406, 357);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "QR 二维码[采用SM4_ECB加密]";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkCardWithCRCCheck
            // 
            this.chkCardWithCRCCheck.AutoSize = true;
            this.chkCardWithCRCCheck.Location = new System.Drawing.Point(210, 182);
            this.chkCardWithCRCCheck.Name = "chkCardWithCRCCheck";
            this.chkCardWithCRCCheck.Size = new System.Drawing.Size(90, 16);
            this.chkCardWithCRCCheck.TabIndex = 26;
            this.chkCardWithCRCCheck.Text = "加入CRC校验";
            this.chkCardWithCRCCheck.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 25;
            this.label2.Text = "测试卡号及二维码";
            // 
            // btnUploadPrivilege
            // 
            this.btnUploadPrivilege.Location = new System.Drawing.Point(25, 290);
            this.btnUploadPrivilege.Name = "btnUploadPrivilege";
            this.btnUploadPrivilege.Size = new System.Drawing.Size(79, 23);
            this.btnUploadPrivilege.TabIndex = 24;
            this.btnUploadPrivilege.Text = "3. 上传权限";
            this.btnUploadPrivilege.UseVisualStyleBackColor = true;
            this.btnUploadPrivilege.Click += new System.EventHandler(this.btnUploadPrivilege_Click);
            // 
            // txtQRData
            // 
            this.txtQRData.Location = new System.Drawing.Point(25, 319);
            this.txtQRData.Name = "txtQRData";
            this.txtQRData.Size = new System.Drawing.Size(340, 21);
            this.txtQRData.TabIndex = 23;
            // 
            // btnCreateQRData
            // 
            this.btnCreateQRData.Location = new System.Drawing.Point(131, 290);
            this.btnCreateQRData.Name = "btnCreateQRData";
            this.btnCreateQRData.Size = new System.Drawing.Size(118, 23);
            this.btnCreateQRData.TabIndex = 22;
            this.btnCreateQRData.Text = "4. 生成二维码数据";
            this.btnCreateQRData.UseVisualStyleBackColor = true;
            this.btnCreateQRData.Click += new System.EventHandler(this.btnCreateQRData_Click);
            // 
            // txtCardNO
            // 
            this.txtCardNO.Location = new System.Drawing.Point(79, 210);
            this.txtCardNO.Name = "txtCardNO";
            this.txtCardNO.Size = new System.Drawing.Size(100, 21);
            this.txtCardNO.TabIndex = 20;
            this.txtCardNO.Text = "12345";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 213);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "卡号";
            // 
            // dateEndHMS1
            // 
            this.dateEndHMS1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateEndHMS1.Location = new System.Drawing.Point(158, 263);
            this.dateEndHMS1.Name = "dateEndHMS1";
            this.dateEndHMS1.ShowUpDown = true;
            this.dateEndHMS1.Size = new System.Drawing.Size(73, 21);
            this.dateEndHMS1.TabIndex = 15;
            this.dateEndHMS1.Value = new System.DateTime(2016, 9, 2, 23, 59, 0, 0);
            // 
            // dtpDeactivate
            // 
            this.dtpDeactivate.Location = new System.Drawing.Point(52, 263);
            this.dtpDeactivate.Name = "dtpDeactivate";
            this.dtpDeactivate.Size = new System.Drawing.Size(104, 21);
            this.dtpDeactivate.TabIndex = 14;
            this.dtpDeactivate.Value = new System.DateTime(2029, 12, 31, 23, 59, 0, 0);
            // 
            // dtpActivate
            // 
            this.dtpActivate.Location = new System.Drawing.Point(79, 237);
            this.dtpActivate.Name = "dtpActivate";
            this.dtpActivate.Size = new System.Drawing.Size(112, 21);
            this.dtpActivate.TabIndex = 16;
            this.dtpActivate.Value = new System.DateTime(2010, 1, 1, 18, 18, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(23, 243);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "有效期从:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(23, 266);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "到:";
            // 
            // btnClearQRPassword
            // 
            this.btnClearQRPassword.Location = new System.Drawing.Point(39, 121);
            this.btnClearQRPassword.Name = "btnClearQRPassword";
            this.btnClearQRPassword.Size = new System.Drawing.Size(100, 23);
            this.btnClearQRPassword.TabIndex = 13;
            this.btnClearQRPassword.Text = "清除二维码密码";
            this.btnClearQRPassword.UseVisualStyleBackColor = true;
            this.btnClearQRPassword.Click += new System.EventHandler(this.btnSetQRPassword_Click);
            // 
            // btnSetQRPassword
            // 
            this.btnSetQRPassword.Location = new System.Drawing.Point(14, 63);
            this.btnSetQRPassword.Name = "btnSetQRPassword";
            this.btnSetQRPassword.Size = new System.Drawing.Size(125, 23);
            this.btnSetQRPassword.TabIndex = 12;
            this.btnSetQRPassword.Text = "1.设置二维码密码";
            this.btnSetQRPassword.UseVisualStyleBackColor = true;
            this.btnSetQRPassword.Click += new System.EventHandler(this.btnSetQRPassword_Click);
            // 
            // txtQRPassword
            // 
            this.txtQRPassword.Location = new System.Drawing.Point(114, 16);
            this.txtQRPassword.MaxLength = 16;
            this.txtQRPassword.Name = "txtQRPassword";
            this.txtQRPassword.Size = new System.Drawing.Size(118, 21);
            this.txtQRPassword.TabIndex = 3;
            this.txtQRPassword.Text = "0123456789ABCDEF";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 24);
            this.label6.TabIndex = 4;
            this.label6.Text = "定义密码\r\n(16个数字或字母)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(37, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "Controller SN\r\n控制器 SN";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSN
            // 
            this.txtSN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSN.Location = new System.Drawing.Point(156, 14);
            this.txtSN.Name = "txtSN";
            this.txtSN.ReadOnly = true;
            this.txtSN.Size = new System.Drawing.Size(233, 21);
            this.txtSN.TabIndex = 13;
            this.txtSN.TabStop = false;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(156, 46);
            this.txtIP.Name = "txtIP";
            this.txtIP.ReadOnly = true;
            this.txtIP.Size = new System.Drawing.Size(233, 21);
            this.txtIP.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(33, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "IP Address IP地址";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInfo
            // 
            this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfo.Location = new System.Drawing.Point(475, 8);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(397, 307);
            this.txtInfo.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(352, 543);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBoxQr
            // 
            this.pictureBoxQr.BackColor = System.Drawing.Color.White;
            this.pictureBoxQr.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBoxQr.Location = new System.Drawing.Point(613, 338);
            this.pictureBoxQr.Name = "pictureBoxQr";
            this.pictureBoxQr.Size = new System.Drawing.Size(240, 228);
            this.pictureBoxQr.TabIndex = 18;
            this.pictureBoxQr.TabStop = false;
            this.pictureBoxQr.Visible = false;
            // 
            // dfrmQRSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 578);
            this.Controls.Add(this.pictureBoxQr);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.tabControl1);
            this.Name = "dfrmQRSet";
            this.Text = "dfrmQRSet";
            this.Load += new System.EventHandler(this.dfrmQRSet_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnUploadPrivilege;
        private System.Windows.Forms.TextBox txtQRData;
        private System.Windows.Forms.Button btnCreateQRData;
        private System.Windows.Forms.TextBox txtCardNO;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.DateTimePicker dateEndHMS1;
        public System.Windows.Forms.DateTimePicker dtpDeactivate;
        public System.Windows.Forms.DateTimePicker dtpActivate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClearQRPassword;
        private System.Windows.Forms.Button btnSetQRPassword;
        private System.Windows.Forms.TextBox txtQRPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnQRRestore;
        private System.Windows.Forms.Button btnQR1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBoxQr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkCardWithCRCCheck;
    }
}