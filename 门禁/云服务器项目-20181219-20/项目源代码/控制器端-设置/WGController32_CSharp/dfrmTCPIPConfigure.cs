﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using System.Net;

namespace WGController32_CSharp
{
    public partial class dfrmTCPIPConfigure : Form
    {
        public dfrmTCPIPConfigure()
        {
            InitializeComponent();
        }

        public string strSN = "";
        public string strMac = "";
        public string strIP = "";
        public string strMask = "";
        public string strGateway = "";
        public string strPCAddr = "";
        



        //            '输入的IP要求是首字节不能为00, 最后字节不能为255
        public Boolean isIPAddress(string ipstr)
        {
            Boolean ret = false;
            try
            {
                if (string.IsNullOrEmpty(ipstr))
                {
                }
                else
                {

                    string[] strIPInput = ipstr.Split('.');
                    if (strIPInput.Length == 4)
                    {
                        int itemp;
                        ret = true;
                        for (int i = 0; i <= 3; i++)
                        {
                            //'数值0到255
                            if (!int.TryParse(strIPInput[i], out itemp))
                            {
                                ret = false;

                                break;
                            }

                            if (!((itemp >= 0) && (itemp <= 255)))
                            {
                                ret = false;
                                break;
                            }
                        }
                        if (int.Parse(strIPInput[0]) == 0) // '第一个值不能为0 
                        {
                            ret = false;

                        }
                        else if (int.Parse(strIPInput[3]) == 255) //最后一个值不能为255 
                        {
                            ret = false;

                        }
                    }
                }
            }
            catch
            {
                ret = false;
            }
            finally
            {
            }
            return ret;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //新的值
            int itemp;
            if (this.txtf_ControllerSN.ReadOnly == false)
            {
                this.txtf_ControllerSN.Text = this.txtf_ControllerSN.Text.Trim();
                if (!int.TryParse(this.txtf_ControllerSN.Text, out itemp))
                {
                   // MessageBox.Show("Controller SN  Wrong");
                   // MessageBox.Show("Controller 控制器 SN 不正确...");
                    MessageBox.Show(string.Format("Controller 控制器 SN: {0}  不正确", this.txtf_ControllerSN.Text));
                    return;
                }

            }

            if (optSetIP.Checked)
            {
            this.txtf_IP.Text = this.txtf_IP.Text.Replace(" ", ""); // '排除空格
                if (!isIPAddress(this.txtf_IP.Text))
                {
                    //               MessageBox.Show("IP  Wrong");
                    //MessageBox.Show("控制器IP地址 不正确...");
                    MessageBox.Show(string.Format("控制器IP地址: {0}  不正确", this.txtf_IP.Text));
                    return;
                }
            this.txtf_mask.Text = this.txtf_mask.Text.Replace(" ", ""); // '排除空格
            if (!isIPAddress(this.txtf_mask.Text))
            {
                //MessageBox.Show("Subnet Mask 掩码  不正确");
                MessageBox.Show(string.Format("Subnet Mask 掩码: {0}  不正确", this.txtf_mask.Text));
                return;
            }

            this.txtf_gateway.Text = this.txtf_gateway.Text.Replace(" ", ""); // '排除空格
            if (!string.IsNullOrEmpty(this.txtf_gateway.Text))
            {
                if (!isIPAddress(this.txtf_gateway.Text))
                {
                   // MessageBox.Show("Gateway 网关  不正确");
                    MessageBox.Show(string.Format("Gateway 网关: {0}  不正确", this.txtf_gateway.Text));
                    return;
                }
            }
            }

            if (this.chkEditDateServer.Checked)
            {
            this.txtHostIP.Text = this.txtHostIP.Text.Replace(" ", ""); // '排除空格
                if (!string.IsNullOrEmpty(this.txtHostIP.Text))
                {
                    if (!isIPAddress(this.txtHostIP.Text))
                    {
                        MessageBox.Show(string.Format("接收服务器IP: {0}  不正确", this.txtHostIP.Text));
                        return;
                    }
                }
            }

            strSN = this.txtf_ControllerSN.Text;
            strMac = this.txtf_MACAddr.Text;
            strIP = this.txtf_IP.Text;
            strMask = this.txtf_mask.Text;
            strGateway = this.txtf_gateway.Text;

            //修改IP
            int ret = 0;
            //创建短报文 pkt

            WGPacketShort pkt = new WGPacketShort();
            pkt.iDevSn = long.Parse(strSN); // controllerSN;
            //设置接收服务器
            if (this.chkEditDateServer.Checked)
            {
                pkt.Reset();
                pkt.functionID = 0x90;
                if (!string.IsNullOrEmpty(this.txtHostIP.Text))
                {
                    IPAddress adr = IPAddress.Parse(this.txtHostIP.Text);
                    Array.Copy(adr.GetAddressBytes(), 0, pkt.data, 0, 4);  //新的接收服务器IP
                }
                int port = int.Parse(this.txtPortShort.Text);
                pkt.data[4] = (byte)(port & 0xff);         //新的port
                pkt.data[5] = (byte)((port >> 8) & 0xff);

                pkt.data[6] = (byte)(this.nudCycle.Value); // 新的cycle

                pkt.data[9] = (byte)(chkUploadInputStatusChange.Checked ? 1 : 0);  //2018-01-31 12:30:29 状态变化立即上传
                ret = pkt.run(strPCAddr);
                if (ret == 1)
                {
                    //修改OK
                }
                else
                {
                    MessageBox.Show("修改接收服务器失败...");
                    return;
                }
            }




            //修改IP
            pkt.Reset();
            pkt.functionID = 0x96;
            IPAddress adrA = IPAddress.Parse(this.txtf_IP.Text);
            Array.Copy(adrA.GetAddressBytes(), 0, pkt.data, 0, 4);  //新IP
            adrA = IPAddress.Parse(this.txtf_mask.Text);
            Array.Copy(adrA.GetAddressBytes(), 0, pkt.data, 4, 4);  //新掩码
            if (!string.IsNullOrEmpty(strGateway))
            {
                adrA = IPAddress.Parse(this.txtf_gateway.Text);
                Array.Copy(adrA.GetAddressBytes(), 0, pkt.data, 8, 4);  //新网关
            }
            if (this.optDhcp.Checked)
            {
                //选择动态获取IP DHCP 则IP设置为00
                pkt.data[0] = 0x0;
                pkt.data[1] = 0x0;
                pkt.data[2] = 0x0;
                pkt.data[3] = 0x0;
            }
            pkt.data[12] = 0x55;
            pkt.data[13] = 0xaa;
            pkt.data[14] = 0xaa;
            pkt.data[15] = 0x55;
            ret = pkt.run(strPCAddr);
            //修改IP会导致控制器设备重启, 所以不会有数据返回, ret的值不为1..
            //IP是否修改成功, 要搜索控制器再次确认
            

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 4字节转成整型数(低位前, 高位后)
        /// </summary>
        /// <param name="buff">字节数组</param>
        /// <param name="start">起始索引位(从0开始计)</param>
        /// <param name="len">长度</param>
        /// <returns>整型数</returns>
        long byteToLong(byte[] buff, int start, int len)
        {
            long val = 0;
            for (int i = 0; i < len && i < 4; i++)
            {
                long lng = buff[i + start];
                val += (lng << (8 * i));
            }
            return val;
        }

        string dataServerShortIP = "";
        int dataServerShortPort = 61005; // int.Parse(this.txtPort.Text);
        int dataServerShortCycle = 4;  //2015-06-14 08:23:17 引入通信周期发送
        int[] dataServerShortOption = new int[8]; //2017-09-08 12:43:53 8个选项 特殊用

        void getdeviceNetInfo()
        {
            int ret = 0;
            //创建短报文 pkt

            WGPacketShort pkt = new WGPacketShort();
            pkt.iDevSn = long.Parse(strSN); // controllerSN;
            //pkt.IP = strIP; // ControllerIP;

            //	读取接收服务器的IP和端口 **********************************************************************************
            pkt.Reset();
            pkt.functionID = 0x92;
            ret = pkt.run(strPCAddr);
            if (ret == 1)
            {
                dataServerShortIP = string.Format("{0}.{1}.{2}.{3}", pkt.recv[8], pkt.recv[9], pkt.recv[10], pkt.recv[11]);
                dataServerShortPort = pkt.recv[12] + (pkt.recv[13] << 8);
                dataServerShortCycle = pkt.recv[14];
                for (int i = 0; i < 8; i++)
                {
                    dataServerShortOption[i] = pkt.recv[15 + i];
                }

                this.txtHostIP.Text = dataServerShortIP;
                this.txtPortShort.Text = dataServerShortPort.ToString();
                this.nudCycle.Value = dataServerShortCycle;
                chkUploadInputStatusChange.Checked = (pkt.recv[17]>0);  //2018-01-31 12:30:29 状态变化立即上传


            }
            if (ret == 1)
            {
                pkt.Reset();
                pkt.functionID = 0xF4;
                pkt.data[0] = 0x55;
                pkt.data[1] = 0xaa;
                pkt.data[2] = 0xaa;
                pkt.data[3] = 0x55;
                pkt.data[4] = 0x92; pkt.data[5] = 0x00; pkt.data[6] = 0x00;
                ret = pkt.run(strPCAddr);
            }
            if (ret == 1)
            {

                if (pkt.recv[14] == 0xA5)
                {
                    this.optDhcp.Checked = true;
                }
                else
                {
                    this.optSetIP.Checked = true;
                }

            }
            this.grpIP.Enabled = this.optSetIP.Checked; //2017-09-08 13:18:15
        }
        private void dfrmTCPIPConfigure_Load(object sender, EventArgs e)
        {
            this.txtf_ControllerSN.Text = strSN;
            this.txtf_MACAddr.Text = strMac;
            this.txtf_IP.Text = strIP;
            this.txtf_mask.Text = strMask;
            this.txtf_gateway.Text = strGateway;

            if (this.txtf_IP.Text == "255.255.255.255")  //当系统为FF时, 改为默认值
            {
                this.txtf_IP.Text = "192.168.0.0";
            }
            if (this.txtf_mask.Text == "255.255.255.255")  //当系统为FF时, 改为默认值
            {
                this.txtf_mask.Text = "255.255.255.0";
            }
            if (this.txtf_gateway.Text == "255.255.255.255")  //当系统为FF时, 改为默认值
            {
                this.txtf_gateway.Text = "";
            }
            if (this.txtf_gateway.Text == "0.0.0.0")
            {
                this.txtf_gateway.Text = "";
            }

            //获取控制器的其他信息
            getdeviceNetInfo();
        }

        private void optSetIP_CheckedChanged(object sender, EventArgs e)
        {
            this.grpIP.Enabled = this.optSetIP.Checked; //2017-09-08 13:18:15
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //推荐配置...
            this.optDhcp.Checked = true;
            this.chkEditDateServer.Checked = true;
            //2018-02-13 07:27:16  this.txtf_IP.Text = strPCAddr;
            this.txtHostIP.Text = strPCAddr;  //2018-02-13 07:27:22             
            this.txtPortShort.Text = "61005";
            this.nudCycle.Value = 4;
           chkUploadInputStatusChange.Checked=false;  //2018-01-31 12:30:29 状态变化立即上传
        }
    }
}
