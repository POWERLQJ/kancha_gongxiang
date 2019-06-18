package org.wiegand.TestZone;

import java.io.IOException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.UnknownHostException;

import java.text.DateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.LinkedList;

import java.util.Queue;


import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.transport.socket.DatagramSessionConfig;
import org.apache.mina.transport.socket.nio.NioDatagramAcceptor;

import org.wiegand.at8000.WgUdpCommShort4Cloud;
import org.wiegand.at8000.wgControllerInfo;


public class TestShort {
	/**
	* AT8000_Java 2015-04-30 12:47:48 karl CSN 陈绍宁 $
	*
	* 门禁控制器 短报文协议 测试案例
	* V2.1 版本  2013-11-09
	*            主要使用 MINA完成
	*            基本功能:  查询控制器状态
	*                       读取日期时间
	*                       设置日期时间
	*                       获取指定索引号的记录
	*                       设置已读取过的记录索引号
	*                       获取已读取过的记录索引号
	*                       远程开门
	*                       权限添加或修改
	*                       权限删除(单个删除)
	*                       权限清空(全部清掉)
	*                       权限总数读取
	*                       权限查询
	*                       设置门控制参数(在线/延时)
	*                       读取门控制参数(在线/延时)

	*                       设置接收服务器的IP和端口
	*                       读取接收服务器的IP和端口
	*
	*
	*                       接收服务器的实现 (在61005端口接收数据) -- 此项功能 一定要注意防火墙设置 必须是允许接收数据的.
	* V2.5 版本  2015-04-30   采用 V6.56驱动版本 型号由0x19改为0x17
	* V2.6 版本  2017-11-01   在接收数据时加入延时
	        long times = 100; 
		    	try {
					 Thread.sleep(times);
				  } catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				  }  //2017-11-01 14:45:57 增加延时
    * V3.7 版本 2018-07-10 10:15:44 移植到云服务器上使用		
    *    如下功能只能在本地操作:  设置接收服务器的IP和端口
    *    Export -> Java -> Runnable JAR File    输出的文件可以执行...
    *    
    *    
    *   2018-10-16 18:05:54 打开显示部分 //2018-10-16 18:05:15				         if (1<0)  //暂时屏蔽掉 显示数据部分
    * V3.8版本 2018-12-11 16:23:26 
    *   显示8字节的卡号信息
    *   显示二维码透传信息  
	*/
	/**
	 * @param args
	 */
	public static boolean bStopRunning = false; //2018-09-01 18:18:55 停止运行
	
