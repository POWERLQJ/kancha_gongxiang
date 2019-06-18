using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WG3000_COMM.Core;
using WGController32_CSharp.OtherConfig;

namespace WGController32_CSharp
{
    public partial class dfrmNetControllerConfig : Form
    {
        public dfrmNetControllerConfig()
        {
            InitializeComponent();
        }

        private void dfrmNetControllerConfig_Load(object sender, EventArgs e)
        {
            this.btnConfigure.Enabled = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblCount.Text = "0";
            this.Cursor = Cursors.WaitCursor; 
            this.dgvFoundControllers.Rows.Clear();
            this.btnConfigure.Enabled = false;

            System.Collections.ArrayList arrControllers = new System.Collections.ArrayList();

            using (WG3000_COMM.Core.wgMjController controllers = new WG3000_COMM.Core.wgMjController())
            {

                controllers.SearchControlers(ref arrControllers);
            }
            if (arrControllers != null)
            {
                if (arrControllers.Count <= 0)
                {
                    MessageBox.Show("Not Found 没有搜索到控制器");
                    this.btnConfigure.Enabled = true;
                    this.Cursor = Cursors.Default;  //2018-03-06 13:36:40
                    return;
                }
                this.dgvFoundControllers.Rows.Clear();
                //wgMjControllerConfigure conf;
                for (int i = 0; i < arrControllers.Count; i++)
                {
                 //   conf = (wgMjControllerConfigure)arrControllers[i];
                 string[] conf =    arrControllers[i].ToString().Split(',');
                    string[] subItems = new string[] {
                         (this.dgvFoundControllers.Rows.Count+1).ToString().PadLeft(4,'0'),  //
                conf[0], //        conf.controllerSN.ToString(),                   //SN
                conf[1], //         conf.ip.ToString(),         //IP
                conf[2], //         conf.mask.ToString(),       //"MASK",
                conf[3], //         conf.gateway.ToString(),    //"Gateway",
               //  conf[0], //        conf.port.ToString(),       //"PORT" 
                 conf[4], //        conf.MACAddr,               //MAC
                 conf[5], //        conf.pcIPAddr               //Note [pcIPAddr]
                    };
                    this.dgvFoundControllers.Rows.Add(subItems);
                }
            }
            this.btnSearch.Enabled = true;
            if (this.dgvFoundControllers.Rows.Count > 0)
            {
                this.btnConfigure.Enabled = true;
            }
            lblCount.Text = this.dgvFoundControllers.Rows.Count.ToString();
            this.Cursor = Cursors.Default; 

        } //Search .NET Device

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            if (this.dgvFoundControllers.SelectedRows.Count <= 0)
            {
                return;
            }
            using (dfrmTCPIPConfigure frm = new dfrmTCPIPConfigure())
            {
                DataGridViewRow dgvdr = this.dgvFoundControllers.SelectedRows[0];

                frm.strSN = dgvdr.Cells["f_ControllerSN"].Value.ToString();
                frm.strMac = dgvdr.Cells["f_MACAddr"].Value.ToString();
                frm.strIP = dgvdr.Cells["f_IP"].Value.ToString();
                frm.strMask = dgvdr.Cells["f_Mask"].Value.ToString();
                frm.strGateway = dgvdr.Cells["f_Gateway"].Value.ToString();
                //frm.strTCPPort = dgvdr.Cells["f_PORT"].Value.ToString();
                string pcIPAddr = "";
                if (dgvdr.Cells["f_PCIPAddr"].Value != null)
                {
                    pcIPAddr = dgvdr.Cells["f_PCIPAddr"].Value.ToString();
                }
                frm.strPCAddr = pcIPAddr;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    MessageBox.Show("请重新搜索...");
                    //this.btnSearch.PerformClick(); //2017-09-08 17:19:26 重新搜索
                    //string strSN = frm.strSN;
                    //string strMac = frm.strMac;
                    //string strIP = frm.strIP;
                    //string strMask = frm.strMask;
                    //string strGateway = frm.strGateway;
                    //string strOperate = frm.Text;
                    //this.Refresh();

                    //Cursor.Current = Cursors.WaitCursor;
                    ////using (wgMjController control = new wgMjController())
                    ////{
                    ////    control.NetIPConfigure(strSN, strMac, strIP, strMask, strGateway, strTCPPort, pcIPAddr);
                    ////}
                }
            }
        }

          private void dgvFoundControllers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.btnConfigure.Enabled)
            {
                this.btnConfigure.PerformClick();
            }
        }

          Boolean bStart = true;
          private void timer1_Tick(object sender, EventArgs e)
          {
              lblDateTime.Text = DateTime.Now.ToString( "yyyy-MM-dd HH:mm:ss dddd"); //年-月-日 时:分:秒 星期一)
              lblDateTime.Refresh(); //2018-04-20 17:05:41 
          //    this.timer1.Enabled = false;
              if (bStart)  //2018-04-20 17:03:29启动时
              {
                  bStart = false;
                  this.btnSearch.PerformClick();
              }
          }

          private void btnQRSet_Click(object sender, EventArgs e)
          {
              if (this.dgvFoundControllers.SelectedRows.Count <= 0)
              {
                  return;
              }
              using (dfrmQRSet frm = new dfrmQRSet())
              {
                  DataGridViewRow dgvdr = this.dgvFoundControllers.SelectedRows[0];

                  frm.strSN = dgvdr.Cells["f_ControllerSN"].Value.ToString();
                  frm.strIP = dgvdr.Cells["f_IP"].Value.ToString();
                  string pcIPAddr = "";
                  if (dgvdr.Cells["f_PCIPAddr"].Value != null)
                  {
                      pcIPAddr = dgvdr.Cells["f_PCIPAddr"].Value.ToString();
                  }

                  frm.ShowDialog(this);
              }
          }

          private void btnOpenDoor_Click(object sender, EventArgs e)  //2018-03-20 18:09:24 增加开门指令...
          {
              if (this.dgvFoundControllers.SelectedRows.Count <= 0)
              {
                  return;
              }

              int ret = 0;
              int success = 0;  //0 失败, 1表示成功
              DataGridViewRow dgvdr = this.dgvFoundControllers.SelectedRows[0];
              string controllerSN = dgvdr.Cells["f_ControllerSN"].Value.ToString();
              string ControllerIP = dgvdr.Cells["f_IP"].Value.ToString();

              //创建短报文 pkt
              WGPacketShort pkt = new WGPacketShort();
              pkt.iDevSn = long.Parse(controllerSN);
              pkt.IP = ControllerIP;

              //1.10	远程开门[功能号: 0x40] **********************************************************************************
              int doorNO = 1;
              pkt.Reset();
              pkt.functionID = 0x40;
              pkt.data[0] = (byte)(doorNO & 0xff); //2013-11-03 20:56:33
              ret = pkt.run();
              success = 0;
              if (ret > 0)
              {
                  if (pkt.recv[8] == 1)
                  {
                      //有效开门.....
                      // 
                      MessageBox.Show("远程开门	 成功...");
                      success = 1;
                  }
              }

              //结束  **********************************************************************************
              pkt.close();  //关闭通信

          }

          //将字符串存储到byte数组中 [str要转换的字符串, bytArr用于存储byte的数组, len要创建的长度
          static int strToByteArr(string str, ref byte[] bytArr, int len)  //2015-12-13 09:25:22
          {
              int ret = -1;
              try
              {
                  int minLen;
                  minLen = len;
                  if (minLen < ((str.Length + 1) >> 1))
                  {
                      minLen = (str.Length + 1) >> 1;
                  }
                  if (minLen < bytArr.Length)
                  {
                      minLen = bytArr.Length;
                  }
                  if (minLen > 0)
                  {
                      for (int i = 0; i < minLen; i++)
                      {
                          bytArr[i] = byte.Parse(str.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                      }
                      ret = 0;
                  }
              }
              catch (Exception ex)
              {
                  
              }
              return ret;
          }
          private void btnSendCommand_Click(object sender, EventArgs e)  //2018-04-20 15:28:04 发送指令
          {
              if (this.dgvFoundControllers.SelectedRows.Count <= 0)
              {
                  return;
              }
              string strCommand = this.txt64Command.Text;
              byte[] bytArr = new byte[64];
             // strToByteArr(filedetail[1], ref arr, arr.Length);
              if (strCommand.Split(' ').Length == (64 - 1))
              {
                  string[] str = strCommand.Split(' ');
                  for (int i = 0; i < 64; i++)
                  {
                      bytArr[i] = byte.Parse(str[i], System.Globalization.NumberStyles.AllowHexSpecifier);
                  }
              }
              else
              {
                  if (strCommand.Replace(" ", "").Length == (64 * 2))
                  {
                      strToByteArr(strCommand.Replace(" ", ""), ref bytArr, bytArr.Length);
                  }
                  else
                  {
                      MessageBox.Show("输入的指令无效");
                      return;
                  }
              }
              int ret = 0;
              int success = 0;  //0 失败, 1表示成功
              DataGridViewRow dgvdr = this.dgvFoundControllers.SelectedRows[0];
              string controllerSN = dgvdr.Cells["f_ControllerSN"].Value.ToString();
              string ControllerIP = dgvdr.Cells["f_IP"].Value.ToString();

              //创建短报文 pkt
              WGPacketShort pkt = new WGPacketShort();
              pkt.iDevSn = long.Parse(controllerSN);
              pkt.IP = ControllerIP;

              // **********************************************************************************
              int doorNO = 1;
              pkt.Reset();
              pkt.functionID = bytArr[1]; 
              pkt.data[0] = (byte)(doorNO & 0xff); //2013-11-03 20:56:33
              for (int i = 0; i < 56; i++)
              {
                  pkt.data[i] = bytArr[8 + i];
              }
              this.txt64CommandSent.Text =System.BitConverter.ToString( pkt.getCommandByte()).Replace("-", " ");
              this.txtInfo.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " +  "指令发出" + "\r\n"); //2014-03-23 12:15:15开始时间

              ret = pkt.run();
              success = 0;
              if (ret > 0)
              {
                  //if (pkt.recv[8] == 1)
                  //{
                  //    //有效开门.....
                  //    // 
                  //    MessageBox.Show("远程开门	 成功...");
                  //    success = 1;
                  //}
                  this.txt64CommandRecved.Text = System.BitConverter.ToString(pkt.recv).Replace("-", " ");
                  this.txtInfo.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + "通信成功" + "\r\n"); //2014-03-23 12:15:15开始时间
              }
              else
              {
                  this.txt64CommandRecved.Text = "通信失败???";
                  this.txtInfo.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + "通信失败???" + "\r\n"); //2014-03-23 12:15:15开始时间
              }
              //结束  **********************************************************************************
              pkt.close();  //关闭通信

          }
    }
}
