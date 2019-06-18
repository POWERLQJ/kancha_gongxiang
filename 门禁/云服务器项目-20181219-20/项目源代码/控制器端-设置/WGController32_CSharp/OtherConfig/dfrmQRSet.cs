using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ZXing.Common;
using ZXing;
using ZXing.QrCode;

namespace WGController32_CSharp.OtherConfig
{
    public partial class dfrmQRSet : Form
    {
        public dfrmQRSet()
        {
            InitializeComponent();
        }

        public string strSN = "";
        public string strIP = "";

        private void dfrmQRSet_Load(object sender, EventArgs e)
        {
            this.txtSN.Text = strSN;
            this.txtIP.Text = strIP;

        }
        void log(string info)  //日志信息
        {
            txtInfo.AppendText(string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), info)); //2015-11-03 20:55:49 显示时间
            txtInfo.ScrollToCaret();//滚动到光标处
            Application.DoEvents();
        }
        private void btnSetQRPassword_Click(object sender, EventArgs e) //2017-11-11 09:20:28 设置二维码密码
        {
            int ret = 0;
            int success = 0;  //0 失败, 1表示成功
            if (string.IsNullOrEmpty(txtSN.Text))
            {
                MessageBox.Show("请输入有效的控制器SN");
                return;
            }
            String ControllerIP = txtIP.Text;
            long controllerSN = long.Parse(txtSN.Text);

            //创建短报文 pkt
            WGPacketShort pkt = new WGPacketShort();
            pkt.iDevSn = controllerSN;
            pkt.IP = ControllerIP;

            //查询控制器驱动版本[功能号: 0x94] **********************************************************************************
            pkt.Reset();
            pkt.functionID = 0x94;
            ret = pkt.run();

            if (ret == 1)
            {
                string controllerVersion = "0"; //控制器版本
                controllerVersion = string.Format("{0:X}.{1:X}", pkt.recv[26], pkt.recv[27]);
                log(" 当前控制器驱动版本 = V" + controllerVersion);
                if (double.Parse(controllerVersion) < 8.92)
                {
                    MessageBox.Show(" 当前控制器驱动版本 = V" + controllerVersion + ",必须要8.92版本才行!");
                    return;
                }
            }
            else
            {
                log("查询控制器驱动版本 失败?????...");
                return;
            }


            pkt.Reset();

            string pwd = txtQRPassword.Text;
            byte[] sendData =
	          	{
	           	(byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, 
	            (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00};
            byte[] pwdData =
	          	{
	           	(byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, 
	            (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00};
            byte[] p = Encoding.UTF8.GetBytes(pwd);
            if (sender == this.btnSetQRPassword)
            {
                for (int i = 0; i < p.Length && i < 16; i++)
                {
                    pwdData[i] = p[i];
                }
            }
            pkt.functionID = 0xE0;
            //防止误操作标识
            Array.Copy(System.BitConverter.GetBytes(WGPacketShort.SpecialFlag), 0, pkt.data, 0, 4);
            for (int i = 0; i < 16; i++)  //设置新密码
            {
                pkt.data[4 + i] = pwdData[i];
                pkt.data[36 + i] = pwdData[i];
            }
            Array.Copy(System.BitConverter.GetBytes(WGPacketShort.SpecialFlag), 0, pkt.data, 20, 4);  //设置QR密码必须

            ret = pkt.run();
            success = 0;
            if (ret > 0)
            {
                //if (pkt.recv[8] == 1)
                {
                    if (sender == this.btnSetQRPassword)
                    {
                        log("QR二维码密码设置成功...");
                    }
                    else
                    {
                        log("清除QR二维码密码成功...");
                    }
                    success = 1;
                }
            }
            if (success == 0)
            {
                log("设置	 失败...");
            }
        }

        private void btnQRFunction_Click(object sender, EventArgs e)
        {
            int ret = 0;
            int success = 0;  //0 失败, 1表示成功
            if (string.IsNullOrEmpty(txtSN.Text))
            {
                MessageBox.Show("请输入有效的控制器SN");
                return;
            }
            String ControllerIP = txtIP.Text;
            long controllerSN = long.Parse(txtSN.Text);

            //创建短报文 pkt
            WGPacketShort pkt = new WGPacketShort();
            pkt.iDevSn = controllerSN;
            pkt.IP = ControllerIP;

            //查询控制器驱动版本[功能号: 0x94] **********************************************************************************
            pkt.Reset();
            pkt.functionID = 0x94;
            ret = pkt.run();

            success = 0;
            if (ret == 1)
            {
                string controllerVersion = "0"; //控制器版本
                controllerVersion = string.Format("{0:X}.{1:X}", pkt.recv[26], pkt.recv[27]);
                log(" 当前控制器驱动版本 = V" + controllerVersion);
                //if (float.Parse(controllerVersion) < 8.0)
                //{
                //    MessageBox.Show("控制器驱动版本低于V8.76. \r\n请将控制器返厂升级到最新驱动版本. \r\n或者更换新的高版本的控制器.");
                //}
                if (float.Parse(controllerVersion) < 8.92)  //2017-11-11 16:47:08
                {
                    MessageBox.Show("控制器驱动版本低于V8.92. \r\n请将控制器返厂升级到最新驱动版本. \r\n或者更换新的高版本的控制器.");
                }
            }
            else
            {
                log("查询控制器驱动版本 失败?????...");
                return;
            }

            //QR串口透传配置 协议文档请参看  20170708新增-设置双串口(二维码)-测试中V8.2以上.doc**********************************************************************************
            pkt.Reset();
            pkt.functionID = 0xF2;
            pkt.data[0] = 0x55; pkt.data[1] = 0xAA; pkt.data[2] = 0xAA; pkt.data[3] = 0x55;
            if (sender == this.btnQR1)
            {
                //透传
                pkt.data[4] = 0xF1; pkt.data[5] = 0x01; pkt.data[6] = 0x81;
            }
             else if (sender == this.btnQRRestore)
            {
                //关闭
                pkt.data[4] = 0xF1; pkt.data[5] = 0x01; pkt.data[6] = 0x0;
            }
            
            else
            {
                return; //
            }
            ret = pkt.run();
            success = 0;
            if (ret > 0)
            {
                if (pkt.recv[8] == 1)
                {
                    log(string.Format("二维码操作	 成功...{0}", (sender as Button).Text));
                    success = 1;

                }
            }
            if (success == 0)
            {
                log(string.Format("二维码操作	 失败????...{0}", (sender as Button).Text));
            }
        }


        /// <summary>
        /// 获取Hex值, 主要用于日期时间格式
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns>Hex值</returns>
        int GetHex(int val)
        {
            return ((val % 10) + (((val - (val % 10)) / 10) % 10) * 16);
        }
        /// <summary>
        /// 整型数转换为4字节数组
        /// </summary>
        /// <param name="outBytes">数组</param>
        /// <param name="startIndex">起始索引位(从0开始计)</param>
        /// <param name="val">数值</param>
        void LongToBytes(ref byte[] outBytes, int startIndex, long val)
        {
            Array.Copy(System.BitConverter.GetBytes(val), 0, outBytes, startIndex, 4);
        }

        private void btnUploadPrivilege_Click(object sender, EventArgs e)
        {
            int ret = 0;
            int success = 0;  //0 失败, 1表示成功
            if (string.IsNullOrEmpty(txtSN.Text))
            {
                MessageBox.Show("请输入有效的控制器SN");
                return;
            }
            String ControllerIP = txtIP.Text;
            long controllerSN = long.Parse(txtSN.Text);

            //创建短报文 pkt
            WGPacketShort pkt = new WGPacketShort();
            pkt.iDevSn = controllerSN;
            pkt.IP = ControllerIP;
            //1.11	权限添加或修改[功能号: 0x50] **********************************************************************************
            //增加卡号0D D7 37 00, 通过当前控制器的所有门
            pkt.Reset();
            pkt.functionID = 0x50;
            //0D D7 37 00 要添加或修改的权限中的卡号 = 0x0037D70D = 3659533 (十进制)
            long cardNOOfPrivilege = long.Parse(this.txtCardNO.Text); // 0x0037D70D;
            LongToBytes(ref pkt.data, 0, cardNOOfPrivilege);

            //20 10 01 01 起始日期:     (必须大于2001年)
            DateTime ptm = dtpActivate.Value;
            pkt.data[4] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100); //0x20;
            pkt.data[5] = (byte)GetHex((int)((ptm.Year) % 100)); // 0x10;
            pkt.data[6] = (byte)GetHex(ptm.Month); // 0x01;
            pkt.data[7] = (byte)GetHex(ptm.Day);
            //20 29 12 31 截止日期: 
            ptm = dtpDeactivate.Value;
            pkt.data[8] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100); //0x20;
            pkt.data[9] = (byte)GetHex((int)((ptm.Year) % 100)); // 0x10;
            pkt.data[10] = (byte)GetHex(ptm.Month); // 0x01;
            pkt.data[11] = (byte)GetHex(ptm.Day);

            pkt.data[30] = (byte)GetHex(ptm.Hour);   //时
            pkt.data[31] = (byte)GetHex(ptm.Minute); //分


            //01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
            pkt.data[12] = 0x01;
            //01 允许通过 二号门 [对双门, 四门控制器有效]
            pkt.data[13] = 0x01;  //如果禁止2号门, 则只要设为 0x00
            //01 允许通过 三号门 [对四门控制器有效]
            pkt.data[14] = 0x01;
            //01 允许通过 四号门 [对四门控制器有效]
            pkt.data[15] = 0x01;

            ret = pkt.run();
            success = 0;
            if (ret > 0)
            {
                if (pkt.recv[8] == 1)
                {
                    //这时 刷卡号为= 0x0037D70D = 3659533 (十进制)的卡, 1号门继电器动作.
                    log("1.11 权限添加或修改	 成功...");
                    success = 1;
                }
            }
            if (success < 1)
            {
                log("1.11 权限添加或修改	 失败...");
            }
        
        }

        /**************** QR 卡号输入部分 ***************************************************************************/
        //2017-09-21 21:21:12 加密操作完成 
        [DllImport("n3k_comm.dll", EntryPoint = "ShortEncryptSM4_ECB", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int ShortEncryptSM4_ECB(IntPtr command, int cmdLen, IntPtr password); //2015-09-28 12:05:54 加密WGPacketShort 包 短报文

        public static int EncryptSM4_ECB(ref byte[] command, byte[] password)  //2015-09-28 13:19:12 2013-4-2_07:31:32 加密数据
        {
            IntPtr pkt = Marshal.AllocHGlobal((Int32)64);

            Marshal.Copy(command, 0, pkt, 16);

            IntPtr commPassword = Marshal.AllocHGlobal((Int32)16);
            Marshal.Copy(password, 0, commPassword, 16);

            int ret = ShortEncryptSM4_ECB(pkt, 64, commPassword);
            if (ret > 0)
            {
                Marshal.Copy(pkt, command, 0, 16);  //复制回来
            }
            Marshal.FreeHGlobal(pkt); //2014-01-02 13:20:43 释放内存
            Marshal.FreeHGlobal(commPassword); //2014-01-02 13:20:43 释放内存
            return ret;

        }

        long getYMD(int Year, int Month, int Day)
        {
            long ymd;
            ymd = (Year % 100) << 9;
            ymd += (Month << 5);
            ymd += (Day);
            return (long)ymd;
        }

        long getHMS(int Hour, int Minute, int Second)
        {
            long hms = 0;
            hms += ((Second >> 1));
            hms += Minute << (5);
            hms += Hour << (11);
            return hms;
        }
        byte crc8(byte[] buf, int len)
        {
            byte i, j, crc;

            crc = 0;
            for (j = 0; j < len; j++)
            {
                crc = (byte)(crc ^ (buf[j]));
                for (i = 8; i > 0; i--)
                {
                    if ((crc & 0x80) > 0)
                    {
                        crc = (byte)((crc << 1) ^ 0x31);  //CRC=X8+X5+X4+1
                    }
                    else
                    {
                        crc = (byte)(crc << 1);
                    }
                }
                //buf++;
            }
            return crc;
        }


        private void btnCreateQRData_Click(object sender, EventArgs e) //2017-11-11 09:20:41 生成二维码数据
        {
            string pwd = txtQRPassword.Text;
            this.dtpActivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpActivate.Value.ToString("yyyy-MM-dd"), "00:00:00"));
            this.dtpDeactivate.Value = DateTime.Parse(string.Format("{0} {1}", this.dtpDeactivate.Value.ToString("yyyy-MM-dd"), this.dateEndHMS1.Value.ToString("HH:mm:ss")));
            byte[] sendData =
	          	{
	           	(byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, 
	            (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00};
            byte[] pwdData =
	          	{
	           	(byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x00, 
	            (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0x00, (byte)0x00};

            sendData[15] = 2; //固定

            //起始日期时间
            DateTime ptm = dtpActivate.Value;
            long ymd = getYMD(ptm.Year, ptm.Month, ptm.Day);
            long hms = getHMS(ptm.Hour, ptm.Minute, ptm.Second);
            sendData[8] = (byte)(ymd & 0xff);
            sendData[9] = (byte)((ymd >> 8) & 0xff);
            sendData[10] = (byte)((hms >> 8) & 0xff); //高位
            sendData[11] = (byte)(hms & 0xE0); //低位

            //截止日期时间
            ptm = dtpDeactivate.Value;
            ymd = getYMD(ptm.Year, ptm.Month, ptm.Day);
            hms = getHMS(ptm.Hour, ptm.Minute, ptm.Second);
            sendData[11] = (byte)(sendData[11] + ((hms & 0xE0) >> 4));
            sendData[12] = (byte)(ymd & 0xff);
            sendData[13] = (byte)((ymd >> 8) & 0xff);
            sendData[14] = (byte)((hms >> 8) & 0xff);
            if ((hms & 0xE0) == 0xE0) //2018-09-20 19:17:39 修改部分
            {
                sendData[14] = (byte)(sendData[14] + 1);
            }
            long cardNO = long.Parse(this.txtCardNO.Text);

            if (cardNO > 0)
            {
                sendData[0] = (byte)(cardNO & 0xff);
                sendData[1] = (byte)((cardNO >> 8) & 0xff);
                sendData[2] = (byte)((cardNO >> 16) & 0xff);
                sendData[3] = (byte)((cardNO >> 24) & 0xff);
                sendData[4] = (byte)((cardNO >> 32) & 0xff);
                sendData[5] = (byte)((cardNO >> 40) & 0xff);
                sendData[6] = (byte)((cardNO >> 48) & 0xff);
                sendData[7] = (byte)((cardNO >> 56) & 0xff);
                if (chkCardWithCRCCheck.Checked)  //2017-12-18 22:28:05 需要驱动V8.82版本
                {
                    sendData[7] = crc8(sendData, 7); //2017-12-18 22:03:47 校验和
                }
            }

            byte[] p = Encoding.UTF8.GetBytes(pwd);
            for (int i = 0; i < p.Length && i < 16; i++)
            {
                pwdData[i] = p[i];
            }
         

            EncryptSM4_ECB(ref sendData, pwdData);

            txtQRData.Text = System.BitConverter.ToString(sendData).Replace("-", "");

            // textBoxText.Text
            string info = System.BitConverter.ToString(sendData).Replace("-", "");

            EncodingOptions options = null;
            BarcodeWriter writer = null;
            options = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Margin = 1,
                Width = pictureBoxQr.Width,
                Height = pictureBoxQr.Height
            };
            writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = options;
            Bitmap bitmap = writer.Write(info);
            pictureBoxQr.Image = bitmap;
            pictureBoxQr.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