	public static void main(String[] args) {
		
		//本案例 先由 控制器设置工具 设置[看云服务器操作视频]
		//本案例中测试说明
		//用于作为接收服务器的IP (本电脑IP 192.168.168.101), 接收服务器端口 (61005)
        //更多功能请看  短报文协议 或 咨询开发技术支持
		
//		String watchServerIP = "10.0.1.14"; //云服务器或测试电脑的IP地址  "192.168.168.101";
		String watchServerIP = "172.16.0.5"; //"111.230.128.68"; //云服务器或测试电脑的IP地址  "192.168.168.101";
		int watchServerPort = 61005;
//异常处理机制
		if(args.length>0)  //参数
		{
			 
			 try {
				 watchServerIP = args[0]; //2018-09-02 16:42:19设置了IP参数
				 if (args.length>1)
				 {
					 watchServerPort =   Integer.valueOf( args[1]);
				 }
				} catch (NumberFormatException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
					log("NumberFormatException e1 ..." );
					log(e1.toString());
					return;
				}
		}
		else
		{
		//获得本机IP 
		try {
			String addr = InetAddress.getLocalHost().getHostAddress();
			if ((addr != null) && (!addr.equals("127.0.0.1")))  //2018-09-01 17:40:57本地127不使用
			{
				watchServerIP = addr; 
			}
		} catch (UnknownHostException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
			log("UnknownHostException e1 ..." );
			log(e1.toString());
			return;
		}
		}
		int ret =0;

		//log("先运行云服务器...");
		log("First  running cloud server...");  //2018-09-01 18:21:24 英文显示
		log(String.format(" 云服务器IP=%s,  Port=%d...",watchServerIP,watchServerPort));
		log(String.format(" Cloud Server IP=%s,  Port=%d...",watchServerIP,watchServerPort));  //2018-09-01 18:21:24 英文显示
		
		ret = WatchingServerRuning(watchServerIP, watchServerPort); //服务器运行....
		if (ret ==0 )
		{
			log("云服务器监控程序启动 失败...[请检查IP和端口 是否被占用]");
			log("Cloud Server Running Failed...[Port may be occupied.]");  //2018-09-01 18:21:24 英文显示
			log("AA测试结束...");
			log("AA Test Complete");  //2018-09-01 18:21:24 英文显示
			bStopRunning = true; //2018-09-01 18:19:15 停止
			return;
		}
		
		log("测试开始...");
		log("Test Begin...");  //2018-09-01 18:21:24 英文显示
        
		
		long dateWaitEnd = System.currentTimeMillis() +60*1000;
		int dealtIndex = 0;
		long controllerSN = 0; //229999901;
		if(1>0)
        {
			while(true) //2018-09-02 15:58:16 
			{
				if (WatchingShortHandler.arrSNReceived.size() > dealtIndex )
				{
					controllerSN = WatchingShortHandler.arrSNReceived.get(dealtIndex);
					if (WatchingShortHandler.isConnected(controllerSN))
					{
						log(String.format("控制器SN = %d 连接************************ \r\n", controllerSN));
						log(String.format("Controller SN = %d  Connected************************ \r\n", controllerSN));  //2018-09-01 18:21:24 英文显示
					    dealtIndex = dealtIndex+1; //2018-07-14 07:40:16 等待下一个接入设备 
						continue;
					}
				}
				try {
					Thread.sleep(30);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
        }
	
		log("1分钟 等待控制器连接  测试远程开门...");
		log("One Minute... waiting controller connect...  test remote open door");  //2018-09-01 18:21:24 英文显示

		//在60秒内--等待60秒的
		while (dateWaitEnd > System.currentTimeMillis() ) //2018-07-14 07:22:07 1分钟之内
		{
			//2018-07-11 16:23:35 等待控制器已经连接上才进行基本测试
			if (WatchingShortHandler.arrSNReceived.size() > dealtIndex )
			{
				controllerSN = WatchingShortHandler.arrSNReceived.get(dealtIndex);
				if (!WatchingShortHandler.isConnected(controllerSN))
				{
					log(String.format("控制器SN = %d 未连接************************ \r\n", controllerSN));
					log(String.format("Controller SN = %d Not Connected************************ \r\n", controllerSN));  //2018-09-01 18:21:24 英文显示
					continue;
				}

				ret = testBasicFunctionRemoteOpenDoor(controllerSN,1 ); //基本功能测试 远程开门 1号门
				if (controllerSN > 200000000)
				{
				ret = testBasicFunctionRemoteOpenDoor(controllerSN,2 ); //基本功能测试 远程开门 2号门
				}
				if (controllerSN > 400000000)
				{
				ret = testBasicFunctionRemoteOpenDoor(controllerSN,3 ); //基本功能测试 远程开门 3号门
				ret = testBasicFunctionRemoteOpenDoor(controllerSN,4 ); //基本功能测试 远程开门 4号门		
				}
				dealtIndex = dealtIndex+1; //2018-07-14 07:40:16 等待下一个接入设备 
				//break; //2018-07-14 09:09:08 只处理一台控制器时
			}
			else
			{
				try {
					Thread.sleep(30);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}
		
//		if (controllerSN >0)  //针对最后一台控制器的处理
//		{		
//			if (WatchingShortHandler.isConnected(controllerSN))
//			{
//				//强制提取所有记录
//				//ret =	testBasicFunction1024GetSwipe(controllerSN , 1); //2018-07-14 09:01:14 采用 1024字节指令  强制提取所有记录  注意显示了头 99条 
//				// 提取新记录 
//				ret = testBasicFunction1024GetSwipe(controllerSN , 0); //2018-07-14 09:01:14 采用 1024字节指令  提取所有新记录     注意显示了头 99条
//				ret = testBasicFunction(controllerSN ); //基本功能测试
//			}
//			else
//			{
//				log(String.format("控制器SN = %d 未连接************************ \r\n", controllerSN));
//				log(String.format("Controller SN = %d Not Connected************************ \r\n", controllerSN));  //2018-09-01 18:21:24 英文显示
//			}
//		}
//
//		bStopRunning = true; //2018-09-01 18:19:15 停止
		log("EE测试结束...");
		log("EE Test Completed...");  //2018-09-01 18:21:24 英文显示
	}

	public static int 	 testBasicFunctionRemoteOpenDoor(long controllerSN, int doorNO)  //基本功能测试--远程开门操作...
	{
		byte[] ;
		int success =0;
		WgUdpCommShort4Cloud pkt = new WgUdpCommShort4Cloud();
		pkt.iDevSn = controllerSN;
		
		
		log(String.format("远程开控制器SN = %d, %d号门************************ \r\n", controllerSN, doorNO));
		log(String.format("RemoteOpen Controller SN = %d, DoorNO = %d************************ \r\n", controllerSN, doorNO));  //2018-09-01 18:21:24 英文显示
		
		//打开udp连接
//		pkt.CommOpen("");
		
		//1.10	远程开门[功能号: 0x40] **********************************************************************************
		//2018-07-14 07:23:39  int doorNO =1;
		pkt.Reset();
		pkt.functionID = (byte) 0x40;
		pkt.iDevSn = controllerSN; 
		pkt.data[0] =(byte) (doorNO & 0xff); //2013-11-03 20:56:33
		 = pkt.run();
		success =0;
		if ( != null)
		{
			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
			{
				//有效开门.....
				log("1.10 远程开门	 成功...");
				log("1.10 Remote Open Door	 OK...");  //2018-09-01 18:21:24 英文显示
				success =1;
			}
		}

		//其他指令  **********************************************************************************
		//结束  **********************************************************************************

		//关闭udp连接
//		pkt.CommClose();
		
		return success;
	}
	
	public static void log(String info) //日志信息
	{
	//2018-07-14 18:40:46	System.out.println(info);
		DateFormat df=DateFormat.getTimeInstance();
		System.out.println("["+df.format(new Date(System.currentTimeMillis())) + "]  " + info); //curDate.toString("HH:mm:ss"));

	}

	public static byte GetHex(int val) //获取Hex值, 主要用于日期时间格式
    {
	    return (byte)((val % 10) + (((val -(val % 10)) / 10)%10) *16);
    }
	
	
    /// <summary>
    /// 显示记录信息
    /// </summary>
    /// <param name=""></param>
	public static void displayRecordInformation(byte[] )
    {

      //8-11	最后一条记录的索引号
		//(=0表示没有记录)	4	0x00000000
		long recordIndex = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);


		//12	记录类型
		//0=无记录
		//1=刷卡记录
		//2=门磁,按钮, 设备启动, 远程开门记录
		//3=报警记录	1	
		int recordType =WgUdpCommShort4Cloud.getIntByByte([12]);

		//13	有效性(0 表示不通过, 1表示通过)	1	
		int recordValid = WgUdpCommShort4Cloud.getIntByByte([13]);

		//14	门号(1,2,3,4)	1	
		int recordDoorNO = WgUdpCommShort4Cloud.getIntByByte([14]);

		//15	进门/出门(1表示进门, 2表示出门)	1	0x01
		int recordInOrOut = WgUdpCommShort4Cloud.getIntByByte([15]);

		//16-19	卡号(类型是刷卡记录时)
		//或编号(其他类型记录)	4	
		long  recordCardNO =WgUdpCommShort4Cloud.getLongByByte(, 16, 4);
		recordCardNO =recordCardNO + (WgUdpCommShort4Cloud.getLongByByte(, 44, 4)<<32);
		
		//20-26	刷卡时间:
		//年月日时分秒 (采用BCD码)见设置时间部分的说明
		String recordTime=  String.format("%02X%02X-%02X-%02X %02X:%02X:%02X", 
			WgUdpCommShort4Cloud.getIntByByte([20]),
			WgUdpCommShort4Cloud.getIntByByte([21]),
			WgUdpCommShort4Cloud.getIntByByte([22]),
			WgUdpCommShort4Cloud.getIntByByte([23]),
			WgUdpCommShort4Cloud.getIntByByte([24]),
			WgUdpCommShort4Cloud.getIntByByte([25]),
			WgUdpCommShort4Cloud.getIntByByte([26]));

		//2012.12.11 10:49:59	7	
		//27	记录原因代码(可以查 "刷卡记录说明.xls"文件的ReasonNO)
		//处理复杂信息才用	1	
		int reason = WgUdpCommShort4Cloud.getIntByByte([27]);
		
        //0=无记录
        //1=刷卡记录
        //2=门磁,按钮, 设备启动, 远程开门记录
        //3=报警记录	1	
        //0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值
        if (recordType == 0)
        {
            log(String.format("索引位=%u  无记录", recordIndex));
        }
        else if (recordType == 0xff)
        {
            log(" 指定索引位的记录已被覆盖掉了,请使用索引0, 取回最早一条记录的索引值");
        }
        else if (recordType == 1) //2015-06-10 08:49:31 显示记录类型为卡号的数据
        {
            //卡号
            log((String.format("索引位=%d  ", recordIndex))+"\r\n" +
            (String.format("  卡号 = %d", recordCardNO))+"\r\n" +
            (String.format("  门号 = %d", recordDoorNO))+"\r\n" +
            (String.format("  进出 = %s", recordInOrOut == 1 ? "进门" : "出门"))+"\r\n" +
            (String.format("  有效 = %s", recordValid == 1 ? "通过" : "禁止"))+"\r\n" +
            (String.format("  时间 = %s", recordTime))+"\r\n" +
            (String.format("  描述 = %s", getReasonDetailChinese(reason))));
        }
        else if (recordType == 2)
        {
            //其他处理
            //门磁,按钮, 设备启动, 远程开门记录
            log((String.format("索引位=%d  非刷卡记录", recordIndex))+"\r\n" +
            (String.format("  编号 = %d", recordCardNO))+"\r\n" +
            (String.format("  门号 = %d", recordDoorNO))+"\r\n" +
            (String.format("  时间 = %s", recordTime))+"\r\n" +
            (String.format("  描述 = %s", getReasonDetailChinese(reason))));
        }
        else if (recordType == 3)
        {
            //其他处理
            //报警记录
            log((String.format("索引位=%d  报警记录", recordIndex))+"\r\n" +
            (String.format("  编号 = %d", recordCardNO))+"\r\n" +
            (String.format("  门号 = %d", recordDoorNO))+"\r\n" +
            (String.format("  时间 = %s", recordTime))+"\r\n" +
            (String.format("  描述 = %s", getReasonDetailChinese(reason))));
        }
    }

	public static  String RecordDetails[] =
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

    public static   String getReasonDetailChinese(int Reason) //中文
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

    public static String getReasonDetailEnglish(int Reason) //英文描述
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
    
    


    
	public static int 	 testBasicFunction(long controllerSN)  //基本功能测试
	{
		byte[] ;
		int success =0;
		WgUdpCommShort4Cloud pkt = new WgUdpCommShort4Cloud();
		pkt.iDevSn = controllerSN;
		
		
		log(String.format("控制器SN = %d \r\n", controllerSN));
		
//		//打开udp连接
//		pkt.CommOpen("");
		
//主动上传可以不用发送		
		//1.4	查询控制器状态[功能号: 0x20](实时监控用) **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x20;
		pkt.iDevSn = controllerSN; 
		 = pkt.run();

		success =0;
		if ( != null)
		{
			//读取信息成功...
			success =1;
			log("1.4 查询控制器状态 成功...");

			//	  	最后一条记录的信息		
			displayRecordInformation(); 

			//	其他信息		
			int[] doorStatus=new int[4];
			//28	1号门门磁(0表示关上, 1表示打开)	1	0x00
			doorStatus[1-1] = WgUdpCommShort4Cloud.getIntByByte([28]);
			//29	2号门门磁(0表示关上, 1表示打开)	1	0x00
			doorStatus[2-1] = WgUdpCommShort4Cloud.getIntByByte([29]);
			//30	3号门门磁(0表示关上, 1表示打开)	1	0x00
			doorStatus[3-1] = WgUdpCommShort4Cloud.getIntByByte([30]);
			//31	4号门门磁(0表示关上, 1表示打开)	1	0x00
			doorStatus[4-1] = WgUdpCommShort4Cloud.getIntByByte([31]);

			int[] pbStatus= new int[4];
			//32	1号门按钮(0表示松开, 1表示按下)	1	0x00
			pbStatus[1-1] = WgUdpCommShort4Cloud.getIntByByte([32]);
			//33	2号门按钮(0表示松开, 1表示按下)	1	0x00
			pbStatus[2-1] = WgUdpCommShort4Cloud.getIntByByte([33]);
			//34	3号门按钮(0表示松开, 1表示按下)	1	0x00
			pbStatus[3-1] = WgUdpCommShort4Cloud.getIntByByte([34]);
			//35	4号门按钮(0表示松开, 1表示按下)	1	0x00
			pbStatus[4-1] = WgUdpCommShort4Cloud.getIntByByte([35]);
			//36	故障号
			//等于0 无故障
			//不等于0, 有故障(先重设时间, 如果还有问题, 则要返厂家维护)	1	
			int errCode = WgUdpCommShort4Cloud.getIntByByte([36]);
			//37	控制器当前时间
			//时	1	0x21
			//38	分	1	0x30
			//39	秒	1	0x58

			//40-43	流水号	4	
			long   sequenceId= WgUdpCommShort4Cloud.getLongByByte(, 40, 4);

			//48
			//特殊信息1(依据实际使用中返回)
			//键盘按键信息	1	
			//49	继电器状态	1	
			int relayStatus = WgUdpCommShort4Cloud.getIntByByte([49]);
			//50	门磁状态的8-15bit位[火警/强制锁门]
			//Bit0  强制锁门
			//Bit1  火警		
			int otherInputStatus = WgUdpCommShort4Cloud.getIntByByte([50]);
			if ((otherInputStatus & 0x1) > 0)
			{
				//强制锁门
			}
			if ((otherInputStatus & 0x2) > 0)
			{
				//火警
			}

			//51	V5.46版本支持 控制器当前年	1	0x13
			//52	V5.46版本支持 月	1	0x06
			//53	V5.46版本支持 日	1	0x22

			String controllerTime; //控制器当前时间
			controllerTime= String.format("20%02X-%02X-%02X %02X:%02X:%02X", 
				WgUdpCommShort4Cloud.getIntByByte([51]),
				WgUdpCommShort4Cloud.getIntByByte([52]),
				WgUdpCommShort4Cloud.getIntByByte([53]),
				WgUdpCommShort4Cloud.getIntByByte([37]),
				WgUdpCommShort4Cloud.getIntByByte([38]),
				WgUdpCommShort4Cloud.getIntByByte([39]));
		}
		else
		{
			log("1.4 查询控制器状态 失败...");
			return 0;
		}



		//1.5	读取日期时间(功能号: 0x32) **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x32;
		pkt.iDevSn = controllerSN; 
		 = pkt.run();
		success =0;
		if ( != null)
		{
			String controllerTime; //控制器当前时间
			controllerTime= String.format("%02X%02X-%02X-%02X %02X:%02X:%02X", 
				WgUdpCommShort4Cloud.getIntByByte([8]),WgUdpCommShort4Cloud.getIntByByte([9]),WgUdpCommShort4Cloud.getIntByByte([10]),WgUdpCommShort4Cloud.getIntByByte([11]),WgUdpCommShort4Cloud.getIntByByte([12]),WgUdpCommShort4Cloud.getIntByByte([13]),WgUdpCommShort4Cloud.getIntByByte([14]));

			log("1.5 读取日期时间 成功...");
			//log(controllerTime);
			success =1;
		}

		//1.6	设置日期时间[功能号: 0x30] **********************************************************************************
		//按电脑当前时间校准控制器.....
		pkt.Reset();
		pkt.functionID = (byte) 0x30;
		pkt.iDevSn = controllerSN; 

		Calendar cal = (Calendar.getInstance());
   
		pkt.data[0] =GetHex((int)(( cal.get(Calendar.YEAR) -(cal.get(Calendar.YEAR)%100))/100)); 
		pkt.data[1] =GetHex((int)(( cal.get(Calendar.YEAR))%100)); //st.GetMonth()); 
		pkt.data[2] =GetHex( cal.get(Calendar.MONTH) + 1); 
		pkt.data[3] =GetHex(cal.get(Calendar.DAY_OF_MONTH)); 
		pkt.data[4] =GetHex(cal.get(Calendar.HOUR_OF_DAY)); 
		pkt.data[5] =GetHex(cal.get(Calendar.MINUTE)); 
		pkt.data[6] = GetHex(cal.get(Calendar.SECOND)); 
		 = pkt.run();
		success =0;
		if ( != null)
		{
			success =1;
			for(int i=0; i<7; i++)
			{
				if(pkt.data[i] != [8+i])
				{
					success = 0;
					break;
				}	
			}
			if (success >0)
			{
				log("1.6 设置日期时间 成功...");
			}
		}

		//1.7	获取指定索引号的记录[功能号: 0xB0] **********************************************************************************
		//(取索引号 0x00000001的记录)
		int  recordIndexToGet =0;
		pkt.Reset();
		pkt.functionID =(byte) 0xB0;
		pkt.iDevSn = controllerSN; 

		//	(特殊
		//如果=0, 则取回最早一条记录信息
		//如果=0xffffffff则取回最后一条记录的信息)
		//记录索引号正常情况下是顺序递增的, 最大可达0xffffff = 16,777,215 (超过1千万) . 由于存储空间有限, 控制器上只会保留最近的20万个记录. 当索引号超过20万后, 旧的索引号位的记录就会被覆盖, 所以这时查询这些索引号的记录, 返回的记录类型将是0xff, 表示不存在了.
		recordIndexToGet =1;
 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexToGet) , 0, pkt.data, 0, 4);
		

		 = pkt.run();
		success =0;
		if ( != null)
		{
			log("1.7 获取索引为1号记录的信息	 成功...");
			//	  	索引为1号记录的信息		
			displayRecordInformation(); 

			success =1;
		}

		//. 发出报文 (取最早的一条记录 通过索引号 0x00000000) [此指令适合于 刷卡记录超过20万时环境下使用]
		pkt.Reset();
		pkt.functionID = (byte) 0xB0;
		pkt.iDevSn = controllerSN; 
		recordIndexToGet =0;
 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexToGet) , 0, pkt.data, 0, 4);

