using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Diagnostics;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        delegate void dlgLog(string info);
        void logEntry(string info)  //日志信息
        {
            txtInfo.AppendText(string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), info)); //2015-11-03 20:55:49 显示时间
            txtInfo.ScrollToCaret();//滚动到光标处
            Application.DoEvents();
        }
        void log(string info)  //日志信息
        {
            //txtInfo.AppendText(string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), info)); //2015-11-03 20:55:49 显示时间
            //txtInfo.ScrollToCaret();//滚动到光标处
            //Application.DoEvents();
            //this.Invoke(new dlgLog(logEntry), new object[] { string.Format("{0} {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), info) });
            this.Invoke(new dlgLog(logEntry), new object[] { info });

        }


        TcpClient tcp;
        string encodingOfTCP = "GB2312";
        string cmdBase = string.Format("N3000 -USER \"{0}\" -PASSWORD \"{1}\" ", "abc", "123");  //2018-01-06 12:51:40 命令基本信息
        private void button37_Click(object sender, EventArgs e)
        {
            string strCommand =cmdBase + string.Format(" -Open \"{0}\"", this.txtDoorName.Text);

          string info =  sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
          if (!string.IsNullOrEmpty(info))
          {
              log(info);
          }
            
        }

        string  sendCommand(string strCommand)
        {
            byte[] arrayCommand = System.Text.Encoding.GetEncoding(encodingOfTCP).GetBytes(strCommand);
            string strRet = "";
            System.Net.Sockets.NetworkStream networkStream;
            networkStream = tcp.GetStream();

            //先清缓存
            while (networkStream.CanRead)
            {
                if (networkStream.DataAvailable)
                {
                    byte[] buffA = new byte[2000];
                    networkStream.Read(buffA, 0, buffA.Length);
                }
                else
                {
                    break;
                }
            }

            if (networkStream.CanWrite)
            {
                networkStream.Write(arrayCommand, 0, arrayCommand.Length); //   '发出指令
            }
            DateTime dt = DateTime.Now.AddSeconds(2);
            byte[] bytes = new byte[2000];
            int rcvingLen = 0;
            while (dt > DateTime.Now)
            {
                if (networkStream.CanRead)
                {
                    if (networkStream.DataAvailable)
                    {
                        Debug.WriteLine(DateTime.Now.ToString() + "*----0");
                        rcvingLen = networkStream.Read(bytes, 0, bytes.Length); //   ' CInt(tcpClient.ReceiveBufferSize))'接收返回的数据
                        string dataStr = System.Text.Encoding.GetEncoding(encodingOfTCP).GetString(bytes);
                        //log(dataStr);
                        strRet = dataStr;
                        Debug.WriteLine(DateTime.Now.ToString() + "#-------1" + "      ");
                        break;
                    }
                }
                // System.Threading.Thread.Sleep(100); //
            }
            return strRet;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string strIP = this.txtServerIP.Text;
            int PORT =(int) nudServerPort.Value;

            if (tcp == null)
            {
                try
                {
                    tcp = new System.Net.Sockets.TcpClient();
                    //2012-10-12_22:17:08 tcp.Connect(System.Net.IPAddress.Parse(strIP), wgTools.COMM_UDP_PORT);// 60000);
                    tcp.Connect(System.Net.IPAddress.Parse(strIP), PORT); //2012-10-12_22:17:20 为V5.30驱动改变 wgTools.COMM_UDP_PORT);// 60000);
                    this.btnDisconnect.Enabled = true;
                    this.btnConnect.Enabled = false;
                    this.groupBox1.Enabled = true;
                }
                catch (Exception ex)
                {
                   log(ex.ToString());
                    //throw;
                    tcp = null;
                }
            }

            //if (tcp == null) //2014-01-04 23:01:51 
            //{
            //    return -1;
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.btnDisconnect.Enabled = false;
            this.btnConnect.Enabled = true;
            this.groupBox1.Enabled = false;

            if (tcp != null)
            {
                try
                {
                    tcp.Close();
                    tcp = null;
                }
                catch (Exception ex)
                {
                    log(ex.ToString());
                    //throw;
                    tcp = null;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strCommand = cmdBase + string.Format(" -GetAllDoorStatus ");

            string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
            if (!string.IsNullOrEmpty(info))
            {
                log(info);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strCommand = cmdBase + string.Format(" -GetDoorStatus \"{0}\"", this.txtDoorName.Text);

            string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
            if (!string.IsNullOrEmpty(info))
            {
                log(info);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
 string strCommand = "";
 if (cboControlMode.SelectedIndex == 1)
 {
     strCommand = cmdBase + string.Format(" -SETDOORNO \"{0}\"", this.txtDoorName.Text);
 }
 else if (cboControlMode.SelectedIndex == 2)
 {
     strCommand = cmdBase + string.Format(" -SETDOORNC \"{0}\"", this.txtDoorName.Text);
 }
 else 
 {
     strCommand = cmdBase + string.Format(" -SETDOORONLINE \"{0}\"", this.txtDoorName.Text);
 }
 string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
            if (!string.IsNullOrEmpty(info))
            {
                log(info);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strCommand = cmdBase + string.Format(" -GETSN \"{0}\"", this.txtDoorName.Text);

            string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
            if (!string.IsNullOrEmpty(info))
            {
                log(info);
                if (info.IndexOf("iRet=0") > 0)
                {
                    log("门名称不存在.  请检查输入是否正确..");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string strCommand = cmdBase + string.Format(" -SetTime \"{0}\"", this.txtDoorName.Text);

            string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
            if (!string.IsNullOrEmpty(info))
            {
                log(info);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string strCommand = cmdBase + string.Format(" -GETDOORCONTROL \"{0}\"", this.txtDoorName.Text);

            string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
            if (!string.IsNullOrEmpty(info))
            {
                log(info);
                log("返回值 (-1失败, 其他值 控制方式(3=在线, 2=常闭, 1=常开)");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox1.Text))
            {
            }
            else
            {
                string strCommand = cmdBase + string.Format("  {0}", this.textBox1.Text);

                string info = sendCommand(strCommand); //2018-01-06 12:54:23 发送指令
                if (!string.IsNullOrEmpty(info))
                {
                    log(info);
                }
            }
        }




    }
}
