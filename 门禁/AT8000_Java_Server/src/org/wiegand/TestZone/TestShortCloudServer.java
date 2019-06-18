package org.wiegand.TestZone;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.SocketException;
import java.net.UnknownHostException;

import java.text.DateFormat;
import java.util.ArrayList;
//import java.util.Calendar;
import java.util.Date;
import java.util.LinkedList;

import java.util.Queue;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.session.IoSession;
import org.apache.mina.transport.socket.DatagramSessionConfig;
import org.apache.mina.transport.socket.nio.NioDatagramAcceptor;

import org.wiegand.at8000.WgUdpCommShort4Cloud;
import org.wiegand.at8000.wgControllerInfo;

public class TestShortCloudServer {
	/**
	 * AT8000_Java 2015-04-30 12:47:48 karl CSN 陈绍宁 $
	 *
	 * 门禁控制器 短报文协议 测试案例 V2.1 版本 2013-11-09 主要使用 MINA完成 基本功能: 查询控制器状态 读取日期时间 设置日期时间
	 * 获取指定索引号的记录 设置已读取过的记录索引号 获取已读取过的记录索引号 远程开门 权限添加或修改 权限删除(单个删除) 权限清空(全部清掉)
	 * 权限总数读取 权限查询 设置门控制参数(在线/延时) 读取门控制参数(在线/延时)
	 * 
	 * 设置接收服务器的IP和端口 读取接收服务器的IP和端口
	 *
	 *
	 * 接收服务器的实现 (在61005端口接收数据) -- 此项功能 一定要注意防火墙设置 必须是允许接收数据的. V2.5 版本 2015-04-30 采用
	 * V6.56驱动版本 型号由0x19改为0x17 V2.6 版本 2017-11-01 在接收数据时加入延时 long times = 100; try {
	 * Thread.sleep(times); } catch (InterruptedException e) { // TODO
	 * Auto-generated catch block e.printStackTrace(); } //2017-11-01 14:45:57 增加延时
	 * V3.7 版本 2018-07-10 10:15:44 移植到云服务器上使用 如下功能只能在本地操作: 设置接收服务器的IP和端口 Export ->
	 * Java -> Runnable JAR File 输出的文件可以执行...
	 * 
	 * 
	 * 2018-10-16 18:05:54 打开显示部分 //2018-10-16 18:05:15 if (1<0) //暂时屏蔽掉 显示数据部分
	 * V3.8版本 2018-12-11 16:23:26 显示8字节的卡号信息 显示二维码透传信息
	 * 
	 * V3.9版本 2019-04-13 09:51:58 远程开门采用opendoorThread线程单独处理 [请查看Thread thread = new
	 * opendDoorThread] 去除掉局域网内使用代码, 只专用于 云服务器 实时监控和远程开门
	 * 
	 * V4.0版本 2019-05-19 09:45:03 客户端程序与java服务器建立通信关系, 实现远程开门, 上传权限, 提取权限, 提取记录的操作
	 * 基本测试可行了 服务器与控制器通信的报文流水号为0x40000001-0x4fffffff;
	 * 客户端与服务器通信的报文流水号是0x70000001-0x7fffffff 控制器主动上传的报文流水号为0x00
	 */
	/**
	 * @param args
	 */
	public static boolean bStopRunning = false; // 2018-09-01 18:18:55 停止运行

	// 本案例 先由 控制器设置工具 设置[看云服务器操作视频]
	// 本案例中测试说明
	// 用于作为接收服务器的IP (本电脑IP 192.168.168.101), 接收服务器端口 (61005)
	// 更多功能请看 短报文协议 或 咨询开发技术支持
	static String watchServerIP = "47.101.164.199"; // 云服务器或测试电脑的IP地址
	static int watchServerPort = 61005;