 	     = pkt.run();
		success =0;
		if ( != null)
		{
			log("1.7 获取最早一条记录的信息	 成功...");
			//	  	最早一条记录的信息		
			displayRecordInformation(); 
			success =1;
		}

		//发出报文 (取最新的一条记录 通过索引 0xffffffff)
		pkt.Reset();
		pkt.functionID = (byte) 0xB0;
		pkt.iDevSn = controllerSN; 
		recordIndexToGet =0xffffffff;
 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexToGet) , 0, pkt.data, 0, 4);

 	     = pkt.run();
		success =0;
		if ( != null)
		{
			log("1.7 获取最新记录的信息	 成功...");
			//	  	最新记录的信息		
			displayRecordInformation(); 

			success =1;
		}

//		//1.8	设置已读取过的记录索引号[功能号: 0xB2] **********************************************************************************
//		pkt.Reset();
//		pkt.functionID = (byte) 0xB2;
//		pkt.iDevSn = controllerSN; 
//		// (设为已读取过的记录索引号为5)
//		long recordIndexGotToSet = 0x5;
// 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexGotToSet) , 0, pkt.data, 0, 4);
//
//		//12	标识(防止误设置)	1	0x55 [固定]
// 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(WgUdpCommShort4Cloud.SpecialFlag) , 0, pkt.data, 4, 4);
//
//		 = pkt.run();
//		success =0;
//		if ( != null)
//		{
//			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
//			{
//				log("1.8 设置已读取过的记录索引号	 成功...");
//				success =1;
//			}
//		}
//
//		//1.9	获取已读取过的记录索引号[功能号: 0xB4] **********************************************************************************
//		pkt.Reset();
//		pkt.functionID = (byte) 0xB4;
//		pkt.iDevSn = controllerSN; 
//		long recordIndexGotToRead =0x0;
//		 = pkt.run();
//		success =0;
//		if ( != null)
//		{
//			recordIndexGotToRead = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
//			log("1.9 获取已读取过的记录索引号	 成功...");
//			success =1;
//		}

		//1.9	提取记录操作
		//1. 通过 0xB4指令 获取已读取过的记录索引号 recordIndex
		//2. 通过 0xB0指令 获取指定索引号的记录  从recordIndex + 1开始提取记录， 直到记录为空为止
		//3. 通过 0xB2指令 设置已读取过的记录索引号  设置的值为最后读取到的刷卡记录索引号
		//经过上面三个步骤， 整个提取记录的操作完成
	    log("1.9 提取记录操作	 开始...");
		pkt.Reset();
		pkt.functionID = (byte) 0xB4;
		pkt.iDevSn = controllerSN; 
		long recordIndexGot4GetSwipe =0x0;
		 = pkt.run();
		success =0;
		if ( != null)
		{
			recordIndexGot4GetSwipe= WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
			pkt.Reset();
			pkt.functionID = (byte) 0xB0;
			pkt.iDevSn = controllerSN; 
			long recordIndexToGetStart = recordIndexGot4GetSwipe + 1;
			long recordIndexValidGet = 0;
			int cnt=0;
			do
			{
				System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexToGetStart) , 0, pkt.data, 0, 4);
				 = pkt.run();
				success =0;
				if ( != null)
				{
					success =1;

					//12	记录类型
					//0=无记录
					//1=刷卡记录
					//2=门磁,按钮, 设备启动, 远程开门记录
					//3=报警记录	1	
					//0xFF=表示指定索引位的记录已被覆盖掉了.  请使用索引0, 取回最早一条记录的索引值
					int recordType = WgUdpCommShort4Cloud.getIntByByte([12]);
					if (recordType == 0)
					{
						break; //没有更多记录
					}
					
					if (recordType == 0xff)
					{
						success = 0;   //此索引号无效  重新设置索引值
						//取最早一条记录的索引位
						recordIndexToGet =0;
				 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexToGet) , 0, pkt.data, 0, 4);

				 	     = pkt.run();
						success =0;
						if ( != null)
						{
							log("1.7 获取最早一条记录的信息	 成功...");
							//	  	最早一条记录的信息		
							
							success =1;
							long recordIndex =0;
			                recordIndex  = WgUdpCommShort4Cloud.getLongByByte(,8, 4);
	                        recordIndexToGetStart = recordIndex;
	                        continue;
						}
						
						
	                    success = 0;  
						break; 
					}
					recordIndexValidGet = recordIndexToGetStart;
					//.......对收到的记录作存储处理
					 displayRecordInformation();
					//*****
					//###############
				}
				else
				{
					//提取失败
					break;
				}
				recordIndexToGetStart++;
			}while(cnt++ < 200000);
			if (success >0)
			{
				//通过 0xB2指令 设置已读取过的记录索引号  设置的值为最后读取到的刷卡记录索引号
				pkt.Reset();
				pkt.functionID = (byte) 0xB2;
				pkt.iDevSn = controllerSN; 
				System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexValidGet) , 0, pkt.data, 0, 4);
				
				//12	标识(防止误设置)	1	0x55 [固定]
		 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(WgUdpCommShort4Cloud.SpecialFlag) , 0, pkt.data, 4, 4);

				 = pkt.run();
				success =0;
				if ( != null)
				{
					if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
					{
						//完全提取成功....
						log("1.9 完全提取成功	 成功...");
						success =1;
					}
				}

			}
		}

		//1.10	远程开门[功能号: 0x40] **********************************************************************************
		int doorNO =1;
		pkt.Reset();
		pkt.functionID = (byte) 0x40;
		pkt.iDevSn = controllerSN; 
		pkt.data[0] =(byte) (doorNO & 0xff); //2013-11-03 20:56:33
		 = pkt.run();
		success =0;
		if ( != null)
		{
			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
			{
				//有效开门.....
				log("1.10 远程开门	 成功...");
				success =1;
			}
		}

		//1.11	权限添加或修改[功能号: 0x50] **********************************************************************************
		//增加卡号0D D7 37 00, 通过当前控制器的所有门
		pkt.Reset();
		pkt.functionID = (byte) 0x50;
		pkt.iDevSn = controllerSN; 
		//0D D7 37 00 要添加或修改的权限中的卡号 = 0x0037D70D = 3659533 (十进制)
		long cardNOOfPrivilege =0x0037D70D;
		//memcpy(&(pkt.data[0]), &cardNOOfPrivilege, 4);
		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilege) , 0, pkt.data, 0, 4);
		//20 10 01 01 起始日期:  2010年01月01日   (必须大于2001年)
		pkt.data[4] = 0x20;
		pkt.data[5] = 0x10;
		pkt.data[6] = 0x01;
		pkt.data[7] = 0x01;
		//20 29 12 31 截止日期:  2029年12月31日
		pkt.data[8] = 0x20;
		pkt.data[9] = 0x29;
		pkt.data[10] = 0x12;
		pkt.data[11] = 0x31;
		//01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
		pkt.data[12] = 0x01;
		//01 允许通过 二号门 [对双门, 四门控制器有效]
		pkt.data[13] = 0x01;  //如果禁止2号门, 则只要设为 0x00
		//01 允许通过 三号门 [对四门控制器有效]
		pkt.data[14] = 0x01;
		//01 允许通过 四号门 [对四门控制器有效]
		pkt.data[15] = 0x01;

		 = pkt.run();
		success =0;
		if ( != null)
		{
			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
			{
				//这时 刷卡号为= 0x0037D70D = 3659533 (十进制)的卡, 1号门继电器动作.
				log("1.11 权限添加或修改	 成功...");
				success =1;
			}
		}

		//1.12	权限删除(单个删除)[功能号: 0x52] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x52;
		pkt.iDevSn = controllerSN; 
		//要删除的权限卡号0D D7 37 00  = 0x0037D70D = 3659533 (十进制)
		long cardNOOfPrivilegeToDelete =0x0037D70D;
		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilegeToDelete) , 0, pkt.data, 0, 4);

		 = pkt.run();
		success =0;
		if ( != null)
		{
			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
			{
				//这时 刷卡号为= 0x0037D70D = 3659533 (十进制)的卡, 1号门继电器不会动作.
				log("1.12 权限删除(单个删除)	 成功...");
				success =1;
			}
		}

		//1.13	权限清空(全部清掉)[功能号: 0x54] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x54;
		pkt.iDevSn = controllerSN; 
		//12	标识(防止误设置)	1	0x55 [固定]
 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(WgUdpCommShort4Cloud.SpecialFlag) , 0, pkt.data, 0, 4);

		 = pkt.run();
		success =0;
		if ( != null)
		{
			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
			{
				//这时清空成功
				log("1.13 权限清空(全部清掉)	 成功...");
				success =1;
			}
		}

		//1.14	权限总数读取[功能号: 0x58] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x58;
		pkt.iDevSn = controllerSN; 
		 = pkt.run();
		success =0;
		if ( != null)
		{
			long privilegeCount  = WgUdpCommShort4Cloud.getLongByByte(,8, 4);
			log("1.14 权限总数读取	 成功...");

			success =1;
		}

		
		//再次添加为查询操作 1.11	权限添加或修改[功能号: 0x50] **********************************************************************************
		//增加卡号0D D7 37 00, 通过当前控制器的所有门
		pkt.Reset();
		pkt.functionID = (byte) 0x50;
		pkt.iDevSn = controllerSN; 
		//0D D7 37 00 要添加或修改的权限中的卡号 = 0x0037D70D = 3659533 (十进制)
		//long 
		cardNOOfPrivilege =0x0037D70D;
		
		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilege) , 0, pkt.data, 0, 4);
		//20 10 01 01 起始日期:  2010年01月01日   (必须大于2001年)
		pkt.data[4] = 0x20;
		pkt.data[5] = 0x10;
		pkt.data[6] = 0x01;
		pkt.data[7] = 0x01;
		//20 29 12 31 截止日期:  2029年12月31日
		pkt.data[8] = 0x20;
		pkt.data[9] = 0x29;
		pkt.data[10] = 0x12;
		pkt.data[11] = 0x31;
		//01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
		pkt.data[12] = 0x01;
		//01 允许通过 二号门 [对双门, 四门控制器有效]
		pkt.data[13] = 0x01;  //如果禁止2号门, 则只要设为 0x00
		//01 允许通过 三号门 [对四门控制器有效]
		pkt.data[14] = 0x01;
		//01 允许通过 四号门 [对四门控制器有效]
		pkt.data[15] = 0x01;

		 = pkt.run();
		success =0;
		if ( != null)
		{
			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
			{
				//这时 刷卡号为= 0x0037D70D = 3659533 (十进制)的卡, 1号门继电器动作.
				log("1.11 权限添加或修改	 成功...");
				success =1;
			}
		}
		
		//1.15	权限查询[功能号: 0x5A] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x5A;
		pkt.iDevSn = controllerSN; 
		// (查卡号为 0D D7 37 00的权限)
		long cardNOOfPrivilegeToQuery =0x0037D70D;
		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilegeToQuery) , 0, pkt.data, 0, 4);

		 = pkt.run();
		success =0;
		if ( != null)
		{

			long cardNOOfPrivilegeToGet = WgUdpCommShort4Cloud.getLongByByte(,8, 4);
			if (cardNOOfPrivilegeToGet == 0)
			{
				//没有权限时: (卡号部分为0)
				log ("1.15      没有权限信息: (卡号部分为0)");
			}
			else
			{
				//具体权限信息...
				log ("1.15     有权限信息...");

			}
			log("1.15 权限查询	 成功...");
			success =1;
		}
		
		//1.16  获取指定索引号的权限[功能号: 0x5C] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x5C;
		pkt.iDevSn = controllerSN; 
		
		cardNOOfPrivilegeToQuery =1;  //索引号(从1开始)
		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilegeToQuery) , 0, pkt.data, 0, 4);

		 = pkt.run();
		success =0;
		if ( != null)
		{

			long cardNOOfPrivilegeToGet = WgUdpCommShort4Cloud.getLongByByte(,8, 4);
			if (cardNOOfPrivilegeToGet == 4294967295l) //'FFFFFFFF对应于4294967295
			{
				//没有权限时: (卡号部分为0)
				log ("1.16      没有权限信息: (权限已删除)");
			}
			else if (cardNOOfPrivilegeToGet == 0)
			{
				//没有权限时: (卡号部分为0)
				log ("1.16       没有权限信息: (卡号部分为0)--此索引号之后没有权限了");
			}
			else
			{
				//具体权限信息...
				log ("1.16     有权限信息...");

			}
			log("1.15 权限查询	 成功...");
			success =1;
		}
		


		//1.17	设置门控制参数(在线/延时) [功能号: 0x80] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x80;
		pkt.iDevSn = controllerSN; 
		//(设置2号门 在线  开门延时 3秒)
		pkt.data[0] = 0x02; //2号门
		pkt.data[1] = 0x03; //在线
		pkt.data[2] = 0x03; //开门延时

		 = pkt.run();
		success =0;
		if ( != null)
		{
			success =1;		
			for(int i=0; i<3; i++)
			{
				if (pkt.data[i] != [8+i])
				{
				  success = 0;
				   break;
				}
			}
			if (success > 0)
			{
				//成功时, 返回值与设置一致
				log("1.17 设置门控制参数	 成功...");
				success =1;			
			}
		}
		
		
