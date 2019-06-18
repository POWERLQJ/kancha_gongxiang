namespace WGController32_CSharp
{
    partial class FormCloudServer
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
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtWatchServerPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWatchServerIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSendCommand = new System.Windows.Forms.Button();
            this.txt64Command = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkLogDetail = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboFloors = new System.Windows.Forms.ComboBox();
            this.btnOpenFloor = new System.Windows.Forms.Button();
            this.cboDoors = new System.Windows.Forms.ComboBox();
            this.btnControllerInfo = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAllControllerB = new System.Windows.Forms.CheckBox();
            this.btnUploadAllPrivileges = new System.Windows.Forms.Button();
            this.btnUploadPrivilegeSingle = new System.Windows.Forms.Button();
            this.txtCardNO = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dateEndHMS1 = new System.Windows.Forms.DateTimePicker();
            this.dtpDeactivate = new System.Windows.Forms.DateTimePicker();
            this.dtpActivate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAdjustTime = new System.Windows.Forms.Button();
            this.chkAllControllers = new System.Windows.Forms.CheckBox();
            this.chkGetAllSwipes = new System.Windows.Forms.CheckBox();
            this.btnGetAllSwipes = new System.Windows.Forms.Button();
            this.btnRemoteOpenDoor1 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstControllers = new System.Windows.Forms.ListBox();
            this.lstSwipe = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkDisplayIP = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(57, 78);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(216, 23);
            this.button4.TabIndex = 10;
            this.button4.Text = "打开接收服务器";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(29, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(216, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Stop 停止监控或测试";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtWatchServerPort
            // 
            this.txtWatchServerPort.Location = new System.Drawing.Point(173, 51);
            this.txtWatchServerPort.Name = "txtWatchServerPort";
            this.txtWatchServerPort.Size = new System.Drawing.Size(50, 21);
            this.txtWatchServerPort.TabIndex = 11;
            this.txtWatchServerPort.Text = "61005";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Watch Server Port\r\n接收服务器端口号";
            // 
            // txtWatchServerIP
            // 
            this.txtWatchServerIP.Location = new System.Drawing.Point(173, 12);
            this.txtWatchServerIP.Name = "txtWatchServerIP";
            this.txtWatchServerIP.Size = new System.Drawing.Size(100, 21);
            this.txtWatchServerIP.TabIndex = 8;
            this.txtWatchServerIP.Text = "192.168.168.101";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 24);
            this.label5.TabIndex = 9;
            this.label5.Text = "Watch Server IP\r\n接收服务器IP地址";
            // 
            // txtInfo
            // 
            this.txtInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInfo.Location = new System.Drawing.Point(5, 33);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(495, 386);
            this.txtInfo.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSendCommand);
            this.groupBox1.Controls.Add(this.txt64Command);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.chkLogDetail);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.cboDoors);
            this.groupBox1.Controls.Add(this.btnControllerInfo);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnAdjustTime);
            this.groupBox1.Controls.Add(this.chkAllControllers);
            this.groupBox1.Controls.Add(this.chkGetAllSwipes);
            this.groupBox1.Controls.Add(this.btnGetAllSwipes);
            this.groupBox1.Controls.Add(this.btnRemoteOpenDoor1);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.txtSN);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(28, 98);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1032, 180);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // btnSendCommand
            // 
            this.btnSendCommand.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSendCommand.Location = new System.Drawing.Point(902, 142);
            this.btnSendCommand.Name = "btnSendCommand";
            this.btnSendCommand.Size = new System.Drawing.Size(70, 23);
            this.btnSendCommand.TabIndex = 29;
            this.btnSendCommand.Text = "发送指令";
            this.btnSendCommand.UseVisualStyleBackColor = true;
            this.btnSendCommand.Click += new System.EventHandler(this.btnSendCommand_Click);
            // 
            // txt64Command
            // 
            this.txt64Command.Location = new System.Drawing.Point(678, 142);
            this.txt64Command.MaxLength = 192;
            this.txt64Command.Multiline = true;
            this.txt64Command.Name = "txt64Command";
            this.txt64Command.Size = new System.Drawing.Size(218, 32);
            this.txt64Command.TabIndex = 27;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(607, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "64字节指令";
            // 
            // chkLogDetail
            // 
            this.chkLogDetail.AutoSize = true;
            this.chkLogDetail.Checked = true;
            this.chkLogDetail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogDetail.Location = new System.Drawing.Point(34, 112);
            this.chkLogDetail.Name = "chkLogDetail";
            this.chkLogDetail.Size = new System.Drawing.Size(96, 16);
            this.chkLogDetail.TabIndex = 26;
            this.chkLogDetail.Text = "显示详细日志";
            this.chkLogDetail.UseVisualStyleBackColor = true;
            this.chkLogDetail.CheckedChanged += new System.EventHandler(this.chkLogDetail_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboFloors);
            this.groupBox4.Controls.Add(this.btnOpenFloor);
            this.groupBox4.Location = new System.Drawing.Point(295, 134);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(295, 43);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "一对多(或梯控)";
            // 
            // cboFloors
            // 
            this.cboFloors.FormattingEnabled = true;
            this.cboFloors.Items.AddRange(new object[] {
            "1号",
            "2号",
            "3号",
            "4号"});
            this.cboFloors.Location = new System.Drawing.Point(100, 16);
            this.cboFloors.Name = "cboFloors";
            this.cboFloors.Size = new System.Drawing.Size(54, 20);
            this.cboFloors.TabIndex = 25;
            this.cboFloors.Text = "1号门";
            // 
            // btnOpenFloor
            // 
            this.btnOpenFloor.Location = new System.Drawing.Point(15, 14);
            this.btnOpenFloor.Name = "btnOpenFloor";
            this.btnOpenFloor.Size = new System.Drawing.Size(79, 23);
            this.btnOpenFloor.TabIndex = 18;
            this.btnOpenFloor.Text = "远程开(NO)";
            this.btnOpenFloor.UseVisualStyleBackColor = true;
            this.btnOpenFloor.Click += new System.EventHandler(this.btnOpenFloor_Click);
            // 
            // cboDoors
            // 
            this.cboDoors.FormattingEnabled = true;
            this.cboDoors.Items.AddRange(new object[] {
            "1号门",
            "2号门",
            "3号门",
            "4号门"});
            this.cboDoors.Location = new System.Drawing.Point(395, 55);
            this.cboDoors.Name = "cboDoors";
            this.cboDoors.Size = new System.Drawing.Size(54, 20);
            this.cboDoors.TabIndex = 24;
            this.cboDoors.Text = "1号门";
            // 
            // btnControllerInfo
            // 
            this.btnControllerInfo.Location = new System.Drawing.Point(518, 25);
            this.btnControllerInfo.Name = "btnControllerInfo";
            this.btnControllerInfo.Size = new System.Drawing.Size(53, 23);
            this.btnControllerInfo.TabIndex = 23;
            this.btnControllerInfo.Text = "信息";
            this.btnControllerInfo.UseVisualStyleBackColor = true;
            this.btnControllerInfo.Click += new System.EventHandler(this.btnControllerInfo_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAllControllerB);
            this.groupBox2.Controls.Add(this.btnUploadAllPrivileges);
            this.groupBox2.Controls.Add(this.btnUploadPrivilegeSingle);
            this.groupBox2.Controls.Add(this.txtCardNO);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.dateEndHMS1);
            this.groupBox2.Controls.Add(this.dtpDeactivate);
            this.groupBox2.Controls.Add(this.dtpActivate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(597, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 123);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "用户权限";
            // 
            // chkAllControllerB
            // 
            this.chkAllControllerB.AutoSize = true;
            this.chkAllControllerB.Location = new System.Drawing.Point(297, 16);
            this.chkAllControllerB.Name = "chkAllControllerB";
            this.chkAllControllerB.Size = new System.Drawing.Size(108, 16);
            this.chkAllControllerB.TabIndex = 34;
            this.chkAllControllerB.Text = "上传到全部设备";
            this.chkAllControllerB.UseVisualStyleBackColor = true;
            // 
            // btnUploadAllPrivileges
            // 
            this.btnUploadAllPrivileges.Location = new System.Drawing.Point(266, 41);
            this.btnUploadAllPrivileges.Name = "btnUploadAllPrivileges";
            this.btnUploadAllPrivileges.Size = new System.Drawing.Size(157, 44);
            this.btnUploadAllPrivileges.TabIndex = 33;
            this.btnUploadAllPrivileges.Text = "上传更新所有权限[1万条] 1024字节指令实现";
            this.btnUploadAllPrivileges.UseVisualStyleBackColor = true;
            this.btnUploadAllPrivileges.Click += new System.EventHandler(this.btnUploadAllPrivileges_Click);
            // 
            // btnUploadPrivilegeSingle
            // 
            this.btnUploadPrivilegeSingle.Location = new System.Drawing.Point(18, 93);
            this.btnUploadPrivilegeSingle.Name = "btnUploadPrivilegeSingle";
            this.btnUploadPrivilegeSingle.Size = new System.Drawing.Size(171, 23);
            this.btnUploadPrivilegeSingle.TabIndex = 32;
            this.btnUploadPrivilegeSingle.Text = "上传单个权限(单台控制器)";
            this.btnUploadPrivilegeSingle.UseVisualStyleBackColor = true;
            this.btnUploadPrivilegeSingle.Click += new System.EventHandler(this.btnUploadPrivilegeSingle_Click);
            // 
            // txtCardNO
            // 
            this.txtCardNO.Location = new System.Drawing.Point(70, 14);
            this.txtCardNO.Name = "txtCardNO";
            this.txtCardNO.Size = new System.Drawing.Size(100, 21);
            this.txtCardNO.TabIndex = 30;
            this.txtCardNO.Text = "12345";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "卡号";
            // 
            // dateEndHMS1
            // 
            this.dateEndHMS1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateEndHMS1.Location = new System.Drawing.Point(149, 67);
            this.dateEndHMS1.Name = "dateEndHMS1";
            this.dateEndHMS1.ShowUpDown = true;
            this.dateEndHMS1.Size = new System.Drawing.Size(73, 21);
            this.dateEndHMS1.TabIndex = 26;
            this.dateEndHMS1.Value = new System.DateTime(2016, 9, 2, 23, 59, 0, 0);
            // 
            // dtpDeactivate
            // 
            this.dtpDeactivate.Location = new System.Drawing.Point(43, 67);
            this.dtpDeactivate.Name = "dtpDeactivate";
            this.dtpDeactivate.Size = new System.Drawing.Size(104, 21);
            this.dtpDeactivate.TabIndex = 25;
            this.dtpDeactivate.Value = new System.DateTime(2029, 12, 31, 23, 59, 0, 0);
            // 
            // dtpActivate
            // 
            this.dtpActivate.Location = new System.Drawing.Point(70, 41);
            this.dtpActivate.Name = "dtpActivate";
            this.dtpActivate.Size = new System.Drawing.Size(112, 21);
            this.dtpActivate.TabIndex = 27;
            this.dtpActivate.Value = new System.DateTime(2010, 1, 1, 18, 18, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(14, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 28;
            this.label7.Text = "有效期从:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(14, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 29;
            this.label8.Text = "到:";
            // 
            // btnAdjustTime
            // 
            this.btnAdjustTime.Location = new System.Drawing.Point(455, 52);
            this.btnAdjustTime.Name = "btnAdjustTime";
            this.btnAdjustTime.Size = new System.Drawing.Size(116, 23);
            this.btnAdjustTime.TabIndex = 21;
            this.btnAdjustTime.Text = "校准时间";
            this.btnAdjustTime.UseVisualStyleBackColor = true;
            this.btnAdjustTime.Click += new System.EventHandler(this.btnAdjustTime_Click);
            // 
            // chkAllControllers
            // 
            this.chkAllControllers.AutoSize = true;
            this.chkAllControllers.Location = new System.Drawing.Point(310, 86);
            this.chkAllControllers.Name = "chkAllControllers";
            this.chkAllControllers.Size = new System.Drawing.Size(108, 16);
            this.chkAllControllers.TabIndex = 20;
            this.chkAllControllers.Text = "取全部设备记录";
            this.chkAllControllers.UseVisualStyleBackColor = true;
            // 
            // chkGetAllSwipes
            // 
            this.chkGetAllSwipes.AutoSize = true;
            this.chkGetAllSwipes.Location = new System.Drawing.Point(427, 86);
            this.chkGetAllSwipes.Name = "chkGetAllSwipes";
            this.chkGetAllSwipes.Size = new System.Drawing.Size(144, 16);
            this.chkGetAllSwipes.TabIndex = 19;
            this.chkGetAllSwipes.Text = "取所有记录(忽略标记)";
            this.chkGetAllSwipes.UseVisualStyleBackColor = true;
            // 
            // btnGetAllSwipes
            // 
            this.btnGetAllSwipes.Location = new System.Drawing.Point(310, 108);
            this.btnGetAllSwipes.Name = "btnGetAllSwipes";
            this.btnGetAllSwipes.Size = new System.Drawing.Size(216, 23);
            this.btnGetAllSwipes.TabIndex = 18;
            this.btnGetAllSwipes.Text = "(1024字节指令实现 提取记录)";
            this.btnGetAllSwipes.UseVisualStyleBackColor = true;
            this.btnGetAllSwipes.Click += new System.EventHandler(this.btnGetAllSwipes_Click);
            // 
            // btnRemoteOpenDoor1
            // 
            this.btnRemoteOpenDoor1.Location = new System.Drawing.Point(310, 54);
            this.btnRemoteOpenDoor1.Name = "btnRemoteOpenDoor1";
            this.btnRemoteOpenDoor1.Size = new System.Drawing.Size(79, 23);
            this.btnRemoteOpenDoor1.TabIndex = 17;
            this.btnRemoteOpenDoor1.Text = "远程开";
            this.btnRemoteOpenDoor1.UseVisualStyleBackColor = true;
            this.btnRemoteOpenDoor1.Click += new System.EventHandler(this.btnRemoteOpenDoor_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(34, 83);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(150, 16);
            this.checkBox2.TabIndex = 16;
            this.checkBox2.Text = "二维码数据-->远程开门";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(34, 61);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(138, 16);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "刷卡记录-->远程开门";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtSN
            // 
            this.txtSN.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtSN.Location = new System.Drawing.Point(412, 25);
            this.txtSN.Name = "txtSN";
            this.txtSN.ReadOnly = true;
            this.txtSN.Size = new System.Drawing.Size(100, 21);
            this.txtSN.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(323, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Controller SN\r\n 控制器序列号";
            // 
            // lstControllers
            // 
            this.lstControllers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstControllers.FormattingEnabled = true;
            this.lstControllers.ItemHeight = 12;
            this.lstControllers.Location = new System.Drawing.Point(506, 31);
            this.lstControllers.Name = "lstControllers";
            this.lstControllers.Size = new System.Drawing.Size(119, 388);
            this.lstControllers.TabIndex = 16;
            this.lstControllers.Click += new System.EventHandler(this.lstControllers_Click);
            // 
            // lstSwipe
            // 
            this.lstSwipe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSwipe.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lstSwipe.Location = new System.Drawing.Point(631, 30);
            this.lstSwipe.Name = "lstSwipe";
            this.lstSwipe.Size = new System.Drawing.Size(386, 389);
            this.lstSwipe.TabIndex = 18;
            this.lstSwipe.UseCompatibleStateImageBehavior = false;
            this.lstSwipe.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "卡号";
            this.columnHeader1.Text = "卡号";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "时间";
            this.columnHeader2.Width = 165;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "SN";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "门号";
            this.columnHeader4.Width = 39;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(629, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "实时刷卡记录";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(507, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "已接入的控制器";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(375, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(525, 84);
            this.label6.TabIndex = 21;
            this.label6.Text = "此应用 关注点:  (控制器主动上传数据)\r\n1. 实时监控最新的刷卡记录\r\n2. 对接收到的记录分析 发出远程开门指令  \r\n3. 实时接收二维码信息, 分析后" +
    "发出远程开门指令.\r\n4. 应用以正常通信为基准, [丢包率低]    对于发出的指令如果控制器没有接收到,\r\n   用户可以再刷一次卡, 这样重试达到开门效果" +
    "";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.txtInfo);
            this.groupBox3.Controls.Add(this.lstControllers);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.lstSwipe);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(22, 284);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1032, 430);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "信息";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(35, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkDisplayIP
            // 
            this.chkDisplayIP.AutoSize = true;
            this.chkDisplayIP.Location = new System.Drawing.Point(309, 82);
            this.chkDisplayIP.Name = "chkDisplayIP";
            this.chkDisplayIP.Size = new System.Drawing.Size(60, 16);
            this.chkDisplayIP.TabIndex = 30;
            this.chkDisplayIP.Text = "显示IP";
            this.chkDisplayIP.UseVisualStyleBackColor = true;
            this.chkDisplayIP.CheckedChanged += new System.EventHandler(this.chkDisplayIP_CheckedChanged);
            // 
            // FormCloudServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 714);
            this.Controls.Add(this.chkDisplayIP);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtWatchServerPort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWatchServerIP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox3);
            this.Name = "FormCloudServer";
            this.Text = "服务器控制 V4.5 - 20180912";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCloudServer_FormClosing);
            this.Load += new System.EventHandler(this.FormCloudServer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtWatchServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWatchServerIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnRemoteOpenDoor1;
        private System.Windows.Forms.ListBox lstControllers;
        private System.Windows.Forms.ListView lstSwipe;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGetAllSwipes;
        private System.Windows.Forms.CheckBox chkGetAllSwipes;
        private System.Windows.Forms.CheckBox chkAllControllers;
        private System.Windows.Forms.Button btnAdjustTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnUploadPrivilegeSingle;
        private System.Windows.Forms.TextBox txtCardNO;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.DateTimePicker dateEndHMS1;
        public System.Windows.Forms.DateTimePicker dtpDeactivate;
        public System.Windows.Forms.DateTimePicker dtpActivate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnUploadAllPrivileges;
        private System.Windows.Forms.CheckBox chkAllControllerB;
        private System.Windows.Forms.Button btnControllerInfo;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ComboBox cboDoors;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboFloors;
        private System.Windows.Forms.Button btnOpenFloor;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkLogDetail;
        private System.Windows.Forms.Button btnSendCommand;
        private System.Windows.Forms.TextBox txt64Command;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkDisplayIP;
    }
}