	public static void main(String[] args) {
		if (args.length > 0) // 参数
		{
			try {
				watchServerIP = args[0]; // 2018-09-02 16:42:19设置了IP参数
				if (args.length > 1) {
					watchServerPort = Integer.valueOf(args[1]);
				}
			} catch (NumberFormatException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
				log("NumberFormatException e1 ...");
				log(e1.toString());
				return;
			}
		} else {
			if (watchServerIP.equals("")) // 2019-05-19 11:12:23 为空时采用本机
			{
				// 获得本机IP
				try {
					String addr = InetAddress.getLocalHost().getHostAddress();
					if ((addr != null) && (!addr.equals("127.0.0.1"))) // 2018-09-01 17:40:57本地127不使用
					{
						watchServerIP = addr;
					}
				} catch (UnknownHostException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
					log("UnknownHostException e1 ...");
					log(e1.toString());
					return;
				}
			}
		}
		int ret = 0;

		log("先运行云服务器...");
		log("First  running cloud server..."); // 2018-09-01 18:21:24 英文显示
		log(String.format(" 云服务器IP=%s,  Port=%d...", watchServerIP, watchServerPort));
		log(String.format(" Cloud Server IP=%s,  Port=%d...", watchServerIP, watchServerPort)); // 2018-09-01 18:21:24
																								// 英文显示

		ret = WatchingServerRuning(watchServerIP, watchServerPort); // 服务器运行....
		if (ret == 0) {
			log("云服务器监控程序启动 失败...[请检查IP和端口 是否被占用]");
			log("Cloud Server Running Failed...[Port may be occupied.]"); // 2018-09-01 18:21:24 英文显示
			log("测试结束...");
			log("Test Complete"); // 2018-09-01 18:21:24 英文显示
			bStopRunning = true; // 2018-09-01 18:19:15 停止
			return;
		}

		log("测试开始...");
		log("Test Begin..."); // 2018-09-01 18:21:24 英文显示

		int dealtIndex = 0;
		long controllerSN = 0; // ;
		while (true) // 2018-09-02 15:58:16
		{
			if (WatchingServerHandler.arrSNReceived.size() > dealtIndex) {
				controllerSN = WatchingServerHandler.arrSNReceived.get(dealtIndex);
				if (WatchingServerHandler.isConnected(controllerSN)) {
					log(String.format("控制器SN = %d 连接************************ \r\n", controllerSN));
					log(String.format("Controller SN = %d  Connected************************ \r\n", controllerSN)); // 2018-09-01
																													// 18:21:24
																													// 英文显示
					dealtIndex = dealtIndex + 1; // 2018-07-14 07:40:16 等待下一个接入设备
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

	public static void log(String info) // 日志信息
	{
		DateFormat df = DateFormat.getTimeInstance();
		System.out.println("[" + df.format(new Date(System.currentTimeMillis())) + "]  " + info);
	}

	public static byte GetHex(int val) // 获取Hex值, 主要用于日期时间格式
	{
		return (byte) ((val % 10) + (((val - (val % 10)) / 10) % 10) * 16);
	}

	/// <summary>
	/// 显示记录信息
	/// </summary>
	/// <param name="recvBuff"></param>
	public static void displayRecordInformation(byte[] recvBuff) {

		// 8-11 最后一条记录的索引号
		// (=0表示没有记录) 4 0x00000000
		long recordIndex = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 8, 4);

		// 12 记录类型
		// 0=无记录
		// 1=刷卡记录
		// 2=门磁,按钮, 设备启动, 远程开门记录
		// 3=报警记录 1
		int recordType = WgUdpCommShort4Cloud.getIntByByte(recvBuff[12]);

		// 13 有效性(0 表示不通过, 1表示通过) 1
		int recordValid = WgUdpCommShort4Cloud.getIntByByte(recvBuff[13]);

		// 14 门号(1,2,3,4) 1
		int recordDoorNO = WgUdpCommShort4Cloud.getIntByByte(recvBuff[14]);

		// 15 进门/出门(1表示进门, 2表示出门) 1 0x01
		int recordInOrOut = WgUdpCommShort4Cloud.getIntByByte(recvBuff[15]);

		// 16-19 卡号(类型是刷卡记录时)
		// 或编号(其他类型记录) 4
		long recordCardNO = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 16, 4);
		recordCardNO = recordCardNO + (WgUdpCommShort4Cloud.getLongByByte(recvBuff, 44, 4) << 32);

		// 20-26 刷卡时间:
		// 年月日时分秒 (采用BCD码)见设置时间部分的说明
		String recordTime = String.format("%02X%02X-%02X-%02X %02X:%02X:%02X",
				WgUdpCommShort4Cloud.getIntByByte(recvBuff[20]), WgUdpCommShort4Cloud.getIntByByte(recvBuff[21]),
				WgUdpCommShort4Cloud.getIntByByte(recvBuff[22]), WgUdpCommShort4Cloud.getIntByByte(recvBuff[23]),
				WgUdpCommShort4Cloud.getIntByByte(recvBuff[24]), WgUdpCommShort4Cloud.getIntByByte(recvBuff[25]),
				WgUdpCommShort4Cloud.getIntByByte(recvBuff[26]));

		// 2012.12.11 10:49:59 7
		// 27 记录原因代码(可以查 "刷卡记录说明.xls"文件的ReasonNO)
		// 处理复杂信息才用 1
		int reason = WgUdpCommShort4Cloud.getIntByByte(recvBuff[27]);

		// 0=无记录
		// 1=刷卡记录
		// 2=门磁,按钮, 设备启动, 远程开门记录
		// 3=报警记录 1
		// 0xFF=表示指定索引位的记录已被覆盖掉了. 请使用索引0, 取回最早一条记录的索引值
		if (recordType == 0) {
			log(String.format("索引位=%u  无记录", recordIndex));
		} else if (recordType == 0xff) {
			log(" 指定索引位的记录已被覆盖掉了,请使用索引0, 取回最早一条记录的索引值");
		} else if (recordType == 1) // 2015-06-10 08:49:31 显示记录类型为卡号的数据
		{
			// 卡号
			log((String.format("索引位=%d  ", recordIndex)) + "\r\n" + (String.format("  卡号 = %d", recordCardNO)) + "\r\n"
					+ (String.format("  门号 = %d", recordDoorNO)) + "\r\n"
					+ (String.format("  进出 = %s", recordInOrOut == 1 ? "进门" : "出门")) + "\r\n"
					+ (String.format("  有效 = %s", recordValid == 1 ? "通过" : "禁止")) + "\r\n"
					+ (String.format("  时间 = %s", recordTime)) + "\r\n"
					+ (String.format("  描述 = %s", getReasonDetailChinese(reason))));
		} else if (recordType == 2) {
			// 其他处理
			// 门磁,按钮, 设备启动, 远程开门记录
			log((String.format("索引位=%d  非刷卡记录", recordIndex)) + "\r\n" + (String.format("  编号 = %d", recordCardNO))
					+ "\r\n" + (String.format("  门号 = %d", recordDoorNO)) + "\r\n"
					+ (String.format("  时间 = %s", recordTime)) + "\r\n"
					+ (String.format("  描述 = %s", getReasonDetailChinese(reason))));
		} else if (recordType == 3) {
			// 其他处理
			// 报警记录
			log((String.format("索引位=%d  报警记录", recordIndex)) + "\r\n" + (String.format("  编号 = %d", recordCardNO))
					+ "\r\n" + (String.format("  门号 = %d", recordDoorNO)) + "\r\n"
					+ (String.format("  时间 = %s", recordTime)) + "\r\n"
					+ (String.format("  描述 = %s", getReasonDetailChinese(reason))));
		}
	}

	public static String RecordDetails[] = {
			// 记录原因 (类型中 SwipePass 表示通过; SwipeNOPass表示禁止通过; ValidEvent 有效事件(如按钮 门磁 超级密码开门);
			// Warn 报警事件)
			// 代码 类型 英文描述 中文描述
			"1", "SwipePass", "Swipe", "刷卡开门", "2", "SwipePass", "Swipe Close", "刷卡关", "3", "SwipePass", "Swipe Open",
			"刷卡开", "4", "SwipePass", "Swipe Limited Times", "刷卡开门(带限次)", "5", "SwipeNOPass",
			"Denied Access: PC Control", "刷卡禁止通过: 电脑控制", "6", "SwipeNOPass", "Denied Access: No PRIVILEGE",
			"刷卡禁止通过: 没有权限", "7", "SwipeNOPass", "Denied Access: Wrong PASSWORD", "刷卡禁止通过: 密码不对", "8", "SwipeNOPass",
			"Denied Access: AntiBack", "刷卡禁止通过: 反潜回", "9", "SwipeNOPass", "Denied Access: More Cards", "刷卡禁止通过: 多卡",
			"10", "SwipeNOPass", "Denied Access: First Card Open", "刷卡禁止通过: 首卡", "11", "SwipeNOPass",
			"Denied Access: Door Set NC", "刷卡禁止通过: 门为常闭", "12", "SwipeNOPass", "Denied Access: InterLock", "刷卡禁止通过: 互锁",
			"13", "SwipeNOPass", "Denied Access: Limited Times", "刷卡禁止通过: 受刷卡次数限制", "14", "SwipeNOPass",
			"Denied Access: Limited Person Indoor", "刷卡禁止通过: 门内人数限制", "15", "SwipeNOPass",
			"Denied Access: Invalid Timezone", "刷卡禁止通过: 卡过期或不在有效时段", "16", "SwipeNOPass", "Denied Access: In Order",
			"刷卡禁止通过: 按顺序进出限制", "17", "SwipeNOPass", "Denied Access: SWIPE GAP LIMIT", "刷卡禁止通过: 刷卡间隔约束", "18",
			"SwipeNOPass", "Denied Access", "刷卡禁止通过: 原因不明", "19", "SwipeNOPass", "Denied Access: Limited Times",
			"刷卡禁止通过: 刷卡次数限制", "20", "ValidEvent", "Push Button", "按钮开门", "21", "ValidEvent", "Push Button Open", "按钮开",
			"22", "ValidEvent", "Push Button Close", "按钮关", "23", "ValidEvent", "Door Open", "门打开[门磁信号]", "24",
			"ValidEvent", "Door Closed", "门关闭[门磁信号]", "25", "ValidEvent", "Super Password Open Door", "超级密码开门", "26",
			"ValidEvent", "Super Password Open", "超级密码开", "27", "ValidEvent", "Super Password Close", "超级密码关", "28",
			"Warn", "Controller Power On", "控制器上电", "29", "Warn", "Controller Reset", "控制器复位", "30", "Warn",
			"Push Button Invalid: Disable", "按钮不开门: 按钮禁用", "31", "Warn", "Push Button Invalid: Forced Lock",
			"按钮不开门: 强制关门", "32", "Warn", "Push Button Invalid: Not On Line", "按钮不开门: 门不在线", "33", "Warn",
			"Push Button Invalid: InterLock", "按钮不开门: 互锁", "34", "Warn", "Threat", "胁迫报警", "35", "Warn", "Threat Open",
			"胁迫报警开", "36", "Warn", "Threat Close", "胁迫报警关", "37", "Warn", "Open too long", "门长时间未关报警[合法开门后]", "38",
			"Warn", "Forced Open", "强行闯入报警", "39", "Warn", "Fire", "火警", "40", "Warn", "Forced Close", "强制关门", "41",
			"Warn", "Guard Against Theft", "防盗报警", "42", "Warn", "7*24Hour Zone", "烟雾煤气温度报警", "43", "Warn",
			"Emergency Call", "紧急呼救报警", "44", "RemoteOpen", "Remote Open Door", "操作员远程开门", "45", "RemoteOpen",
			"Remote Open Door By USB Reader", "发卡器确定发出的远程开门" };

	public static String getReasonDetailChinese(int Reason) // 中文
	{
		if (Reason > 45) {
			return "";
		}
		if (Reason <= 0) {
			return "";
		}
		return RecordDetails[(Reason - 1) * 4 + 3]; // 中文信息
	}

	public static String getReasonDetailEnglish(int Reason) // 英文描述
	{
		if (Reason > 45) {
			return "";
		}
		if (Reason <= 0) {
			return "";
		}
		return RecordDetails[(Reason - 1) * 4 + 2]; // 英文信息
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

	public static int WatchingServerRuning(String serverIP, int serverPort) // 进入服务器监控状态
	{

		// 创建UDP数据包NIO
		NioDatagramAcceptor acceptor = new NioDatagramAcceptor();
		// NIO设置底层IOHandler
		acceptor.setHandler(new WatchingServerHandler(queue, UDPQueue4Mobile, RemoteEndPointQueue4Mobile,
				UDPQueue4RemoteOpenDoor, RemoteEndPointQueue4RemoteOpenDoor));

		// 设置是否重用地址？ 也就是每个发过来的udp信息都是一个地址？
		DatagramSessionConfig dcfg = acceptor.getSessionConfig();
		dcfg.setReuseAddress(true);

		// 绑定端口地址
		try {
			acceptor.bind(new InetSocketAddress(serverIP, serverPort));
		} catch (IOException e) {
			log("绑定接收服务器失败....");
			log("Bind DataServer Failed...."); // 2018-09-01 18:21:24 英文显示
			e.printStackTrace();
			return 0;
		}
		log("进入接收服务器监控状态....[如果在win7下使用 一定要注意防火墙设置]");
		log("Watching Server Started...."); // 2018-09-01 18:21:24 英文显示

		// To monitor if receive Msg from Server
		new Thread() {
			@Override
			public void run() {
				long recordIndex = 0;
				// 2018-09-01 18:20:06 while(true)
				while (!bStopRunning) // 2018-09-01 18:19:15 停止
				{
					boolean bDealt = false;
					if (!queue.isEmpty()) {
						bDealt = true;
						byte[] recvBuff;
						synchronized (queue) {
							recvBuff = queue.poll();
						}
						{
							if ((recvBuff[1] == 0x20)) {
								long sn = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 4, 4);
								if (WgUdpCommShort4Cloud.GetControllerType(sn) > 0) // 2018-03-21 12:00:12 有效的序列号
								{
									long recordIndexGet = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 8, 4);
									log(String.format("接收到来自控制器SN = %d 的数据包..记录索引号=%d##########", sn, recordIndexGet));
									log(String.format(
											"Receve Data From Controlller SN = %d.. RecordIndex=%d..##########", sn,
											recordIndexGet)); // 2018-10-16 18:21:03 增加记录索引号 2018-09-01 18:21:24 英文显示

									int iget = WatchingServerHandler.arrSNReceived.indexOf((int) sn);

									if (iget >= 0) {

										if (WatchingServerHandler.arrControllerInfo.size() >= iget) {
											recordIndex = (long) WatchingServerHandler.arrRecordIndex.get(iget);
											WatchingServerHandler.arrRecordIndex.set(iget, (int) recordIndexGet); // 2017-09-07
																													// 11:23:50
																													// 保存新值
											wgControllerInfo info = WatchingServerHandler.arrControllerInfo.get(iget);
											if (info != null) {
												recordIndex = info.recordIndex4WatchingRemoteOpen;
												info.recordIndex4WatchingRemoteOpen = recordIndexGet;
												// if (recordIndex < recordIndexGet)
												if ((recordIndex < recordIndexGet) // 2017-12-12 17:38:25 获取到记录索引 大于
																					// 已获取的记录索引号
														|| ((recordIndexGet - recordIndex) < -5)) // 新的记录索引 比
																									// 已获取的记录索引值小,
																									// 相差大于5条[则取新的](设备的记录索引号可能复位造成)
												{
													recordIndex = recordIndexGet;
													displayRecordInformation(recvBuff);
													int recordType = WgUdpCommShort4Cloud.getIntByByte(recvBuff[12]);
													int recordValid = WgUdpCommShort4Cloud.getIntByByte(recvBuff[13]);

													// 只针对不能开门的刷卡信息 进行远程开门操作
//													if ((recordType == 1) // 2015-06-10 08:49:31 显示记录类型为卡号的数据
//															&& (recordValid == 0)) // 2017-09-07 10:56:00 禁止通过时
//													{
//														// 14 门号(1,2,3,4) 1
//														int recordDoorNO = recvBuff[14];
//														// 卡号(类型是刷卡记录时) //或编号(其他类型记录) 4
//														long recordCardNO = WgUdpCommShort4Cloud.getLongByByte(recvBuff,
//																16, 4);
//														recordCardNO = recordCardNO + (WgUdpCommShort4Cloud
//																.getLongByByte(recvBuff, 44, 4) << 32);
//														log(String.format("收到刷卡信息 发出 模拟卡号远程开门 指令\r\n"));
//														Thread thread = new opendDoorThread((int) sn, recordDoorNO,
//																watchServerIP, watchServerPort, recordCardNO);
//														thread.start();
//
//													}
												}

											}
										}
									}
								}
							} else if ((recvBuff[1] == 0x22)) // 2018-07-10 10:39:43 增加0x22
							{
								long sn = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 4, 4);
//								long recordIndexGet = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 8, 4);
								log(String.format("接收到来自控制器SN = %d 的二维码数据包....", sn));
								log(String.format("Receive QR Data From Controller SN = %d....", sn)); // 2018-09-01
																										// 18:21:24 英文显示

								// 2018-12-11 16:02:01 新增
								// 8-11 二维码数据长度
								// (=0表示没有记录) 4 0x00000000
								long qrDataLen = 0;
								qrDataLen = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 8, 4);
								int doorNO = recvBuff[14];
								long cmdSequenceId = WgUdpCommShort4Cloud.getLongByByte(recvBuff, 40, 4); // 2017-09-07
																											// 15:56:06
																											// 流水号
								if (qrDataLen >= 1) // 2017-09-07 15:49:29 有二维码数据
								{
									byte[] qrData = new byte[(int) qrDataLen];
									// Array.Copy(recvBuff, 64, qrData, 0, qrDataLen); //数据
									for (int i = 0; i < qrDataLen; i++) {
										qrData[i] = (byte) recvBuff[64 + i];
									}

									log(String.format("流水号=%d 二维码原始数据:\r\n        %s\r\n", cmdSequenceId,
											toHex(qrData)));

									// 转换为字符串数据
									log(String.format("流水号=%d 二维码原始数据(转换为字符串):\r\n        %s", cmdSequenceId,
											new String(qrData)));

									// 2018-12-11 16:43:24 如下是远程开门动作部分
									{
//										int iloc = -1;
//										iloc = WatchingServerHandler.arrSNReceived.indexOf((int) sn);
//										if ((iloc) >= 0) {
////											// 远程开门
////											log(String.format("收到二维码透传信息 发出 模拟卡号远程开门 指令\r\n"));
////											long card4qr = 123456789777l; // 模拟卡号
////											Thread thread = new opendDoorThread((int) sn, doorNO, watchServerIP,
////													watchServerPort, card4qr);
////											thread.start();
//										}
//									}
//								}
//							}
//						}
//					}
//					if (DealRuninfoPacketProc4Mobile()) {
//						bDealt = true;
//					}
//					if (DealRuninfoPacketProc4RemoteOpenDoor()) {
//						bDealt = true;
//					}
//					if (!bDealt) {
//						long times = 100;
//						try {
//							Thread.sleep(times);
//						} catch (InterruptedException e) {
//							// TODO Auto-generated catch block
//							e.printStackTrace();
//						} // 2017-11-01 14:45:57 增加延时
//					}
//				}
//			}
//		}.start(); // 2018-07-10 10:35:14
//		return 1;
	}

	static ArrayList<Long> arrXIDSend = new ArrayList<Long>();
	static ArrayList<Long> arrXIDReceivedMobile = new ArrayList<Long>();
	static ArrayList<IoSession> arrXIDEndPointMobile = new ArrayList<IoSession>();
	static long dtXIDSendLast = System.currentTimeMillis(); // DateTime.Now.Ticks;

	static Queue<byte[]> UDPQueue4RemoteOpenDoor = new LinkedList<byte[]>(); // 2018-09-02 10:35:40
	static Queue<IoSession> RemoteEndPointQueue4RemoteOpenDoor = new LinkedList<IoSession>(); // 2018-09-02 10:35:40

//	private static boolean DealRuninfoPacketProc4Mobile() // 2015-06-22 21:11:18处理来自手机的指令数据[主要是远程开门 或许还有加入数据]
//	{
//		byte[] recv;
//		boolean ret = false;
//		try {
//			do {
//				if (!UDPQueue4Mobile.isEmpty()) {
//					// 取数据
//					IoSession senderRemote;
//					synchronized (UDPQueue4Mobile) {
//						recv = (byte[]) UDPQueue4Mobile.poll();
//						senderRemote = (IoSession) RemoteEndPointQueue4Mobile.poll();
//					}
//					// 2019-05-19 09:42:13 if ((recv.length == 64) && ((recv[0] == 0x37)))
//					// //2018-02-12 10:20:51 手机专用指令
//					if (((recv.length % 64) == 0) && ((recv[0] == 0x37))) // 2019-05-19 09:42:30 2018-02-12 10:20:51
//																			// 手机专用指令
//					{
//						long m_ControllerSN = WgUdpCommShort4Cloud.getLongByByte(recv, 4, 4);
//						int iloc = -1;
//						if ((iloc = WatchingServerHandler.arrSNReceived.indexOf((int) m_ControllerSN)) >= 0) {
//							// 直接将数据转发出去
//							recv[0] = 0x17; // 2018-02-12 10:22:05 修改
//
//							long oldxid = WgUdpCommShort4Cloud.getXidOfCommand(recv);
//							long newxid = WgUdpCommShort4Cloud.getXid();
//							// 产生唯一流水号
//							System.arraycopy(WgUdpCommShort4Cloud.longToByte(newxid), 0, recv, 40, 4);
//							synchronized (arrXIDSend) {
//								arrXIDSend.add(newxid); // 新的
//								arrXIDReceivedMobile.add(oldxid); // 旧的
//								arrXIDEndPointMobile.add(senderRemote); // 2015-06-21 15:34:08 指令源
//							}
//							if (recv.length == 1024) // >64) //支持1024字节指令
//							{
//								for (int j = 0; j < recv.length; j = j + 64) {
//									if ((recv[j] == 0x37)) {
//										recv[j] = 0x17;
//										System.arraycopy(WgUdpCommShort4Cloud.longToByte(newxid + j / 64), 0, recv,
//												j + 40, 4); // 流水号
//									}
//								}
//							}
//
//							dtXIDSendLast = System.currentTimeMillis() + 2 * 60 * 1000; // DateTime.Now.Ticks + 2 * 60 *
//																						// 1000 * 1000 * 10;
//																						// //2015-06-22 13:16:32 2分钟
//																						// Date.Now.Ticks 'us
//
//							UDP_OnlySend(recv, WatchingServerHandler.arrControllerInfo.get(iloc).Session); // 2018-02-12
//																											// 17:02:35
//						}
//					}
//					ret = true;
//
//				} else {
//					if (!arrXIDSend.isEmpty()) {
//						if (dtXIDSendLast < System.currentTimeMillis()) // DateTime.Now.Ticks)
//						{
//							// 2015-06-22 10:57:06 如果2分钟内没有接收到手机指令 清空数据信息...
//							synchronized (arrXIDSend) {
//								arrXIDSend.clear(); // 新的
//								arrXIDReceivedMobile.clear(); // 旧的
//								arrXIDEndPointMobile.clear(); // 2015-06-21 15:34:08 指令源
//							}
//							ret = true;
//						}
//					}
//				}
//			} while (false); // (!bUDPListenStop);
//
//		} catch (Exception e) {
//
//			// Debug.WriteLine(e.ToString());
//		}
//		return ret;
//	}

	private static boolean DealRuninfoPacketProc4RemoteOpenDoor() // 2019-05-19 09:39:24 来自客户端的指令 2015-06-22 21:10:46
																	// 处理远程开门指令的返回数据[来自控制器]
	{
		byte[] recv;
		boolean ret = false;
		try {
			do {
				if (!UDPQueue4RemoteOpenDoor.isEmpty()) {
					// 取数据
					synchronized (UDPQueue4RemoteOpenDoor) {
						recv = (byte[]) UDPQueue4RemoteOpenDoor.poll();
					}
//2019-05-19 09:39:19					if ((recv.length == 64) && ((recv[1] == 0x40))) //2015-06-24 20:18:12 手机指令  远程开门
//					if ((recv.length % 64) == 0) // 2015-06-24 20:18:12 手机指令 远程开门
//					{
//						long oldxid = WgUdpCommShort4Cloud.getXidOfCommand(recv);
//						int iloc = -1;
//
//						if ((iloc = arrXIDSend.indexOf(oldxid)) >= 0) {
//							recv[0] = 0x37; // 2018-02-12 17:08:53 手机发出的是0x37
//
//							// 直接将数据转发出去
//							long newxid = (long) arrXIDReceivedMobile.get(iloc);
//							IoSession senderRemote = (IoSession) arrXIDEndPointMobile.get(iloc);
//							System.arraycopy(WgUdpCommShort4Cloud.longToByte(newxid), 0, recv, 40, 4);
//							synchronized (arrXIDSend) // 处理过的清除掉...
//							{
//								arrXIDEndPointMobile.remove(iloc);
//								arrXIDReceivedMobile.remove(iloc);
//								arrXIDSend.remove(iloc);
//							}
//							if (recv.length == 1024) // >64) //支持1024字节指令
//							{
//								for (int j = 0; j < recv.length; j = j + 64) {
//									if ((recv[j] == 0x17)) {
//										recv[j] = 0x37;
//										System.arraycopy(WgUdpCommShort4Cloud.longToByte(newxid + j / 64), 0, recv,
//												j + 40, 4); // 流水号
//									}
//								}
//							}
//							UDP_OnlySend(recv, senderRemote); // 2018-02-12 17:02:35
//						}
//					}
					ret = true;
				} 
			} while (false); // (!bUDPListenStop);

		} catch (Exception e) {

			// Debug.WriteLine(e.ToString());
		}
		return ret;
	}

	/// <summary>
	/// 只发送数据
	/// </summary>
	/// <param name="cmd">64字节指令</param>
	/// <param name="sequenceId">流水号(如果为0, 采用系统默认)</param>
	/// <returns></returns>
	public static long UDP_OnlySend(byte[] bytCommand, IoSession session) // 只发送数据包
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

//	public static class opendDoorThread extends Thread // 2019-04-13 09:14:06 远程开门线程
//	{
//		// private String name;
//		int sn;
//		int doorNO;
//		String IP;
//		int port;
//		long cardno;
//
//		public opendDoorThread(int sn, int doorNO, String IP, int port, long cardno) {
//			this.sn = sn;
//			this.doorNO = doorNO;
//			this.IP = IP;
//			this.port = port;
//			this.cardno = cardno;
//		}

//		private static int tag = 0x70000001; // 手机远程开门时使用流水号
//
//		public void run() {
//			// System.out.println("hello " + name);
//			int ret = -13;
//			byte content[] = null;
//			DatagramPacket snddataPacket;
//			int tries = 3; // 重试操作
//			while (tries-- > 0) {
//				try {
//					int controllerPort = port; // 60000;
//					byte[] byteCmd = new byte[] { (byte) 0x37, (byte) 0x40, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00,
//							(byte) 0x00, (byte) 0x00, (byte) 0x00 };
//
//					byteCmd[0] = 0x37; // 2018-03-06 13:15:55 通过云服务器对控制器操作 [云服务器上运行服务程序]
//					byteCmd[4] = (byte) (sn & 0xff);
//					byteCmd[5] = (byte) ((sn >> 8) & 0xff);
//					byteCmd[6] = (byte) ((sn >> 16) & 0xff);
//					byteCmd[7] = (byte) ((sn >> 24) & 0xff);
//
//					byteCmd[8] = (byte) doorNO;
//
//					byteCmd[20] = (byte) (cardno & 0xff);
//					byteCmd[20 + 1] = (byte) ((cardno >> 8) & 0xff);
//					byteCmd[20 + 2] = (byte) ((cardno >> 16) & 0xff);
//					byteCmd[20 + 3] = (byte) ((cardno >> 24) & 0xff);
//					byteCmd[28] = 0x00; // [模拟刷卡的操作] =0x00 表示进门的请求 =0x01 表示出门的请求
//					byteCmd[32] = 0x5A; // 不受设备内的权限约束. 设为0则受设备内的权限约束
//
//					byteCmd[40] = (byte) (tag & 0xff);
//					byteCmd[41] = (byte) ((tag >> 8) & 0xff);
//					byteCmd[42] = (byte) ((tag >> 16) & 0xff);
//					byteCmd[43] = (byte) ((tag >> 24) & 0xff);
//
//					content = null;
//					DatagramSocket dataSocket = new DatagramSocket();
//					dataSocket.setSoTimeout(1000);
//
//					snddataPacket = new DatagramPacket((byteCmd), byteCmd.length, InetAddress.getByName(IP),
//							controllerPort);
//					dataSocket.send(snddataPacket);
//
//					byte recvDataByte[] = new byte[64];
//
//					Thread.sleep(200);
//					DatagramPacket dataPacket = new DatagramPacket(recvDataByte, recvDataByte.length);
//					dataSocket.receive(dataPacket);
//
//					content = dataPacket.getData();
//					dataSocket.close();
//				} catch (NumberFormatException exNum) {
//					exNum.printStackTrace();
//				}
//
//				catch (UnknownHostException e1) {
//					e1.printStackTrace();
//					// return ret;
//				} catch (SocketException e) {
//					e.printStackTrace();
//				} catch (IOException e) {
//					e.printStackTrace();
//				} catch (InterruptedException e) {
//					e.printStackTrace();
//				} finally {
//
//				}
//
//				if ((content != null) && (content.length == 64)) {
//					ret = content[8];
//				} else {
//					ret = -13;
//				}
//				if (ret >= 0) // 2019-04-13 10:01:07 有数据通信则不用重试
//				{
//					break;
//				}
//			}
//			tag++;
//			if (tag >= 0x7fffffff) {
//				tag = 0x70000001; // 2018-02-13 19:06:22
//			}
//
//			if (ret > 0) {
//				// 远程开门
//				log(String.format("sn = %d opendDoorThread Successful\r\n", sn));
//
//			} else {
//				log(String.format("sn = %d opendDoorThread Failed\r\n", sn));
//
//			}
//
//		}
//	}
//}