//		//1.21	权限按从小到大顺序添加[功能号: 0x56] 适用于权限数过1000, 少于8万 **********************************************************************************
//        //此功能实现 完全更新全部权限, 用户不用清空之前的权限. 只是将上传的权限顺序从第1个依次到最后一个上传完成. 如果中途中断的话, 仍以原权限为主
//        //建议权限数更新超过50个, 即可使用此指令
//
//        //逐个权限上传方式
//        log("1.21	权限按从小到大顺序添加[功能号: 0x56]	开始...");
//        log("       1万条权限...");
//
//        //以10000个卡号为例, 此处简化的排序, 直接是以50001开始的10000个卡. 用户按照需要将要上传的卡号排序存放
//        int cardCount = 10000;  //2015-06-09 20:20:20 卡总数量
//        long cardArray[]= new long[10000];
//        for (int i = 0; i < cardCount; i++)
//        {
//            cardArray[i] = 50001+i;
//        }
//
//        for (int i = 0; i < cardCount; i++)
//        {
//        	pkt.Reset();
//    		pkt.functionID = (byte) 0x56;
//    		pkt.iDevSn = controllerSN; 
//    		
//    		cardNOOfPrivilege =cardArray[i];
//    		
//    		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilege) , 0, pkt.data, 0, 4);
//    		//20 10 01 01 起始日期:  2010年01月01日   (必须大于2001年)
//    		pkt.data[4] = 0x20;
//    		pkt.data[5] = 0x10;
//    		pkt.data[6] = 0x01;
//    		pkt.data[7] = 0x01;
//    		//20 29 12 31 截止日期:  2029年12月31日
//    		pkt.data[8] = 0x20;
//    		pkt.data[9] = 0x29;
//    		pkt.data[10] = 0x12;
//    		pkt.data[11] = 0x31;
//    		//01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
//    		pkt.data[12] = 0x01;
//    		//01 允许通过 二号门 [对双门, 四门控制器有效]
//    		pkt.data[13] = 0x01;  //如果禁止2号门, 则只要设为 0x00
//    		//01 允许通过 三号门 [对四门控制器有效]
//    		pkt.data[14] = 0x01;
//    		//01 允许通过 四号门 [对四门控制器有效]
//    		pkt.data[15] = 0x01;
//
//    		
//    		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardCount) , 0, pkt.data, 32-8, 4);//总的权限数
//			int i2=i+1;
//			System.arraycopy(WgUdpCommShort4Cloud.longToByte(i2) , 0, pkt.data, 35-8, 4);//当前权限的索引位(从1开始)
//
//    		 = pkt.run();
//    		success =0;
//    		if ( != null)
//    		{
//    			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
//    			{
//    				success =1;
//    			}
//    			else	if (WgUdpCommShort4Cloud.getIntByByte([8]) == 0xE1)
//                {
//                    log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 =0xE1 表示卡号没有从小到大排序...???");
//                    success = 0;
//                    break;
//                }
//    			else 
//    			{
//    				 log("1.21	权限按从小到大顺序添加[功能号: 0x56]	??? [8]=" + [8]);
//    				 success = 0;
//                     break;
//    			}
//    		}
//            else
//            {
//                break;
//            }
//        }
//        if (success == 1)
//        {
//            log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 成功...");
//        }
//        else
//        {
//            log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 失败...????");
//        }

		

		//1.21	权限按从小到大顺序添加[功能号: 0x56]  **********************************************************************************
        //此功能实现 完全更新全部权限, 用户不用清空之前的权限. 只是将上传的权限顺序从第1个依次到最后一个上传完成. 如果中途中断的话, 仍以原权限为主
        //建议权限数更新超过50个, 即可使用此指令
        //如果权限数超过8万时, 中途中断的话, 权限会为空. 所以要上传完整

        log("1.21	权限按从小到大顺序添加[功能号: 0x56]	开始...[采用1024字节指令, 每次上传16个权限]");
         log("       1万条权限...");
         byte[] command1024 = new byte[1024];
        //以10000个卡号为例, 此处简化的排序, 直接是以50001开始的10000个卡. 用户按照需要将要上传的卡号排序存放
        int cardCount = 10000;  //2015-06-09 20:20:20 卡总数量
        long cardArray[]= new long[10000];
        for (int i = 0; i < cardCount; i++)
        {
            cardArray[i] = 50001+i;
        }

        long cardNOOfPrivilegeToGetlast = 0;
        for (int i = 0; i < cardCount; ) //2018-07-13 22:43:39  i++)
        {
        	 for (int j = 0; j < 1024; j++)
             {
                 command1024[j] = 0; //复位
             }

        	success = 1; 
    		for (int j = 0; j < 1024; j = j + 64)
            {
                if (i >= cardCount)
                {
                    break;
                }
        	pkt.Reset();
    		pkt.functionID = (byte) 0x56;
    		pkt.iDevSn = controllerSN; 
    		cardNOOfPrivilege =cardArray[i];
    		 if (cardNOOfPrivilegeToGetlast >= cardNOOfPrivilege)
             {
                 log("卡号没有从小到大排序...???  (上传失败)");
                 success = 0;
                 break;
             }
             cardNOOfPrivilegeToGetlast = cardNOOfPrivilege;
             
    		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardNOOfPrivilege) , 0, pkt.data, 0, 4);
    		//20 10 01 01 起始日期:  2010年01月01日   (必须大于2001年)
    		pkt.data[4] = 0x20;
    		pkt.data[5] = 0x10;
    		pkt.data[6] = 0x01;
    		pkt.data[7] = 0x01;
    		//20 29 12 31 截止日期:  2029年12月31日
    		pkt.data[8] = 0x20;
    		pkt.data[9] = 0x29;
    		pkt.data[10] = 0x12;
    		pkt.data[11] = 0x31;
    		//01 允许通过 一号门 [对单门, 双门, 四门控制器有效] 
    		pkt.data[12] = 0x01;
    		//01 允许通过 二号门 [对双门, 四门控制器有效]
    		pkt.data[13] = 0x01;  //如果禁止2号门, 则只要设为 0x00
    		//01 允许通过 三号门 [对四门控制器有效]
    		pkt.data[14] = 0x01;
    		//01 允许通过 四号门 [对四门控制器有效]
    		pkt.data[15] = 0x01;

    		
    		System.arraycopy(WgUdpCommShort4Cloud.longToByte(cardCount) , 0, pkt.data, 32-8, 4);//总的权限数
			int i2=i+1;
			System.arraycopy(WgUdpCommShort4Cloud.longToByte(i2) , 0, pkt.data, 35-8, 4);//当前权限的索引位(从1开始)
			
			System.arraycopy(pkt.toByte(), 0, command1024, j, 64);
             i++;

         }
    		if (success==0)
    		{
    			//卡号没有顺序排放时
    			break;
    		}
    		 = pkt.run(command1024);
    		success =0;
    		if ( != null)
    		{
    			if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
    			{
    				success =1;
    			}
    			else	if (WgUdpCommShort4Cloud.getIntByByte([8]) == 0xE1)
                {
                    log("1.21	权限按从小到大顺序添加[功能号: 0x56]	 =0xE1 表示卡号没有从小到大排序...???");
                    success = 0;
                    break;
                }
    			else 
    			{
    				 log("1.21	权限按从小到大顺序添加[功能号: 0x56]	??? [8]=" + [8]);
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
		
		//其他指令  **********************************************************************************

		
		//结束  **********************************************************************************

		//关闭udp连接
//		pkt.CommClose();
		
		return success;
	}

	//bForceGetAllSwipe  >=1  强制提取所有记录(包括之前已提取的), ==0 表示 提取新的记录(不含 已提取过的)
	public static int 	 testBasicFunction1024GetSwipe(long controllerSN, int bForceGetAllSwipe)  //通过1024字节获取数据
	{
		byte[] ;
		int success =0;
		WgUdpCommShort4Cloud pkt = new WgUdpCommShort4Cloud();
		pkt.iDevSn = controllerSN;
		
		
		log(String.format("控制器SN = %d \r\n", controllerSN));
		
		//打开udp连接
//		pkt.CommOpen("");
		

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
        log("控制器SN = " + controllerSN);
        log("1.9 提取记录操作	 开始...[1024字节指令]");
		pkt.Reset();
		pkt.functionID = (byte) 0xB0;
		pkt.iDevSn = controllerSN; 
		long recordIndexGot4GetSwipe =0x0;
		 = pkt.run();
		success =0;
		if ( != null)
		{			
            firstRecordIndex = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
            log(" 获取最早一条记录索引	 =" +String.valueOf( firstRecordIndex));
		}
		
            pkt.Reset();
    		pkt.functionID = (byte) 0xB0;
    		pkt.iDevSn = controllerSN; 
    		pkt.data[0] =(byte) 0xff; //取最后的一条记录索引
    		pkt.data[1] =(byte) 0xff; //取最后的一条记录索引
    		pkt.data[2] =(byte) 0xff; //取最后的一条记录索引
    		pkt.data[3] =(byte) 0xff; //取最后的一条记录索引
    		 = pkt.run();
    		success =0;
    		if ( != null)
    		{			
    			lastRecordIndex = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
                log(" 获取最后一条记录索引	 =" +String.valueOf( lastRecordIndex));
    		}
    		
    		
            pkt.Reset();
    		pkt.functionID = (byte) 0xB4; //获取已读取过的记录索引号
    		pkt.iDevSn = controllerSN; 
    		 = pkt.run();
    		success =0;
    		if ( != null)
    		{			
    			recordIndexGotToRead = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
                log(" 获取已读取过的记录索引号	 =" +String.valueOf( recordIndexGotToRead));
    		}	
            long validRecordsCount = 0;
            if (bForceGetAllSwipe>0)
            {
            recordIndexGotToRead = 0;  //2015-11-05 21:31:05 强制取所有记录
            }
            if ( != null)
            {
            	long recordIndexValidGet = 0;

                long recordIndexToGetStart = recordIndexGotToRead + 1;  //准备要提取的记录索引位
                if (recordIndexGotToRead > lastRecordIndex || recordIndexGotToRead < firstRecordIndex) //超过范围 取第一个记录的索引号
                {
                    recordIndexToGetStart = firstRecordIndex;
                }

                long recordIndexCurrent;
                int cnt = 0;
                
                
            recordIndexGot4GetSwipe= recordIndexGotToRead; //WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
			pkt.Reset();
			pkt.functionID = (byte) 0xB0;
			pkt.iDevSn = controllerSN; 
	        byte[] command1024 = new byte[1024];

			do
            {
                for (int j = 0; j < 1024; j++)
                {
                    command1024[j] = 0; //复位
                }
                recordIndexCurrent = recordIndexToGetStart;
                for (int j = 0; j < 1024; j = j + 64)
                {
        			pkt.Reset();
        			pkt.functionID = (byte) 0xB0;
        			pkt.iDevSn = controllerSN; 
                    System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexToGetStart) , 0, pkt.data, 0, 4);
                	System.arraycopy(pkt.toByte(), 0, command1024, j, 64);
                    recordIndexToGetStart++;
                    cnt++;
                }
                 = pkt.run(command1024);
                
                success = 0;
                if (!=null)
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
                        System.arraycopy(, j, recvNew, 0, 64);
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
			
			if (success >0)
			{
				//通过 0xB2指令 设置已读取过的记录索引号  设置的值为最后读取到的刷卡记录索引号
				pkt.Reset();
				pkt.functionID = (byte) 0xB2;
				pkt.iDevSn = controllerSN; 
				System.arraycopy(WgUdpCommShort4Cloud.longToByte(recordIndexValidGet) , 0, pkt.data, 0, 4);
				
				//12	标识(防止误设置)	1	0x55 [固定]
		 	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(WgUdpCommShort4Cloud.SpecialFlag) , 0, pkt.data, 4, 4);

				 = pkt.run();
				success =0;
				if ( != null)
				{
					if (WgUdpCommShort4Cloud.getIntByByte([8]) == 1)
					{
						//完全提取成功....
						log("1.9 完全提取成功	 成功...(1024字节指令)");
						success =1;
					}
				}

			}
		}

		//结束  **********************************************************************************

		//关闭udp连接
