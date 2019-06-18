using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace WGController32_CSharp
{
    public partial class FormCloudServer : Form
    {
        public FormCloudServer()
        {
            InitializeComponent();
        }
        private void FormCloudServer_Load(object sender, EventArgs e)
        {
            cboFloors.Items.Clear();
            for (int i = 0; i < 40; i++)
            {
                cboFloors.Items.Add(string.Format("{0}楼", i + 1));
            }
            cboFloors.SelectedIndex = 0;

            string hostName = System.Net.Dns.GetHostName();

            Boolean bFound = false;
            foreach (System.Net.IPAddress ipaddr in System.Net.Dns.GetHostEntry(hostName).AddressList) //获取主机的IP地址列表 获取主机的IP地址
            {
                if (ipaddr.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork) //2011-12-29_18:53:13 只允许 IPV4通过
                {
                    continue;
                }
                if (ipaddr.IsIPv6LinkLocal)
                {
                    continue; //
                }
                if (ipaddr.ToString() == "127.0.0.1")
                {
                    continue; //
                }
                if (bFound)
                {
                    MessageBox.Show("电脑存在多个IP, 建议前期开发时只使用一个IP操作.  [假如无线与网线口同时在用时, 请关键无线口]");
                    break;
                }

                bFound = true;
                txtWatchServerIP.Text = ipaddr.ToString();
            }
            if (!bFound)
            {
                MessageBox.Show("网络不通! 请接好网线..");
            }

        }

        Boolean bStopWatchServer = true; //2015-05-05 17:35:07 停止接收服务器
        private void button4_Click(object sender, EventArgs e)
        {
            if (bStopWatchServer)
            {
                bStopWatchServer = false;
                this.button4.BackColor = Color.Yellow;
                this.groupBox1.Enabled = true;
                WatchingServerRuning(txtWatchServerIP.Text, int.Parse(this.txtWatchServerPort.Text)); //服务器运行....
                this.button4.BackColor = Color.Transparent; //2017-09-09 14:57:15
                bStopWatchServer = true;
            }
        }

        private void FormCloudServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            bStopWatchServer = true;
        }

        delegate void dlgLog(string info);
        void logEntry(string info)  //日志信息
        {
            if (txtInfo.Text.Length > 32000)  //2018-04-25 10:34:41 日志过长了
            {
                int nextrow = txtInfo.Text.IndexOf("\r\n", 16000);
                txtInfo.Text = txtInfo.Text.Substring(nextrow+2); //
            }
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
       void logDetail(string info)  //详细日志信息
        {
           if (bDisplayDetail)
                            {
            this.Invoke(new dlgLog(logEntry), new object[] { info });
                             }
         
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
        /// <summary>
        /// 获取Hex值, 主要用于日期时间格式
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns>Hex值</returns>
        int GetHex(int val)
        {
            return ((val % 10) + (((val - (val % 10)) / 10) % 10) * 16);
        }

        delegate void dlgAddSwipeItems(ListViewItem info);
        void addSwipeItems(ListViewItem info)  //记录信息
        {
            if (lstSwipe.Items.Count > 32000) //2018-04-25 10:35:47 记录过多时 删除部分
            {
                for (int i=0; i<16000;i++)
                {
                    lstSwipe.Items.RemoveAt(16000 - 1 - i);
                }
            }
            lstSwipe.Items.Add(info);
        }

        /// <summary>
        /// 显示记录信息
        /// </summary>
        /// <param name="recv"></param>
        void displayRecordInformation(byte[] recv)
        {
            //8-11	记录的索引号
            //(=0表示没有记录)	4	0x00000000
            int recordIndex = 0;
            recordIndex = (int)byteToLong(recv, 8, 4);

            //12	记录类型**********************************************
            //0=无记录
            //1=刷卡记录
            //2=门磁,按钮, 设备启动, 远程开门记录
            //3=报警记录	1	
            //0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值
            int recordType = recv[12];

            //13	有效性(0 表示不通过, 1表示通过)	1	
            int recordValid = recv[13];

            //14	门号(1,2,3,4)	1	
            int recordDoorNO = recv[14];

            //15	进门/出门(1表示进门, 2表示出门)	1	0x01
            int recordInOrOut = recv[15];

            //16-19	卡号(类型是刷卡记录时)
            //或编号(其他类型记录)	4	
            long recordCardNO = 0;
            recordCardNO = byteToLong(recv, 16, 4);
            long recordCardNOHigh = 0;
            recordCardNOHigh = byteToLong(recv, 44, 4);              //2017-10-30 16:52:38 新增
            recordCardNO = recordCardNO + (recordCardNOHigh << 32);  //2017-10-30 16:52:29 新增

            //20-26	刷卡时间:
            //年月日时分秒 (采用BCD码)见设置时间部分的说明
            string recordTime = "2000-01-01 00:00:00";
            recordTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                recv[20], recv[21], recv[22], recv[23], recv[24], recv[25], recv[26]);
            //2012.12.11 10:49:59	7	
            //27	记录原因代码(可以查 “刷卡记录说明.xls”文件的ReasonNO)
            //处理复杂信息才用	1	
            int reason = recv[27];


            //0=无记录
            //1=刷卡记录
            //2=门磁,按钮, 设备启动, 远程开门记录
            //3=报警记录	1	
            //0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值
            if (recordType == 0)
            {
                log(string.Format("索引位={0}  无记录", recordIndex));
            }
            else if (recordType == 0xff)
            {
                log(" 指定索引位的记录已被覆盖掉了,请使用索引0, 取回最早一条记录的索引值");
            }
            else if (recordType == 1) //2015-06-10 08:49:31 显示记录类型为卡号的数据
            {
                //卡号
                log(string.Format("索引位={0}  ", recordIndex));
                log(string.Format("  卡号 = {0}", recordCardNO));
                log(string.Format("  门号 = {0}", recordDoorNO));
                log(string.Format("  进出 = {0}", recordInOrOut == 1 ? "进门" : "出门"));
                log(string.Format("  有效 = {0}", recordValid == 1 ? "通过" : "禁止"));
                log(string.Format("  时间 = {0}", recordTime));
                log(string.Format("  描述 = {0}", getReasonDetailChinese(reason)));

                //2017-12-13 09:49:57 显示在列表中
                long sn;
                sn = byteToLong(recv, 4, 4);
                ListViewItem itmA = new ListViewItem(new string[] { recordCardNO.ToString(), recordTime,sn.ToString(),recordDoorNO.ToString() });
                //lstSwipe.Items.Add(itmA);
                this.Invoke(new dlgAddSwipeItems(this.addSwipeItems), new object[] { itmA }); 
                
            }
            else if (recordType == 2)
            {
                //其他处理
                //门磁,按钮, 设备启动, 远程开门记录
                log(string.Format("索引位={0}  非刷卡记录", recordIndex));
                if ((reason == 44) && (recordCardNO > 1)) //2017-09-07 12:08:34 远程开门
                {
                    log(string.Format("  模拟卡号 = {0}", recordCardNO)); //2017-09-07 12:09:41
                }
                else
                {
                    log(string.Format("  编号 = {0}", recordCardNO));
                }
                log(string.Format("  门号 = {0}", recordDoorNO));
                log(string.Format("  时间 = {0}", recordTime));
                log(string.Format("  描述 = {0}", getReasonDetailChinese(reason)));
            }
            else if (recordType == 3)
            {
                //其他处理
                //报警记录
                log(string.Format("索引位={0}  报警记录", recordIndex));
                log(string.Format("  编号 = {0}", recordCardNO));
                log(string.Format("  门号 = {0}", recordDoorNO));
                log(string.Format("  时间 = {0}", recordTime));
                log(string.Format("  描述 = {0}", getReasonDetailChinese(reason)));
            }
        }

        string[] RecordDetails =
        {
//记录原因 (类型中 SwipePass 表示通过; SwipeNOPass表示禁止通过; ValidEvent 有效事件(如按钮 门磁 超级密码开门); Warn 报警事件)
//代码  类型   英文描述  中文描述
"1","SwipePass","Swipe","刷卡开门",
"2","SwipePass","Swipe Close","刷卡关",
"3","SwipePass","Swipe Open","刷卡开",
"4","SwipePass","Swipe Limited Times","刷卡开门(带限次)",
"5","SwipeNOPass","Denied Access: PC Control","刷卡禁止通过: 电脑控制",
"6","SwipeNOPass","Denied Access: No PRIVILEGE","刷卡禁止通过: 没有权限",
"7","SwipeNOPass","Denied Access: Wrong PASSWORD","刷卡禁止通过: 密码不对",
"8","SwipeNOPass","Denied Access: AntiBack","刷卡禁止通过: 反潜回",
"9","SwipeNOPass","Denied Access: More Cards","刷卡禁止通过: 多卡",
"10","SwipeNOPass","Denied Access: First Card Open","刷卡禁止通过: 首卡",
"11","SwipeNOPass","Denied Access: Door Set NC","刷卡禁止通过: 门为常闭",
"12","SwipeNOPass","Denied Access: InterLock","刷卡禁止通过: 互锁",
"13","SwipeNOPass","Denied Access: Limited Times","刷卡禁止通过: 受刷卡次数限制",
"14","SwipeNOPass","Denied Access: Limited Person Indoor","刷卡禁止通过: 门内人数限制",
"15","SwipeNOPass","Denied Access: Invalid Timezone","刷卡禁止通过: 卡过期或不在有效时段",
"16","SwipeNOPass","Denied Access: In Order","刷卡禁止通过: 按顺序进出限制",
"17","SwipeNOPass","Denied Access: SWIPE GAP LIMIT","刷卡禁止通过: 刷卡间隔约束",
"18","SwipeNOPass","Denied Access","刷卡禁止通过: 原因不明",
"19","SwipeNOPass","Denied Access: Limited Times","刷卡禁止通过: 刷卡次数限制",
"20","ValidEvent","Push Button","按钮开门",
"21","ValidEvent","Push Button Open","按钮开",
"22","ValidEvent","Push Button Close","按钮关",
"23","ValidEvent","Door Open","门打开[门磁信号]",
"24","ValidEvent","Door Closed","门关闭[门磁信号]",
"25","ValidEvent","Super Password Open Door","超级密码开门",
"26","ValidEvent","Super Password Open","超级密码开",
"27","ValidEvent","Super Password Close","超级密码关",
"28","Warn","Controller Power On","控制器上电",
"29","Warn","Controller Reset","控制器复位",
"30","Warn","Push Button Invalid: Disable","按钮不开门: 按钮禁用",
"31","Warn","Push Button Invalid: Forced Lock","按钮不开门: 强制关门",
"32","Warn","Push Button Invalid: Not On Line","按钮不开门: 门不在线",
"33","Warn","Push Button Invalid: InterLock","按钮不开门: 互锁",
"34","Warn","Threat","胁迫报警",
"35","Warn","Threat Open","胁迫报警开",
"36","Warn","Threat Close","胁迫报警关",
"37","Warn","Open too long","门长时间未关报警[合法开门后]",
"38","Warn","Forced Open","强行闯入报警",
"39","Warn","Fire","火警",
"40","Warn","Forced Close","强制关门",
"41","Warn","Guard Against Theft","防盗报警",
"42","Warn","7*24Hour Zone","烟雾煤气温度报警",
"43","Warn","Emergency Call","紧急呼救报警",
"44","RemoteOpen","Remote Open Door","操作员远程开门",
"45","RemoteOpen","Remote Open Door By USB Reader","发卡器确定发出的远程开门"
        };

        string getReasonDetailChinese(int Reason) //中文
        {
            if (Reason > 45)
            {
                return "";
            }
            if (Reason <= 0)
            {
                return "";
            }
            return RecordDetails[(Reason - 1) * 4 + 3]; //中文信息
        }

        string getReasonDetailEnglish(int Reason) //英文描述
        {
            if (Reason > 45)
            {
                return "";
            }
            if (Reason <= 0)
            {
                return "";
            }
            return RecordDetails[(Reason - 1) * 4 + 2]; //英文信息
        }

        ArrayList arrRecordIndex = new ArrayList(); //2018-05-11 08:04:53 放全局 2017-09-07 11:18:40 采用数组记录
        ArrayList arrControllerSN = new ArrayList(); //2018-05-11 08:04:53 放全局 2017-09-07 11:18:40 采用数组记录
        /// <summary>
        /// 打开接收服务器接收数据 (注意防火墙 要允许此端口的所有包进入才行)
        /// </summary>
        /// <param name="watchServerIP">接收服务器IP(一般是当前电脑IP)</param>
        /// <param name="watchServerPort">接收服务器端口</param>
        /// <returns>1 表示成功,否则失败</returns>
        int WatchingServerRuning(string watchServerIP, int watchServerPort)
        {
            //注意防火墙 要允许此端口的所有包进入才行
            try
            {
                WG3000_COMM.Core.wgUdpServerCom udpserver = new WG3000_COMM.Core.wgUdpServerCom(watchServerIP, watchServerPort);
                //2017-09-07 16:42:33 不显示IP                 udpserver.IncludeIPInfo = true; //2016-01-05 12:51:55 获取IP
                udpserver.IncludeIPInfo = this.chkDisplayIP.Checked; //2018-05-23 11:20:58 显示IP

                if (!udpserver.IsWatching())
                {
                    log("进入接收服务器监控状态....失败");
                    return -1;
                }
                log("进入接收服务器监控状态....");
                long recordIndex = 0;  //已获取到的记录索引位
                //2018-05-11 08:04:39 ArrayList arrRecordIndex = new ArrayList(); //2017-09-07 11:18:40 采用数组记录
                //2018-05-11 08:04:43 ArrayList arrControllerSN = new ArrayList(); //2017-09-07 11:18:40 采用数组记录


                int recv_cnt;
                while (!bStopWatchServer)
                {
                    recv_cnt = udpserver.receivedCount();
                    if (recv_cnt > 0)
                    {
                        byte[] buff = udpserver.getRecords();
                        if (buff[1] == 0x20)
                        {
                            long sn;
                            long recordIndexGet;
                            sn = byteToLong(buff, 4, 4);
                            if (WG3000_COMM.Common.wgControllerInfo.GetControllerType((int)sn) > 0)  //2018-03-21 12:00:12 有效的序列号
                            {
                                logDetail(string.Format("接收到来自控制器SN = {0} 的数据包..\r\n", sn));
                                udpserverLast = udpserver; //2017-12-12 16:54:21
                                //                          if (udpserver.IncludeIPInfo && buff.Length == 68) //2016-01-05 14:10:23 
                                if (udpserver.IncludeIPInfo && (buff.Length % 64) == 4) //2017-09-07 15:33:25 2016-01-05 14:10:23 
                                {
                                    //long ip = byteToLong(buff, 64, 4);
                                    //2017-09-07 15:33:48 log(string.Format("接收到来自控制器IP = {0:d}.{1:d}.{2:d}.{3:d} 的数据包..\r\n", buff[64], buff[65], buff[66], buff[67]));  //2016-01-05 14:10:29 获取IP
                                    logDetail(string.Format("接收到来自控制器IP = {0:d}.{1:d}.{2:d}.{3:d} 的数据包..\r\n",
                                        buff[buff.Length - 4], buff[buff.Length - 3], buff[buff.Length - 2], buff[buff.Length - 1]));  //2017-09-07 15:34:22 2016-01-05 14:10:29 获取IP
                                }
                                recordIndexGet = byteToLong(buff, 8, 4);

                                int iLoc = arrControllerSN.IndexOf(sn);
                                if (iLoc >= 0)
                                {
                                    recordIndex = (long)arrRecordIndex[iLoc];
                                    arrRecordIndex[iLoc] = recordIndexGet; //2017-09-07 11:23:50 保存新值
                                }
                                else
                                {
                                    recordIndex = 0;
                                    arrControllerSN.Add(sn);
                                    arrRecordIndex.Add(recordIndexGet);
                                    lstControllers.Items.Add(sn); //2017-12-12 17:53:53 控制器序列号
                                }

                                if ((recordIndex < recordIndexGet)  //2017-12-12 17:38:25 获取到记录索引 大于 已获取的记录索引号
                                  || ((recordIndexGet - recordIndex) < -5))  //新的记录索引 比 已获取的记录索引值小,  相差大于5条[则取新的](设备的记录索引号可能复位造成)
                                {
                                    recordIndex = recordIndexGet;

                                    displayRecordInformation(buff); //2015-06-09 20:01:21
                                    dealSwipeRecord(buff, ref udpserver); //2017-09-07 11:18:09 处理刷卡记录
                                }
                            }

                        }

                        //************************二维码
                        if (buff[1] == 0x22) //2017-09-07 15:38:29 增加二维码的数据
                        {
                            long sn;
                            long qrDataLen;
                            sn = byteToLong(buff, 4, 4);
                            if (WG3000_COMM.Common.wgControllerInfo.GetControllerType((int)sn) > 0)  //2018-03-21 12:00:12 有效的序列号
                            {
                                string allData = "";
                                for (int i = 0; i < buff.Length; i++)
                                {
                                   allData +=  string.Format("{0:X2} ",buff[i]);
                                }
 //2018-09-12 09:36:57                               logDetail(string.Format("接收到来自控制器SN = {0} 的二维码数据包..\r\n", sn));
                                logDetail(string.Format("接收到来自控制器SN = {0} 的二维码数据包: \r\n{1}\r\n", sn, allData));  //2018-09-12 09:37:40 显示数据信息
                                udpserverLast = udpserver; //2017-12-12 16:54:21

                                //                          if (udpserver.IncludeIPInfo && buff.Length == 68) //2016-01-05 14:10:23 
                                if (udpserver.IncludeIPInfo && (buff.Length % 64) == 4) //2017-09-07 15:33:25 2016-01-05 14:10:23 
                                {
                                    //long ip = byteToLong(buff, 64, 4);
                                    //2017-09-07 15:33:48 log(string.Format("接收到来自控制器IP = {0:d}.{1:d}.{2:d}.{3:d} 的数据包..\r\n", buff[64], buff[65], buff[66], buff[67]));  //2016-01-05 14:10:29 获取IP
                                    logDetail(string.Format("接收到来自控制器IP = {0:d}.{1:d}.{2:d}.{3:d} 的二维码数据包..\r\n",
                                        buff[buff.Length - 4], buff[buff.Length - 3], buff[buff.Length - 2], buff[buff.Length - 1]));  //2017-09-07 15:34:22 2016-01-05 14:10:29 获取IP
                                }
                                qrDataLen = byteToLong(buff, 8, 4);
                                dealQRData(buff, ref udpserver); //2017-09-07 11:18:09 处理二维码记录
                            }

                        }

                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                        Application.DoEvents();

                    }
                }
                udpserver.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
                // throw;
            }
            return 0;
        }

        //static long sequenceId4RemoteOpen = 0x40000000; //2017-09-07 11:04:33 用于远程开门的流水号
        void dealSwipeRecord(byte[] recv, ref WG3000_COMM.Core.wgUdpServerCom server)
        {
            //8-11	记录的索引号
            //(=0表示没有记录)	4	0x00000000
            int recordIndex = 0;
            recordIndex = (int)byteToLong(recv, 8, 4);

            //12	记录类型**********************************************
            //0=无记录
            //1=刷卡记录
            //2=门磁,按钮, 设备启动, 远程开门记录
            //3=报警记录	1	
            //0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值
            int recordType = recv[12];

            //13	有效性(0 表示不通过, 1表示通过)	1	
            int recordValid = recv[13];

            //14	门号(1,2,3,4)	1	
            int recordDoorNO = recv[14];

            //15	进门/出门(1表示进门, 2表示出门)	1	0x01
            int recordInOrOut = recv[15];

            //16-19	卡号(类型是刷卡记录时)
            //或编号(其他类型记录)	4	
            long recordCardNO = 0;
            recordCardNO = byteToLong(recv, 16, 4);
            long recordCardNOHigh = 0;
            recordCardNOHigh = byteToLong(recv, 44, 4);              //2017-10-30 16:52:38 新增
            recordCardNO = recordCardNO + (recordCardNOHigh << 32);  //2017-10-30 16:52:29 新增

            //20-26	刷卡时间:
            //年月日时分秒 (采用BCD码)见设置时间部分的说明
            string recordTime = "2000-01-01 00:00:00";
            recordTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                recv[20], recv[21], recv[22], recv[23], recv[24], recv[25], recv[26]);
            //2012.12.11 10:49:59	7	
            //27	记录原因代码(可以查 “刷卡记录说明.xls”文件的ReasonNO)
            //处理复杂信息才用	1	
            int reason = recv[27];


            //0=无记录
            //1=刷卡记录
            //2=门磁,按钮, 设备启动, 远程开门记录
            //3=报警记录	1	
            //0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值

            if (recordType == 1) //2015-06-10 08:49:31 显示记录类型为卡号的数据
            {
                //卡号
                //log(string.Format("索引位={0}  ", recordIndex));
                //log(string.Format("  卡号 = {0}", recordCardNO));
                //log(string.Format("  门号 = {0}", recordDoorNO));
                //log(string.Format("  进出 = {0}", recordInOrOut == 1 ? "进门" : "出门"));
                //log(string.Format("  有效 = {0}", recordValid == 1 ? "通过" : "禁止"));
                //log(string.Format("  时间 = {0}", recordTime));
                //log(string.Format("  描述 = {0}", getReasonDetailChinese(reason)));
                if (recordValid == 0)  //2017-09-07 10:56:00 禁止通过时
                    if (this.checkBox1.Checked) //2017-12-12 16:46:26 选中
                    {
                        //2017-09-07 10:56:10 检查卡号是否满足要求
                        long sn;
                        sn = byteToLong(recv, 4, 4);
                        remoteOpenDoor(ref server, sn, recordDoorNO, recordInOrOut, recordCardNO);
                    }
            }

        }

        void dealQRData(byte[] recv, ref WG3000_COMM.Core.wgUdpServerCom server) //2017-09-07 15:40:56 处理QR数据
        {
            //8-11	二维码数据长度
            //(=0表示没有记录)	4	0x00000000
            int qrDataLen = 0;
            qrDataLen = (int)byteToLong(recv, 8, 4);

            //12	不考虑[2017-09-07 15:47:32]
            //2017-09-07 15:47:36  int recordType = recv[12];

            //13	串口号(1或2) 
            int serialPort = recv[13];

            //14	门号(1,2,3,4)	1	
            int recordDoorNO = recv[14];

            //15	进门/出门(1表示进门, 2表示出门)	1	0x01
            int recordInOrOut = recv[15];

            //16-36	不考虑


            long cmdSequenceId = byteToLong(recv, 40, 4); //2017-09-07 15:56:06 流水号

            if (qrDataLen >= 1) //2017-09-07 15:49:29 有二维码数据
            {
                byte[] qrData = new Byte[qrDataLen];
                Array.Copy(recv, 64, qrData, 0, qrDataLen); //数据


                log(string.Format("流水号={0} 二维码原始数据:\r\n        {1}\r\n", cmdSequenceId, System.BitConverter.ToString(qrData)).Replace('-', ' '));

                //转换为字符串数据 
                log(string.Format("流水号={0} 二维码原始数据(转换为字符串):\r\n        {1}",
                    cmdSequenceId, System.Text.Encoding.GetEncoding("GB2312").GetString(qrData).Trim()));

                if (this.checkBox2.Checked) //2017-12-12 16:46:26 选中
                {
                    //2017-09-07 10:56:10 分析二维码数据
                    //...............
                    //...............
                    long recordCardNO = 0;
                    //                   recordCardNO = byteToLong(recv, 64, 4);  //2017-09-07 15:59:19 测试取QR数据的前8字节 可以修改
                    recordCardNO = cmdSequenceId; //2017-09-07 16:21:18 用流水号替换 也可根据实际需要替换为 用户的工号或卡号(必须是数字)

                    //再作如下远程开门处理
                    long sn;
                    sn = byteToLong(recv, 4, 4);
                    remoteOpenDoor(ref server , sn, recordDoorNO, recordInOrOut, recordCardNO);

                   
                }
            }

        }

        WG3000_COMM.Core.wgUdpServerCom udpserverLast;


        private void btnRemoteOpenDoor_Click(object sender, EventArgs e) //远程开门
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //直接处理最后SN数据
                long sn;
                sn = long.Parse(this.txtSN.Text);
                int doorNO=1;        //1号门
                if (this.cboDoors.SelectedIndex >= 0)
                {
                    doorNO = this.cboDoors.SelectedIndex + 1;
                    if (WG3000_COMM.Common.wgControllerInfo.GetControllerType((int)sn) < doorNO)
                    {
                        MessageBox.Show("此控制器不存在 选定的门号...");
                        return;
                    }
                }
                int inOrOut = 1;     //1表示进门, 2表示出门
                long cardNO = 99999; //模拟卡号
              int ret =  remoteOpenDoor(ref udpserverLast, sn, doorNO, inOrOut, cardNO);
            }
        }




        /// <summary>
        /// 远程开门操作
        /// </summary>
        /// <param name="server">通信服务器</param>
        /// <param name="sn">控制器序列号</param>
        /// <param name="doorNO">门号(1-4)</param>
        /// <param name="inOrOut">进门(1) 出门(2)</param>
        /// <param name="cardNO">模拟卡号</param>
        /// <returns>等于1表示成功 其他值表示失败</returns>
        int remoteOpenDoor(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn, int doorNO, int inOrOut, long cardNO)  //2017-12-23 09:25:43 远程开门操作
        {
            int ret = -13; //
            //2017-09-07 10:56:10 检查卡号是否满足要求
            byte[] buff = new byte[WGPacketShort.WGPacketSize];
            for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
            {
                buff[i] = 0;
            }
            buff[0] = (byte)0x17; //Type;
            buff[1] = (byte)0x40; //functionID;
            Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);

            //Array.Copy(data, 0, buff, 8, data.Length);
            buff[8 + 0] = (byte)(doorNO & 0xff); //门号
            buff[28] = (byte)(inOrOut == 1 ? 0 : 1); // recordInOrOut == 1 ? "进门" : "出门"));
            Array.Copy(System.BitConverter.GetBytes(cardNO), 0, buff, 20, 4); //模拟卡号
            Array.Copy(System.BitConverter.GetBytes(cardNO), 4, buff, 24, 4); //2017-10-31 14:51:50 模拟卡号 高4字节
            buff[32] = (byte)(0x5A); //不受设备内的权限约束
            //Array.Copy(System.BitConverter.GetBytes(sequenceId4RemoteOpen), 0, buff, 40, 4);
            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                    log(string.Format("\r\n    ====> 发出远程开门指令 控制器SN={0}, 门号= {1}, {2} 模拟卡号= {3}, 流水号= 0x{4:X} \r\n",
                        sn.ToString(), doorNO.ToString(), (inOrOut == 1 ? "进门" : "出门"), cardNO, sequenceId));
                }
                else
                {
                    log(string.Format("\r\n    ====>??? 失败: 发出远程开门指令 控制器SN={0}, 门号= {1}, {2} 模拟卡号= {3} \r\n",
                      sn.ToString(), doorNO.ToString(), (inOrOut == 1 ? "进门" : "出门"), cardNO));
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            log(string.Format("\r\n    ====>收到返回 信息 {0} \r\n",
                            System.BitConverter.ToString(reply)));
                            log((reply[8] == 1) ? "成功!\r\n" : "失败...\r\n");
                            if (reply[8] == 1)
                            {
                                ret = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                }
                if (ret >= 0)
                {
                    break;
                }        
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            return ret;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bStopWatchServer = true;
            this.groupBox1.Enabled  = false;  
        }

        private void lstControllers_Click(object sender, EventArgs e)
        {
            if (lstControllers.Items.Count > 0)
            {
                if (lstControllers.SelectedIndex < 0)
                {
                    lstControllers.SelectedIndex = 0;
                }
                this.txtSN.Text = lstControllers.Items[lstControllers.SelectedIndex].ToString();
            }
        }

        Thread tRunBatch = null; //2017-12-27 16:59:09 批量线程

        private void btnGetAllSwipes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //getSwipeRecords(ref udpserverLast, sn);
                //Thread tGetSwipe = new Thread(new ParameterizedThreadStart(getSwipeRecords));
                 if ( (tRunBatch != null) && (tRunBatch.IsAlive))
                {
                    MessageBox.Show("正在提取记录");
                }
                else
                {
                    //直接处理最后SN数据
                    long sn;
                    sn = long.Parse(this.txtSN.Text);
                    ArrayList arrSN = new ArrayList();
                    if (this.chkAllControllers.Checked)
                    {
                        for (int i = 0; i < lstControllers.Items.Count; i++ )
                        {
                            arrSN.Add(long.Parse( lstControllers.Items[i].ToString()));
                        }
                    }
                    else
                    {
                        arrSN.Add( sn);
                    }
                    tRunBatch = new Thread(new ParameterizedThreadStart(getSwipeRecords));
                    tRunBatch.Name = "Batch Running "; //2010-12-11 21:09:44 
                    tRunBatch.Start(new object[] { udpserverLast, arrSN });
                }
            }
        }

        /// <summary>
        /// 执行通信指令
        /// </summary>
        /// <param name="serverCurrent"></param>
        /// <param name="sn"></param>
        /// <param name="buff"></param>
        /// <param name="recv"></param>
        /// <returns>小于0表示失败, 大于0 表示指令流水号</returns>
        int runComm(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn, byte[] buff, ref byte[] recv)
        {
            int ret = -13;
            
            buff[0] = (byte)0x17; //Type;
           
            Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);

            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                }
                else
                {
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            recv = reply;
                            ret = 1;
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                } 
                if (ret >= 0)
                {
                    break;
                }
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            else
            {
                ret = (int) sequenceId;
            }
            return ret;
        }

        void getSwipeRecords(object arg)  //2017-12-24 15:18:05 获取所有记录
        {
            object[] objs = arg as object[];
            WG3000_COMM.Core.wgUdpServerCom serverCurrent = (WG3000_COMM.Core.wgUdpServerCom)objs[0];
            ArrayList arrSN = (ArrayList)objs[1];
            for (int indexSn = 0; indexSn < arrSN.Count; indexSn++)
            {
                long sn = (long) arrSN[indexSn];
                //1024字节指令 测试
                int ret = 0;
                int success = 0;  //0 失败, 1表示成功
                byte[] command1024 = new byte[1024];


                //1.9	提取记录操作
                //1. 通过 0xB0指令 获取最早一条记录索引
                //2. 通过 0xB0指令 获取最后一条记录索引
                //3. 通过 0xB4指令 获取已读取过的记录索引号 recordIndex
                //4. 通过 0xB0指令 获取指定索引号的记录  从recordIndex + 1开始提取记录， 直到记录为空为止
                //5. 通过 0xB2指令 设置已读取过的记录索引号  设置的值为最后读取到的刷卡记录索引号
                //经过上面步骤， 整个提取记录的操作完成
                long firstRecordIndex = 0;  //第一条记录索引号
                long lastRecordIndex = 0;   //最后一条记录索引号
                long recordIndexGotToRead = 0x0;
                long recordIndexToGet = 0;
                log("控制器SN = " + sn.ToString());
                log("1.9 提取记录操作	 开始...[1024字节指令]");
                //pkt.Reset();
                //pkt.functionID = 0xB0;//取最早的一条记录索引
                //recordIndexToGet = 0x0;
                //LongToBytes(ref pkt.data, 0, recordIndexToGet);
                //ret = pkt.run();

                byte[] buff = new byte[WGPacketShort.WGPacketSize];
                for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
                {
                    buff[i] = 0;
                }

                buff[0] = 0x17;  //2017-12-28 11:02:10 
                buff[1] = 0xb0;
                recordIndexToGet = 0x0;
                LongToBytes(ref buff, 8, recordIndexToGet);
                byte[] recv = null;
                ret = runComm(ref serverCurrent, sn, buff, ref recv);
                success = 0;
                if (ret >= 0)
                {
                    firstRecordIndex = (int)byteToLong(recv, 8, 4);
                    log(" 获取最早一条记录索引	 =" + firstRecordIndex.ToString());
                }
                if (ret >= 0)
                {
                    for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
                    {
                        buff[i] = 0;
                    }
                    buff[0] = 0x17;  //2017-12-28 11:02:10 
                    buff[1] = 0xb0;  //取最后的一条记录索引
                    recordIndexToGet = 0xffffffff; //取最后的一条记录索引
                    LongToBytes(ref buff, 8, recordIndexToGet);
                    recv = null;
                    ret = runComm(ref serverCurrent, sn, buff, ref recv);
                }
                if (ret >= 0)
                {
                    lastRecordIndex = (int)byteToLong(recv, 8, 4);
                    log(" 获取最后一条记录索引	  =" + lastRecordIndex.ToString());
                }

                if (this.chkGetAllSwipes.Checked)
                {
                    recordIndexGotToRead = firstRecordIndex - 1;  //2017-12-27 15:20:31 取所有记录时
                }
                else
                {
                    //已提取过的记录 不再提取
                    if (ret >= 0)
                    {
                        
                        for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
                        {
                            buff[i] = 0;
                        }
                        buff[0] = 0x17;  //2017-12-28 11:02:10 
                        buff[1] = 0xB4;//获取已读取过的记录索引号
                        recordIndexToGet = 0;
                        LongToBytes(ref buff, 8, recordIndexToGet);
                        recv = null;
                        ret = runComm(ref serverCurrent, sn, buff, ref recv);
                    }
                    if (ret >= 0)
                    {
                        recordIndexGotToRead = (int)byteToLong(recv, 8, 4);
                        log("获取已读取过的记录索引号	  =" + recordIndexGotToRead.ToString());
                    }
                }
                long validRecordsCount = 0;
                //recordIndexGotToRead = 0;  //2015-11-05 21:31:05 强制取所有记录
                if (ret >= 0)
                {
                    long recordIndexValidGet = 0;

                    long recordIndexToGetStart = recordIndexGotToRead + 1;  //准备要提取的记录索引位
                    if (recordIndexGotToRead > lastRecordIndex || recordIndexGotToRead < firstRecordIndex) //超过范围 取第一个记录的索引号
                    {
                        recordIndexToGetStart = firstRecordIndex;
                    }

                    long recordIndexCurrent;
                    int cnt = 0;
                    for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
                    {
                        buff[i] = 0;
                    }
                    buff[0] = 0x17;//
                    buff[1] = 0xB0;//
                    recordIndexToGet = 0;
                    LongToBytes(ref buff, 8, recordIndexToGet);
                    Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);
                    recv = null;
                    //ret = runComm(ref serverCurrent, sn, buff, ref recv);
                    do
                    {
                        for (int j = 0; j < 1024; j++)
                        {
                            command1024[j] = 0; //复位
                        }
                        recordIndexCurrent = recordIndexToGetStart;
                        for (int j = 0; j < 1024; j = j + 64)
                        {
                            LongToBytes(ref buff, 8, recordIndexToGetStart);
                            //byte[] cmd = pkt.toByte();
                            Array.Copy(buff, 0, command1024, j, 64);
                            recordIndexToGetStart++;
                            cnt++;
                        }
                        //ret = pkt.run1024(command1024);
                        ret = runComm(ref serverCurrent, sn, command1024, ref recv);
                        success = 0;
                        if (ret >= 0)
                        {
                            for (int j = 0; j < 1024; j = j + 64)
                            {
                                success = 0;

                                //12	记录类型
                                //0=无记录
                                //1=刷卡记录
                                //2=门磁,按钮, 设备启动, 远程开门记录
                                //3=报警记录	1	
                                //0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值
                                byte[] recvNew = new byte[64];
                                Array.Copy(recv, j, recvNew, 0, 64);
                                int recordType = recvNew[12];
                                if (recordType == 0)
                                {
                                    success = 2;
                                    break; //没有更多记录
                                }
                                if (recordType == 0xff)//此索引号无效
                                {
                                    success = 0;
                                    break;
                                }
                                success = 1;
                                recordIndexValidGet = recordIndexCurrent;
                                recordIndexCurrent++;
                                validRecordsCount++;
                                //
                                if (validRecordsCount < 100) //2015-11-05 14:59:20显示前100个, 太多显示处理速度慢 不作分析了...
                                {
                                    displayRecordInformation(recvNew); //2015-06-09 20:01:21
                                    if (validRecordsCount == 99)
                                    {
                                        log(" 为加快提取速度, 超过100个的  不再显示记录信息.......");
                                        Application.DoEvents();
                                    }
                                }
                                //.......对收到的记录作存储处理
                                //*****
                                //###############
                            }
                        }
                        else
                        {
                            //提取失败
                            break;
                        }
                        if (success != 1)
                        {
                            break;
                        }
                    } while (cnt < 200000);

                    log("1.9 完全提取成功	 ... 有效记录数= " + validRecordsCount.ToString());
                    if ((success > 0) && validRecordsCount > 0)
                    {
                        //通过 0xB2指令 设置已读取过的记录索引号  设置的值为最后读取到的刷卡记录索引号
                        //pkt.Reset();
                        //pkt.functionID = 0xB2;
                        //LongToBytes(ref pkt.data, 0, recordIndexValidGet);

                        ////12	标识(防止误设置)	1	0x55 [固定]
                        //LongToBytes(ref pkt.data, 4, WGPacketShort.SpecialFlag);

                        //ret = pkt.run();


                        for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
                        {
                            buff[i] = 0;
                        }
                        buff[0] = 0x17;  //2017-12-28 11:02:10 
                        buff[1] = 0xB2;//通过 0xB2指令 设置已读取过的记录索引号  设置的值为最后读取到的刷卡记录索引号
                        recordIndexToGet = 0;
                        LongToBytes(ref buff, 8, recordIndexValidGet);
                        LongToBytes(ref buff, 12, WGPacketShort.SpecialFlag);
                        Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);
                        recv = null;
                        ret = runComm(ref serverCurrent, sn, buff, ref recv);
                        success = 0;
                        if (ret >= 0)
                        {
                            if (recv[8] == 1)
                            {
                                //完全提取成功....
                                log("1.9 完全提取成功	 成功...");
                                success = 1;
                            }
                        }

                    }
                }
            }

        }

        private void btnAdjustTime_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //直接处理最后SN数据
                long sn;
                sn = long.Parse(this.txtSN.Text);
               int ret = adjustTime(ref udpserverLast, sn);
            }
        }

        /// <summary>
        /// 校准时间
        /// </summary>
        /// <param name="serverCurrent">通信服务器</param>
        /// <param name="sn">控制器序列号SN</param>
        /// <returns>等于1表示成功 其他值表示失败</returns>
        int adjustTime(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn)  
        {
            int ret = -13;
           byte[] buff = new byte[WGPacketShort.WGPacketSize];
            for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
            {
                buff[i] = 0;
            }
            buff[0] = (byte)0x17; //Type;
            buff[1] = (byte)0x30; //functionID;
            Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);

            DateTime ptm = DateTime.Now;
            buff[8+0] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100);
            buff[8+1] = (byte)GetHex((int)((ptm.Year) % 100)); //st.GetMonth()); 
            buff[8+2] = (byte)GetHex(ptm.Month);
            buff[8+3] = (byte)GetHex(ptm.Day);
            buff[8+4] = (byte)GetHex(ptm.Hour);
            buff[8+5] = (byte)GetHex(ptm.Minute);
            buff[8+6] = (byte)GetHex(ptm.Second);
            
            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                    log(string.Format("\r\n    ====> 发出校准时间 指令 控制器SN={0},  流水号= 0x{1:X} \r\n",
                        sn.ToString(),  sequenceId));
                }
                else
                {
                    log(string.Format("\r\n    ====>??? 失败: 发出校准时间指令 控制器SN={0} \r\n",
                      sn.ToString()));
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            log(string.Format("\r\n    ====>收到返回 信息 {0} \r\n",
                            System.BitConverter.ToString(reply)));
                            Boolean bOK = true;
                            for (int i = 0; i < 8; i++)
                            {
                              if (  buff[8+i]!=reply[8+i])
                                {
                                    bOK = false;
                                    break;
                                }
                            }
                            log(bOK ? "成功!\r\n" : "失败...\r\n");
                            if (bOK)
                            {
                                ret = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                }
                if (ret >= 0)
                {
                    break;
                }
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            return ret;
        }

        private void btnUploadPrivilegeSingle_Click(object sender, EventArgs e) //2018-01-04 09:50:18 上传单个权限
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //直接处理最后SN数据
                long sn;
                sn = long.Parse(this.txtSN.Text);

                long cardNO = long.Parse(this.txtCardNO.Text); //卡号
                int ret = uploadPrivilegSingle(ref udpserverLast, sn,cardNO,this.dtpActivate.Value, this.dtpDeactivate.Value);
            }
        }

        int uploadPrivilegSingle(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn,  long cardNO,DateTime beginDate, DateTime endDate)  //上传单个权限
        {
            int ret = -13; //
            //2017-09-07 10:56:10 检查卡号是否满足要求
            byte[] buff = new byte[WGPacketShort.WGPacketSize];
            for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
            {
                buff[i] = 0;
            }
            buff[0] = (byte)0x17; //Type;
            buff[1] = (byte)0x50; //functionID;
            Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);

            //1.11	权限添加或修改[功能号: 0x50] **********************************************************************************
            //增加卡号0D D7 37 00, 通过当前控制器的所有门
            //0D D7 37 00 要添加或修改的权限中的卡号 = 0x0037D70D = 3659533 (十进制)

            Array.Copy(System.BitConverter.GetBytes(cardNO), 0, buff, 8, 4); //卡号
            Array.Copy(System.BitConverter.GetBytes(cardNO), 4, buff, 44, 4); //卡号 高4字节

            //20 10 01 01 起始日期:     (必须大于2001年)
            DateTime ptm = beginDate;
            buff[8+4] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100); //0x20;
            buff[8+5] = (byte)GetHex((int)((ptm.Year) % 100)); // 0x10;
            buff[8+6] = (byte)GetHex(ptm.Month); // 0x01;
            buff[8+7] = (byte)GetHex(ptm.Day);
            //20 29 12 31 截止日期: 
            ptm = endDate;
            buff[8+8] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100); //0x20;
            buff[8+9] = (byte)GetHex((int)((ptm.Year) % 100)); // 0x10;
            buff[8+10] = (byte)GetHex(ptm.Month); // 0x01;
            buff[8+11] = (byte)GetHex(ptm.Day);

            buff[8 + 30] = (byte)GetHex(ptm.Hour);   //时
            buff[8 + 31] = (byte)GetHex(ptm.Minute); //分

            buff[8+12] = 0x01; //01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
            
            buff[8+13] = 0x01; //01 允许通过 二号门 [对双门, 四门控制器有效] //如果禁止2号门, 则只要设为 0x00
            
            buff[8+14] = 0x01;//01 允许通过 三号门 [对四门控制器有效]
            
            buff[8+15] = 0x01;//01 允许通过 四号门 [对四门控制器有效]

            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                    log(string.Format("\r\n    ====> 发出 上传单个权限 指令 控制器SN={0},  卡号= {1}, 流水号= 0x{2:X} \r\n",
                        sn.ToString(),  cardNO, sequenceId));
                }
                else
                {
                    log(string.Format("\r\n    ====>??? 失败: 发出指令 控制器SN={0},  卡号= {1} \r\n",
                      sn.ToString(),   cardNO));
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            log(string.Format("\r\n    ====>收到返回 信息 {0} \r\n",
                            System.BitConverter.ToString(reply)));
                            log((reply[8] == 1) ? "成功!\r\n" : "失败...\r\n");
                            if (reply[8] == 1)
                            {
                                ret = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                }
                if (ret >= 0)
                {
                    break;
                }
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            return ret;
        }


        //2018-01-04 10:49:36 上传所有的用户权限
        private void btnUploadAllPrivileges_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //getSwipeRecords(ref udpserverLast, sn);
                //Thread tGetSwipe = new Thread(new ParameterizedThreadStart(getSwipeRecords));
                if ((tRunBatch != null) && (tRunBatch.IsAlive))
                {
                    MessageBox.Show("正在上传权限或提取记录...");
                }
                else
                {
                    //直接处理最后SN数据
                    long sn;
                    sn = long.Parse(this.txtSN.Text);
                    ArrayList arrSN = new ArrayList();
                    if (this.chkAllControllerB.Checked)
                    {
                        for (int i = 0; i < lstControllers.Items.Count; i++)
                        {
                            arrSN.Add(long.Parse(lstControllers.Items[i].ToString()));
                        }
                    }
                    else
                    {
                        arrSN.Add(sn);
                    }
                    tRunBatch = new Thread(new ParameterizedThreadStart(uploadAllPrivlieges));
                    tRunBatch.Name = "Batch Running "; //2010-12-11 21:09:44 
                    tRunBatch.Start(new object[] { udpserverLast, arrSN });
                }
            }
        }


        void uploadAllPrivlieges(object arg)  //2018-01-04 10:55:54 上传所有权限
        {
            object[] objs = arg as object[];
            WG3000_COMM.Core.wgUdpServerCom serverCurrent = (WG3000_COMM.Core.wgUdpServerCom)objs[0];
            ArrayList arrSN = (ArrayList)objs[1];
            for (int indexSn = 0; indexSn < arrSN.Count; indexSn++)
            {
                long sn = (long)arrSN[indexSn];
                //1024字节指令 测试
                int ret = 0;
                int success = 0;  //0 失败, 1表示成功
                byte[] command1024 = new byte[1024];

                log("控制器SN = " + sn.ToString());
                log("上传权限操作	 开始...[1024字节指令]");

                 byte[] buff = new byte[WGPacketShort.WGPacketSize];
                for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
                {
                    buff[i] = 0;
                }
                 {
                    

                    //1.21	权限按从小到大顺序添加[功能号: 0x56]  **********************************************************************************
                    //此功能实现 完全更新全部权限, 用户不用清空之前的权限. 只是将上传的权限顺序从第1个依次到最后一个上传完成. 如果中途中断的话, 仍以原权限为主
                    //建议权限数更新超过50个, 即可使用此指令
                    //如果权限数超过8万时, 中途中断的话, 权限会为空. 所以要上传完整

                    log("1.21	权限按从小到大顺序添加[功能号: 0x56]	开始...[采用1024字节指令, 每次上传16个权限]");

                     //卡号部分**************************************************************************************************
                    //以10000个卡号为例, 此处简化的排序, 直接是以50001开始的10000个卡. 用户按照需要将要上传的卡号排序存放
                    int cardCount = 1 * 10000; // 10000;  //2015-06-09 20:20:20 卡总数量
                    log(string.Format("       {0}万条权限...", cardCount / 10000));
                    long[] cardArray = new long[cardCount];
                    for (int i = 0; i < cardCount; i++)
                    {
                        cardArray[i] = 50001 + i;
                    }
                     //****************************************************************************


                    long cardNOOfPrivilegeToGetlast = 0;
                    long cardNOOfPrivilege;
                    for (int i = 0; i < cardCount; )
                    {
                        for (int j = 0; j < 1024; j++)
                        {
                            command1024[j] = 0; //复位
                        }


                        buff[0] = 0x17;//
                        buff[1] = 0x56;//
                        buff[2] = 0;//
                        buff[3] = 0;//
                        Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4); //控制器SN

                        for (int j = 0; j < 1024; j = j + 64)
                        {
                            if (i >= cardCount)
                            {
                                break;
                            }
                           

                            cardNOOfPrivilege = cardArray[i];
                            if (cardNOOfPrivilegeToGetlast >= cardNOOfPrivilege)
                            {
                                log("卡号没有从小到大排序...???  (上传失败)");
                                success = 0;
                                break;
                            }
                            cardNOOfPrivilegeToGetlast = cardNOOfPrivilege;
                            LongToBytes(ref buff, 8, cardNOOfPrivilege);

                            //其他参数简化时 统一, 可以依据每个卡的不同进行修改
                            //20 10 01 01 起始日期:  2010年01月01日   (必须大于2001年)
                            buff[8+4] = 0x20;
                            buff[8+5] = 0x10;
                            buff[8+6] = 0x01;
                            buff[8+7] = 0x01;
                            //20 29 12 31 截止日期:  2029年12月31日  23点59分
                            buff[8+8] = 0x20;
                            buff[8+9] = 0x29;
                            buff[8+10] = 0x12;
                            buff[8+11] = 0x31;

                            buff[8 + 30] = 0x23;   //时
                            buff[8 + 31] = 0x59;   //分


                            //01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
                            buff[8+12] = 0x01;
                            //01 允许通过 二号门 [对双门, 四门控制器有效]
                            buff[8+13] = 0x01;  //如果禁止2号门, 则只要设为 0x00
                            //01 允许通过 三号门 [对四门控制器有效]
                            buff[8+14] = 0x01;
                            //01 允许通过 四号门 [对四门控制器有效]
                            buff[8+15] = 0x01;




                            LongToBytes(ref buff, 32, cardCount); //总的权限数
                            LongToBytes(ref buff, 35, i + 1);//当前权限的索引位(从1开始)

                            //byte[] cmd = pkt.toByte();
                            Array.Copy(buff, 0, command1024, j, 64);
                            i++;

                        }
                        byte[] recv = null;
                        ret = runComm(ref serverCurrent, sn, command1024, ref recv); 
                        success = 0;
                        if (ret > 0)
                        {
                            if (recv[8] == 1)
                            {
                                success = 1;
                            }
                            if (recv[8] == 0xE1)
                            {
                                log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 =0xE1 表示卡号没有从小到大排序...???");
                                success = 0;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (success == 1)
                    {
                        log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 成功...");
                    }
                    else
                    {
                        log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 失败...????");
                    }
                }
            }

        }

        private void btnControllerInfo_Click(object sender, EventArgs e) //获取设备运行信息
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //直接处理最后SN数据
                long sn;
                sn = long.Parse(this.txtSN.Text);
                WG3000_COMM.Common.wgControllerInfo con = this.udpserverLast.GetControllerInfo((int)sn); //2018-01-04 16:07:27
                if (con != null)
                {
                    string info = "";
                    info += string.Format("控制器序列号SN = {0}\r\n", con.ControllerSN);
                    info += string.Format("通信IP = {0}\r\n", con.IP);
                    info += string.Format("通信PORT = {0}\r\n", con.PORT);
                    info += string.Format("刷新时间 = {0}\r\n", con.UpdateDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    //2018-01-06 10:25:47 
                    info += string.Format("接收数据 = {0}\r\n",(con.ReceivedBytes));

                    log(info);

                    string[] recvinfo = con.ReceivedBytes.Split('-');
                    byte[] recv = new byte[recvinfo.Length];
                    for (int i = 0; i < recv.Length; i++)
                    {
                        recv[i] = (byte)(int.Parse(recvinfo[i],System.Globalization.NumberStyles.AllowHexSpecifier));
                    }
                    if (recv.Length == 64 && recv[1]==0x20)
                    {
                        log(" 查询控制器状态...");

                        //	  	最后一条记录的信息		
                        displayRecordInformation(recv); //2015-06-09 20:01:21

                        //	其他信息		
                        int[] doorStatus = new int[4];
                        //28	1号门门磁(0表示关上, 1表示打开)	1	0x00
                        doorStatus[1 - 1] = recv[28];
                        //29	2号门门磁(0表示关上, 1表示打开)	1	0x00
                        doorStatus[2 - 1] = recv[29];
                        //30	3号门门磁(0表示关上, 1表示打开)	1	0x00
                        doorStatus[3 - 1] = recv[30];
                        //31	4号门门磁(0表示关上, 1表示打开)	1	0x00
                        doorStatus[4 - 1] = recv[31];

                        int[] pbStatus = new int[4];
                        //32	1号门按钮(0表示松开, 1表示按下)	1	0x00
                        pbStatus[1 - 1] = recv[32];
                        //33	2号门按钮(0表示松开, 1表示按下)	1	0x00
                        pbStatus[2 - 1] = recv[33];
                        //34	3号门按钮(0表示松开, 1表示按下)	1	0x00
                        pbStatus[3 - 1] = recv[34];
                        //35	4号门按钮(0表示松开, 1表示按下)	1	0x00
                        pbStatus[4 - 1] = recv[35];

                        //36	故障号
                        //等于0 无故障
                        //不等于0, 有故障(先重设时间, 如果还有问题, 则要返厂家维护)	1	
                        int errCode = recv[36];

                        //37	控制器当前时间
                        //时	1	0x21
                        //38	分	1	0x30
                        //39	秒	1	0x58

                        //40-43	流水号	4	
                        long sequenceId = 0;
                        sequenceId = byteToLong(recv, 40+4, 4);

                        //48
                        //特殊信息1(依据实际使用中返回)
                        //键盘按键信息	1	


                        //49	继电器状态	1	 [0表示门上锁, 1表示门开锁. 正常门上锁时, 值为0000]
                        int relayStatus = recv[49];
                        if ((relayStatus & 0x1) > 0)
                        {
                            //一号门 开锁
                        }
                        else
                        {
                            //一号门 上锁
                        }
                        if ((relayStatus & 0x2) > 0)
                        {
                            //二号门 开锁
                        }
                        else
                        {
                            //二号门 上锁
                        }
                        if ((relayStatus & 0x4) > 0)
                        {
                            //三号门 开锁
                        }
                        else
                        {
                            //三号门 上锁
                        }
                        if ((relayStatus & 0x8) > 0)
                        {
                            //四号门 开锁
                        }
                        else
                        {
                            //四号门 上锁
                        }

                        //50	门磁状态的8-15bit位[火警/强制锁门]
                        //Bit0  强制锁门
                        //Bit1  火警		
                        int otherInputStatus = recv[50];
                        if ((otherInputStatus & 0x1) > 0)
                        {
                            //强制锁门
                        }
                        if ((otherInputStatus & 0x2) > 0)
                        {
                            //火警
                        }
                        if ((otherInputStatus & 0x8) > 0)
                        {
                            //扩展板 已连接
                            //54 扩展口输入/输出
                            log(string.Format("扩展板输入口/输出口数据 驱动要求是V7.72 [正常连接时起作用] 低4bits为输出口 ={0}, 高4bits为输入口 ={1} ",recv[54] & 0xf, (recv[54]>>4)&0xf));
                        }

                        //51	V5.46版本支持 控制器当前年	1	0x13
                        //52	V5.46版本支持 月	1	0x06
                        //53	V5.46版本支持 日	1	0x22

                        string controllerTime = "2000-01-01 00:00:00"; //控制器当前时间
                        controllerTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                            0x20, recv[51], recv[52], recv[53], recv[37], recv[38], recv[39]);
                        log(string.Format("控制器时间:{0}" , controllerTime));

                        if (WG3000_COMM.Common.wgControllerInfo.IsElevator((int)sn)) //梯控 一对多设备
                        {
                            //如果是一对多输入
                            log(string.Format("一对多信息[输入]: {0:X2}-{1:X2}-{2:X2} {3:X2}-{4:X2}-{5:X2}", recv[32],recv[33],recv[34],recv[28],recv[29],recv[30]));
                            string infoConnected = "";
                            string infoNotConnected = "";
                            int cntConnected = 0;
                            int cntNotConnected = 0;
                            if ((recv[34] & 0x80) > 0)
                            {
                                log(string.Format("一对多信息 已连接1-20输入口 "));
                                long val = recv[32] + (recv[33]<<8) + (recv[34]<<16);
                                for (int i = 0; i < 20; i++)
                                {
                                    if ((val & (1 << i)) > 0)
                                    {
                                        infoNotConnected = infoNotConnected + (i + 1).ToString() + ",";
                                        cntNotConnected++;
                                    }
                                    else
                                    {
                                        infoConnected = infoConnected + (i + 1).ToString() + ",";
                                        cntConnected++;
                                    }
                                }
                            }
                            if ((recv[30] & 0x80) > 0)
                            {
                                log(string.Format("一对多信息 已连接21-40输入口 "));
                                long val = recv[28] + (recv[29] << 8) + (recv[30] << 16);
                                 for (int i = 0; i < 20; i++)
                                {
                                    if ((val & (1 << i)) > 0)
                                    {
                                        infoNotConnected = infoNotConnected + (i + 21).ToString() + ",";
                                        cntNotConnected++;
                                    }
                                    else
                                    {
                                        infoConnected = infoConnected + (i + 21).ToString() + ",";
                                        cntConnected++;
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(infoConnected) && string.IsNullOrEmpty(infoNotConnected))
                            {
                                //没有连接多输入口板
                            }
                            else
                            {
                                log(string.Format("多输入口,已短接的端口有{0}个,分别是: {1} ",cntConnected, infoConnected));
                                log(string.Format("多输入口,空端口有{0}个,分别是: {1} ",cntNotConnected,infoNotConnected));
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(this.txtSN.Text))
                    {
                    }
                    else
                    {
                        //直接处理最后SN数据
                        int ret = getControllerInfo(ref udpserverLast, sn);
                    }
                }
            }
        }

        int getControllerInfo(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn)  //获取版本信息
        {
            int ret = -13;
            byte[] buff = new byte[WGPacketShort.WGPacketSize];
            for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
            {
                buff[i] = 0;
            }
            buff[0] = (byte)0x17; //Type;
            buff[1] = (byte)0x94; //functionID;
            Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);

           

            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                    //log(string.Format("\r\n    ====> 发出 指令 控制器SN={0},  流水号= 0x{1:X} \r\n",
                    //    sn.ToString(), sequenceId));
                }
                else
                {
                    log(string.Format("\r\n    ====>??? 失败: 发出指令 控制器SN={0} \r\n",
                      sn.ToString()));
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            //log(string.Format("\r\n    ====>收到返回 信息 {0} \r\n", System.BitConverter.ToString(reply)));
                            int infoloc = 8;
                            log(string.Format("控制器配置IP: {0}.{1}.{2}.{3}", reply[infoloc], reply[infoloc + 1], reply[infoloc + 2], reply[infoloc + 3]));
                            infoloc = 12;
                            log(string.Format("控制器配置掩码: {0}.{1}.{2}.{3}", reply[infoloc], reply[infoloc + 1], reply[infoloc + 2], reply[infoloc + 3]));
                            infoloc = 16;
                            log(string.Format("控制器配置网关: {0}.{1}.{2}.{3}", reply[infoloc], reply[infoloc + 1], reply[infoloc + 2], reply[infoloc + 3]));
                            infoloc = 20;
                            log(string.Format("控制器配置MAC: {0:X2}:{1:X2}:{2:X2}:{3:X2}:{4:X2}:{5:X2}", reply[infoloc], reply[infoloc + 1], reply[infoloc + 2], reply[infoloc + 3], reply[infoloc + 4], reply[infoloc + 5]));
                            infoloc = 26;
                            log(string.Format("驱动版本: V{0:X}.{1:X}", reply[infoloc], reply[infoloc + 1]));
                            ret = 1;
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                }
                if (ret >= 0)
                {
                    break;
                }
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            return ret;
        }

        private void btnOpenFloor_Click(object sender, EventArgs e)  //开指定楼层
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //直接处理最后SN数据
                long sn;
                sn = long.Parse(this.txtSN.Text);
                if (!WG3000_COMM.Common.wgControllerInfo.IsElevator((int)sn))
                {
                    MessageBox.Show("此控制器不是一对多(梯控)设备");
                    return;
                }
                int floorNO = 1;
                if (this.cboFloors.SelectedIndex >= 0)
                {
                    floorNO = this.cboFloors.SelectedIndex + 1;
                }
                long cardNO = 99999; //模拟卡号
                int ret = remoteOpenFloor(ref udpserverLast, sn, floorNO,  cardNO);
            }
        }

        int remoteOpenFloor(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn, int floorNO,  long cardNO)  //2017-12-23 09:25:43 远程开门操作
        {
            int ret = -13; //
            //2017-09-07 10:56:10 检查卡号是否满足要求
            byte[] buff = new byte[WGPacketShort.WGPacketSize];
            for (int i = 0; i < WGPacketShort.WGPacketSize; i++)
            {
                buff[i] = 0;
            }
            buff[0] = (byte)0x17; //Type;
            buff[1] = (byte)0x40; //functionID;
            Array.Copy(System.BitConverter.GetBytes(sn), 0, buff, 4, 4);

            //Array.Copy(data, 0, buff, 8, data.Length);
            int doorNO = 1;
            buff[8 + 0] = (byte)(doorNO & 0xff); //门号
            buff[9 + 0] = (byte)(floorNO & 0xff); //门号
            //            buff[28] = (byte)(inOrOut == 1 ? 0 : 1); // recordInOrOut == 1 ? "进门" : "出门"));
            Array.Copy(System.BitConverter.GetBytes(cardNO), 0, buff, 20, 4); //模拟卡号
            Array.Copy(System.BitConverter.GetBytes(cardNO), 4, buff, 24, 4); //2017-10-31 14:51:50 模拟卡号 高4字节
            buff[32] = (byte)(0x5A); //不受设备内的权限约束
            //Array.Copy(System.BitConverter.GetBytes(sequenceId4RemoteOpen), 0, buff, 40, 4);
            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                    log(string.Format("\r\n    ====> 发出远程开门指令 控制器SN={0}, 楼号= {1}  模拟卡号= {2}, 流水号= 0x{3:X} \r\n",
                        sn.ToString(), floorNO.ToString(), cardNO, sequenceId));
                }
                else
                {
                    log(string.Format("\r\n    ====>??? 失败: 发出远程开门指令 控制器SN={0}, 楼号= {1}  模拟卡号= {2} \r\n",
                      sn.ToString(), floorNO.ToString(),  cardNO));
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            log(string.Format("\r\n    ====>收到返回 信息 {0} \r\n",
                            System.BitConverter.ToString(reply)));
                            log((reply[8] == 1) ? "成功!\r\n" : "失败...\r\n");
                            if (reply[8] == 1)
                            {
                                ret = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                }
                if (ret >= 0)
                {
                    break;
                }
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            return ret;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtInfo.Text = "";
        }

        Boolean bDisplayDetail = true; //2018-01-06 12:45:05显示详细日志
        private void chkLogDetail_CheckedChanged(object sender, EventArgs e)
        {
            bDisplayDetail = this.chkLogDetail.Checked; 
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
    
        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
                if (lstControllers.Items.Count > 0)
                {
                    this.txtSN.Text = lstControllers.Items[0].ToString();
                }
            }
            if (string.IsNullOrEmpty(this.txtSN.Text))
            {
            }
            else
            {
                //直接处理最后SN数据
                long sn;
                sn = long.Parse(this.txtSN.Text);
                

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
                Array.Copy(System.BitConverter.GetBytes(sn), 0, bytArr, 4, 4);
                int ret = sendCust64Byte(ref udpserverLast, sn, bytArr);
             }
        }

        int sendCust64Byte(ref WG3000_COMM.Core.wgUdpServerCom serverCurrent, long sn, byte[] buff)  //2017-12-23 09:25:43 远程开门操作
        {
            int ret = -13; //
            int tries = 3;
            long sequenceId = 0;
            while (tries > 0)
            {
                serverCurrent.receivedReplyClear(); //先清空回复信息
                sequenceId = serverCurrent.UDP_OnlySend(buff, sequenceId);
                if (sequenceId >= 0)
                {
                    log(string.Format("\r\n    ====> 发出指令 控制器SN={0},  流水号= 0x{1:X} \r\n",
                        sn.ToString(), sequenceId));
                }
                else
                {
                    log(string.Format("\r\n    ====>??? 失败: 发出远程开门指令 控制器SN={0} \r\n",
                      sn.ToString()));
                    log("控制器 没有连接...");
                    return ret;
                }
                int waits = 100;
                while (waits-- > 0)
                {
                    if (udpserverLast.receivedReplyCount() > 0)
                    {
                        byte[] reply = udpserverLast.getReply();
                        if (byteToLong(reply, 40, 4) == sequenceId)
                        {
                            log(string.Format("\r\n    ====>收到返回 信息 {0} \r\n",
                            System.BitConverter.ToString(reply)));
                            log((reply[8] == 1) ? "成功!\r\n" : "失败...\r\n");
                            if (reply[8] == 1)
                            {
                                ret = 1;
                            }
                            break;
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(10);  //'延时10ms
                    }
                }
                //if (waits < 0)
                //{
                //    log("失败...\r\n");
                //}
                if (waits >= 0)
                {
                    break;
                }
                if (ret >= 0)
                {
                    break;
                }
                tries--;
            }
            if (ret <= 0)
            {
                log("失败...\r\n");
            }
            return ret;
        }

        private void chkDisplayIP_CheckedChanged(object sender, EventArgs e)
        {
           
        }




    }
}