//		pkt.CommClose();
		
		return success;
	}
	
	//controllerIP 被设置的控制器IP地址
	//controllerSN 被设置的控制器序列号
	//watchServerIP   要设置的服务器IP
	//watchServerPort 要设置的端口
	@SuppressWarnings("unused")
	public static int testWatchingServer(String controllerIP, long controllerSN, String watchServerIP,int watchServerPort)  //接收服务器测试 -- 设置
	{
		byte[] ;
		int success =0;  //0 失败, 1表示成功
		WgUdpCommShort4Cloud pkt = new WgUdpCommShort4Cloud();
		pkt.iDevSn = controllerSN;
		
		//打开udp连接
//		pkt.CommOpen(controllerIP);
		
		//1.18	设置接收服务器的IP和端口 [功能号: 0x90] **********************************************************************************
		//	接收服务器的IP: 192.168.168.101  [当前电脑IP]
		//(如果不想让控制器发出数据, 只要将接收服务器的IP设为0.0.0.0 就行了)
		//接收服务器的端口: 61005
		//每隔5秒发送一次: 05
		pkt.Reset();
		pkt.functionID = (byte)0x90;
		pkt.iDevSn = controllerSN;


			
		//服务器IP: 192.168.168.101
		//pkt.data[0] = 192; 
		//pkt.data[1] = 168; 
		//pkt.data[2] = 168; 
		//pkt.data[3] = 101; 
		String[] ip;
        ip= watchServerIP.split("\\.");
        if (ip.length == 4)
        {
			pkt.data[0] =  (byte)Integer.parseInt(ip[0]);   
			pkt.data[1] =  (byte)Integer.parseInt(ip[1]);   
			pkt.data[2] =  (byte)Integer.parseInt(ip[2]);  
			pkt.data[3] =  (byte)Integer.parseInt(ip[3]); 
        }

		//接收服务器的端口: 61005
		pkt.data[4] =(byte)(watchServerPort & 0xff);
		pkt.data[5] =(byte)((watchServerPort >>8) & 0xff);

		//每隔5秒发送一次: 05 (定时上传信息的周期为5秒 [正常运行时每隔5秒发送一次  有刷卡时立即发送])
		pkt.data[6] = 5;

		 = pkt.run();
		success =0;
		if ( != null)
		{
			if ([8] == 1)
			{
				log("1.18 设置接收服务器的IP和端口 	 成功...");
				success =1;
			}
		}


		//1.19	读取接收服务器的IP和端口 [功能号: 0x92] **********************************************************************************
		pkt.Reset();
		pkt.functionID = (byte) 0x92;
		pkt.iDevSn = controllerSN; 

		 = pkt.run();
		success =0;
		if ( != null)
		{
			log("1.19 读取接收服务器的IP和端口 	 成功...");
			success =1;
		}
		
		//关闭udp连接
//		pkt.CommClose();
		return 1;
	}
	private static String toHex(byte[] bytes) {
		StringBuffer sb = new StringBuffer();
		for (int i = 0; i < bytes.length; i++) {
			String x16 = Integer.toHexString(bytes[i]);
			if (x16.length() < 2) {
				sb.append("0" + x16);
			} else if (x16.length() > 2) {
				sb.append(x16.substring(x16.length() - 2));
			} else
				sb.append(x16);
		}
		return sb.toString();
	}
	
	static Queue<byte[]> queue = new LinkedList<byte[]>();
	static Queue<byte[]> UDPQueue4Mobile = new LinkedList<byte[]>();
	static Queue<IoSession> RemoteEndPointQueue4Mobile = new LinkedList<IoSession>();
	public static int WatchingServerRuning(String watchServerIP,int watchServerPort) // 进入服务器监控状态
	{
		
		 // 创建UDP数据包NIO
        NioDatagramAcceptor acceptor = new NioDatagramAcceptor();
        // NIO设置底层IOHandler
        acceptor.setHandler(new WatchingShortHandler(queue, UDPQueue4Mobile, RemoteEndPointQueue4Mobile,UDPQueue4RemoteOpenDoor, RemoteEndPointQueue4RemoteOpenDoor));

        // 设置是否重用地址？ 也就是每个发过来的udp信息都是一个地址？
        DatagramSessionConfig dcfg = acceptor.getSessionConfig();
        dcfg.setReuseAddress(true);

        // 绑定端口地址
        try {
			acceptor.bind(new InetSocketAddress(watchServerIP, watchServerPort));
		} catch (IOException e) {
            log("绑定接收服务器失败....");
			log("Bind DataServer Failed....");  //2018-09-01 18:21:24 英文显示
			e.printStackTrace();
			return 0;
		}
        log("进入接收服务器监控状态....[如果在win7下使用 一定要注意防火墙设置]");
		log("Watching Server Started....");  //2018-09-01 18:21:24 英文显示

      //To monitor if receive Msg from Server
		new Thread()
		{
			@Override
			public void run()
			{
		      long recordIndex = 0;
//2018-09-01 18:20:06			  while(true)				
			  while(!) //2018-09-01 18:19:15 停止
			  {
				  boolean bDealt = false;
				  if (!queue.isEmpty())
				    {
					  bDealt= true;
				         byte[] ;
				         synchronized (queue)
				         {
				        	 = queue.poll();
				         }
//2018-10-16 18:05:15				         if (1<0)  //暂时屏蔽掉 显示数据部分
				         {
					         if (([1]== 0x20) )
									{
										long sn = WgUdpCommShort4Cloud.getLongByByte(, 4, 4);
										 if (WgUdpCommShort4Cloud.GetControllerType(sn) > 0)  //2018-03-21 12:00:12 有效的序列号
				                            {
										long recordIndexGet = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
										//2018-10-16 18:21:24 log(String.format("接收到来自控制器SN = %d 的数据包..##########", sn));
										//2018-10-16 18:20:58 log(String.format("Receve Data From Controlller SN = %d..##########", sn));  //2018-09-01 18:21:24 英文显示
										log(String.format("接收到来自控制器SN = %d 的数据包..记录索引号=%d##########", sn, recordIndexGet));
										log(String.format("Receve Data From Controlller SN = %d.. RecordIndex=%d..##########", sn,recordIndexGet));  //2018-10-16 18:21:03 增加记录索引号 2018-09-01 18:21:24 英文显示
			                            
										int iget = WatchingShortHandler.arrSNReceived.indexOf((int) sn);
										

								   		if (iget >= 0) {
	
								   			if (WatchingShortHandler.arrControllerInfo.size()>=iget)
								   			{
		                                    recordIndex = (long) WatchingShortHandler.arrRecordIndex.get(iget);
		                                    WatchingShortHandler.arrRecordIndex.set(iget,(int) recordIndexGet); //2017-09-07 11:23:50 保存新值
								   			wgControllerInfo info = WatchingShortHandler.arrControllerInfo.get(iget);
								   			if (info != null)
								   			{
								   				recordIndex = info.recordIndex4WatchingRemoteOpen;
								   				info.recordIndex4WatchingRemoteOpen =recordIndexGet;
	//											if (recordIndex < recordIndexGet)
						                        if ((recordIndex < recordIndexGet)  //2017-12-12 17:38:25 获取到记录索引 大于 已获取的记录索引号
						                                || ((recordIndexGet - recordIndex) < -5))  //新的记录索引 比 已获取的记录索引值小,  相差大于5条[则取新的](设备的记录索引号可能复位造成)		
												{
													recordIndex = recordIndexGet;
													displayRecordInformation(); 
													int recordType =WgUdpCommShort4Cloud.getIntByByte([12]);
													int recordValid = WgUdpCommShort4Cloud.getIntByByte([13]);
													if ((recordType == 1) //2015-06-10 08:49:31 显示记录类型为卡号的数据
										               && (recordValid == 0))  //2017-09-07 10:56:00 禁止通过时
									                   {
									                	 //14	门号(1,2,3,4)	1	
									                       int recordDoorNO = [14];
										            }
												}
				
								   			}
								   		}
									}
									}
									}	
					         else  if ( ([1]== 0x22))  //2018-07-10 10:39:43 增加0x22
								{
									long sn = WgUdpCommShort4Cloud.getLongByByte(, 4, 4);
									long recordIndexGet = WgUdpCommShort4Cloud.getLongByByte(, 8, 4);
									log(String.format("接收到来自控制器SN = %d 的二维码数据包....", sn));
									log(String.format("Receive QR Data From Controller SN = %d....", sn));  //2018-09-01 18:21:24 英文显示
									
									//2018-12-11 16:02:01 新增
									//8-11	二维码数据长度
						            //(=0表示没有记录)	4	0x00000000
									long qrDataLen = 0;
						            qrDataLen = WgUdpCommShort4Cloud.getLongByByte(, 8, 4); 
						            int doorNO = [14];
						            long cmdSequenceId = WgUdpCommShort4Cloud.getLongByByte(, 40, 4); //2017-09-07 15:56:06 流水号
						            if (qrDataLen >= 1) //2017-09-07 15:49:29 有二维码数据
						            {
						                byte[] qrData = new byte[(int)qrDataLen];
						                //Array.Copy(, 64, qrData, 0, qrDataLen); //数据
						                for (int i = 0; i < qrDataLen; i++) {
						                	qrData[i] = (byte) [64+i];
						        		}


						                log(String.format("流水号=%d 二维码原始数据:\r\n        %s\r\n", cmdSequenceId, toHex(qrData)));

						                //转换为字符串数据 
						                log(String.format("流水号=%d 二维码原始数据(转换为字符串):\r\n        %s",
						                    cmdSequenceId, new String (qrData)));

						                if (1>0)  //2018-12-11 16:43:24 如下是远程开门动作部分
						                {
						                int iloc = -1;
				                        if ((iloc = WatchingShortHandler.arrSNReceived.indexOf((int)sn)) >= 0)
				                        {
				                            //远程开门
				                        	log(String.format("收到二维码透传信息 发出 模拟卡号远程开门 指令\r\n"));
				                        	WgUdpCommShort4Cloud pkt = new WgUdpCommShort4Cloud();
				                    		
				                    		pkt.Reset();
				                    		pkt.functionID = (byte) 0x40;
				                    		pkt.iDevSn = sn; 
				                    		pkt.data[0] =(byte) (doorNO & 0xff); //2013-11-03 20:56:33
//				                    		pkt.data[12]= qrData[0];
//				                    		pkt.data[13]= qrData[1];
//				                    		pkt.data[14]= qrData[2];
//				                    		pkt.data[15]= qrData[3];//模拟卡号
//				                    		pkt.data[16]= qrData[4];
//				                    		pkt.data[17]= qrData[5];
//				                    		pkt.data[18]= qrData[6];
//				                    		pkt.data[19]= qrData[7];//2017-10-31 14:51:50 模拟卡号 高4字节

				                    		long  card4qr = 123456789777l;
				                    		pkt.data[12]= (byte)((card4qr>>0) & 0xff);
				                    		pkt.data[13]=  (byte)((card4qr>>8) & 0xff);
				                    		pkt.data[14]=  (byte)((card4qr>>16) & 0xff);
				                    		pkt.data[15]=  (byte)((card4qr>>24) & 0xff);//模拟卡号
				                    		pkt.data[16]=  (byte)((card4qr>>32) & 0xff);
				                    		pkt.data[17]=  (byte)((card4qr>>40) & 0xff);
				                    		pkt.data[18]=  (byte)((card4qr>>48) & 0xff);
				                    		pkt.data[19]=  (byte)((card4qr>>56) & 0xff);;//2017-10-31 14:51:50 模拟卡号 高4字节

				                    		pkt.data[24]=(byte)0x5a; //不受设备内的权限约束
				                            byte[] sendbuff = pkt.toByte(); //2018-02-12 10:22:05 修改
				                            
				                            UDP_OnlySend(sendbuff,  WatchingShortHandler.arrControllerInfo.get(iloc).Session); //2018-02-12 17:02:35
				                        }
						              }
						            }
								}
					         }
				    }
				  if (DealRuninfoPacketProc4Mobile())
				  {
					  bDealt = true;  
				  }
				  if (DealRuninfoPacketProc4RemoteOpenDoor())
				  {
					  bDealt = true;  
				  }
				   if(!bDealt)
				    {
		                long times = 100; 
				    	try {
							 Thread.sleep(times);
						  } catch (InterruptedException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
						  }  //2017-11-01 14:45:57 增加延时
				    }	     
			  }
// 如果 不是while(true), 则打开下面的注释...	  
//	  acceptor.unbind();
//	  acceptor.dispose();
//	  return 0;
			}
	}.start(); //2018-07-10 10:35:14
	return 1;
	}
	
	static ArrayList<Long> arrXIDSend = new ArrayList<Long>();
	static ArrayList<Long> arrXIDReceivedMobile = new ArrayList<Long>();
	static ArrayList<IoSession>  arrXIDEndPointMobile = new ArrayList<IoSession>();
	static long dtXIDSendLast = System.currentTimeMillis() ; // DateTime.Now.Ticks;
	
	static  Queue<byte[]> UDPQueue4RemoteOpenDoor=new LinkedList<byte[]>();  //2018-09-02 10:35:40 
	static  Queue<IoSession> RemoteEndPointQueue4RemoteOpenDoor= new LinkedList<IoSession>();  //2018-09-02 10:35:40 

    private static boolean DealRuninfoPacketProc4Mobile()  //2015-06-22 21:11:18处理来自手机的指令数据[主要是远程开门 或许还有加入数据]
    {
        byte[] recv;
        boolean ret = false;
        try
        {
            do
            {
                if (!UDPQueue4Mobile.isEmpty())
                {
                    //取数据
                    IoSession senderRemote;
                    synchronized (UDPQueue4Mobile)
                    {
                        recv = (byte[])UDPQueue4Mobile.poll();
                        senderRemote = (IoSession)RemoteEndPointQueue4Mobile.poll();
                    }
                    //if ((recv.Length == 64) && ((recv[1] == 0x20))) //信息指令
                    //{
                    //    arrSNReceived.Add();
                    //}

                    //if ((recv.Length == 64) && ((recv[1] == 0x28)) && ((recv[9] == 0xF0))) //手机专用指令 且是远程开门
                    //if ((recv.Length == 64) && ((recv[1] == 0x28))) //手机专用指令 
                    if ((recv.length == 64) && ((recv[0] == 0x37))) //2018-02-12 10:20:51 手机专用指令 
                    {
                        long m_ControllerSN = WgUdpCommShort4Cloud.getLongByByte(recv, 4,4);
                        int iloc = -1;
                        if ((iloc = WatchingShortHandler.arrSNReceived.indexOf((int)m_ControllerSN)) >= 0)
                        {
                            //直接将数据转发出去

                            recv[0] = 0x17; //2018-02-12 10:22:05 修改
                            long oldxid = WgUdpCommShort4Cloud.getXidOfCommand(recv);
                            long newxid = WgUdpCommShort4Cloud.getXid();
                            //产生唯一流水号
                        //    Array.Copy(System.BitConverter.GetBytes(newxid), 0, recv, 40, 4);
                     	    System.arraycopy(WgUdpCommShort4Cloud.longToByte(newxid) , 0, recv, 40, 4);
                     	   synchronized (arrXIDSend)
                            {
                                arrXIDSend.add(newxid);                 //新的
                                arrXIDReceivedMobile.add(oldxid);       //旧的
                                arrXIDEndPointMobile.add(senderRemote); //2015-06-21 15:34:08 指令源
                            }
                            dtXIDSendLast = System.currentTimeMillis() + 2 * 60 * 1000 ; // DateTime.Now.Ticks + 2 * 60 * 1000 * 1000 * 10; //2015-06-22 13:16:32 2分钟 Date.Now.Ticks  'us


                            UDP_OnlySend(recv,  WatchingShortHandler.arrControllerInfo.get(iloc).Session); //2018-02-12 17:02:35
                        }
                    }
                    ret = true;

                }
                else
                {
                    if (!arrXIDSend.isEmpty())
                    {
                        if (dtXIDSendLast < System.currentTimeMillis() ) //DateTime.Now.Ticks)
                        {
                            //2015-06-22 10:57:06 如果2分钟内没有接收到手机指令 清空数据信息...
                        	synchronized (arrXIDSend)
                            {
                                arrXIDSend.clear();                 //新的
                                arrXIDReceivedMobile.clear();       //旧的
                                arrXIDEndPointMobile.clear(); //2015-06-21 15:34:08 指令源
                            }
                        ret = true;
                       }
                     }
                    //2014-05-04 12:08:42 延时10ms 改为 1ms System.Threading.Thread.Sleep(10);  //'延时10ms
                  //  System.Threading.Thread.Sleep(1);  //2014-05-04 12:08:39 '延时1ms

                }
            } while(false); // (!bUDPListenStop);

        }
        catch (Exception e)
        {

          //  Debug.WriteLine(e.ToString());
        }
        return ret;
    }

    private static boolean DealRuninfoPacketProc4RemoteOpenDoor() //2015-06-22 21:10:46 处理远程开门指令的返回数据[来自控制器]
    {
        byte[] recv;
        boolean ret = false;
        try
        {
            do
            {
                if (!UDPQueue4RemoteOpenDoor.isEmpty())
                {
                    //取数据
                	synchronized (UDPQueue4RemoteOpenDoor)
                    {
                        recv = (byte[])UDPQueue4RemoteOpenDoor.poll();
                    }
                    if ((recv.length == 64) && ((recv[1] == 0x40))) //2015-06-24 20:18:12 手机指令  远程开门
                    {
                        long oldxid = WgUdpCommShort4Cloud.getXidOfCommand(recv);
                        int iloc = -1;

                        if ((iloc = arrXIDSend.indexOf(oldxid)) >= 0)
                        {
                            recv[0] = 0x37; //2018-02-12 17:08:53 手机发出的是0x37
                            //直接将数据转发出去
                            long newxid = (long)arrXIDReceivedMobile.get(iloc);
                            IoSession senderRemote = (IoSession)arrXIDEndPointMobile.get(iloc);
                           // Array.Copy(System.BitConverter.GetBytes(newxid), 0, recv, 40, 4); //原流水ID恢复
                            System.arraycopy(WgUdpCommShort4Cloud.longToByte(newxid) , 0, recv, 40, 4);
                            //recv[1] = 0x28; //
                            synchronized (arrXIDSend) //处理过的清除掉...
                            {
                                arrXIDEndPointMobile.remove(iloc);
                                arrXIDReceivedMobile.remove(iloc);
                                arrXIDSend.remove(iloc);
                            }

            //                UDP_OnlySend(recv, 300, ((IPEndPoint)senderRemote).Address.ToString(), (int)senderRemote.Port, 2); //2015-06-21 15:22:06 只是发出数据
                            UDP_OnlySend(recv, senderRemote); //2018-02-12 17:02:35
                       }
                    }
               ret = true;
                }
                else
                {
                    //2014-05-04 12:08:42 延时10ms 改为 1ms System.Threading.Thread.Sleep(10);  //'延时10ms
                  //  System.Threading.Thread.Sleep(1);  //2014-05-04 12:08:39 '延时1ms

                }
             } while (false); //(!bUDPListenStop);

        }
        catch (Exception e)
        {

          //  Debug.WriteLine(e.ToString());
        }
        return ret;
    }

    
    //       public int UDP_OnlySend(byte[] cmd, int parWaitMs, string ipAddr, int ipPort) //只发送数据包
    /// <summary>
    /// 只发送数据
    /// </summary>
    /// <param name="cmd">64字节指令</param>
    /// <param name="sequenceId">流水号(如果为0, 采用系统默认)</param>
    /// <returns></returns>
    public static long UDP_OnlySend(byte[] bytCommand, IoSession session) //只发送数据包
    {
    	IoBuffer b;
		if (session != null) {
			if (session.isConnected()) {
				b = IoBuffer.allocate(bytCommand.length);
				b.put(bytCommand);
				b.flip();
				session.write(b);
			}
		}       
		return 0;
    }

}
    


    

