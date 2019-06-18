package org.wiegand.TestZone;

import java.util.ArrayList;
//import java.util.Date;
import java.util.LinkedList;
import java.util.Queue;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.service.IoHandlerAdapter;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.core.session.IoSession;
import org.wiegand.at8000.WgUdpCommShort4Cloud;
import org.wiegand.at8000.wgControllerInfo;


    
    /**
     * Class the extends IoHandlerAdapter in order to properly handle
     * connections and the data the connections send
     *
     * @author <a href="http://mina.apache.org" mce_href="http://mina.apache.org">Apache MINA Project</a>
     */
    public class WatchingServerHandler extends IoHandlerAdapter {

    	private Queue<byte[]> queue;
    	private Queue<byte[]> UDPQueue4Mobile;  //2018-09-02 10:35:40 
    	private Queue<IoSession> RemoteEndPointQueue4Mobile;  //2018-09-02 10:35:40 
    	private Queue<byte[]> UDPQueue4RemoteOpenDoor;  //2018-09-02 10:35:40 
    	private Queue<IoSession> RemoteEndPointQueue4RemoteOpenDoor;  //2018-09-02 10:35:40 
       public WatchingServerHandler(Queue<byte[]> queue) {
    		super();
    		this.queue = queue;
    	}
  
        public WatchingServerHandler(Queue<byte[]> queue,Queue<byte[]> UDPQueue4Mobile,Queue<IoSession> RemoteEndPointQueue4Mobile
        		,Queue<byte[]> UDPQueue4RemoteOpenDoor,Queue<IoSession> RemoteEndPointQueue4RemoteOpenDoor) {
    		super();
    		this.queue = queue;
    		this.RemoteEndPointQueue4Mobile = RemoteEndPointQueue4Mobile;
    		this.UDPQueue4Mobile = UDPQueue4Mobile;
       		this.UDPQueue4RemoteOpenDoor = UDPQueue4RemoteOpenDoor;
    		this.RemoteEndPointQueue4RemoteOpenDoor = RemoteEndPointQueue4RemoteOpenDoor;
  	}
        /**
         * 异常来关闭session
         */
        @Override
        public void exceptionCaught(IoSession session, Throwable cause)
                throws Exception {
            cause.printStackTrace();
            session.close(true);
        }

        public  static   ArrayList<Integer> arrSNReceived = new ArrayList<Integer>();
        public  static   ArrayList<Integer> arrRecordIndex = new ArrayList<Integer>();
   public  static   ArrayList<wgControllerInfo> arrControllerInfo = new ArrayList<wgControllerInfo>(); //2017-12-24 14:58:44 
   public  static   Queue<byte[]> queueApp= new LinkedList<byte[]>(); //2018-07-11 16:05:33 应用队列
   
       public static boolean isConnected(long controllerSN)  //检查控制器是否连接
       {
    	   int iget = arrSNReceived.indexOf((int) controllerSN);
	   		if (iget >= 0) {
	   			if (arrControllerInfo.size()>=iget)
	   			{
	   			wgControllerInfo info = arrControllerInfo.get(iget);
	   			if (info != null)
	   			{
	   				if ((info.UpdateDateTime + 5*60*1000) > System.currentTimeMillis()) //在5分钟以内
	   				{
	   				   return true;
	   				}
	   			}
	   			}
	   		}
    	   return false;
       }
       
       /**
         * 服务器端收到一个消息
         */
        @Override
        public void messageReceived(IoSession session, Object message)
                throws Exception {

        	IoBuffer io = (IoBuffer) message;
    		if (io.hasRemaining())
    		{
    			byte[] validBytes = new byte[io.remaining()];
    			io.get(validBytes,0,io.remaining());
        		if (((validBytes.length == WgUdpCommShort4Cloud.WGPacketSize)
            			|| ((validBytes.length % WgUdpCommShort4Cloud.WGPacketSize) ==0)) //2018-07-10 14:30:38 引入64的倍数, 用于二维码
                        &&(validBytes.length>0)
            			&& ((validBytes[0] == WgUdpCommShort4Cloud.Type)||(validBytes[0] == 0x37)))  //型号固定
    			{
//2019-05-19 09:08:06     				 if ((validBytes.length == 64) && ((validBytes[0] == 0x37)) && ((validBytes[1]) == 0x40)) //2015-06-24 15:07:51 表示手机专用指令 手机专用指令 且是远程开门
         		    if ((((validBytes.length % WgUdpCommShort4Cloud.WGPacketSize) ==0)) && ((validBytes[0] == 0x37))) //2019-05-19 09:08:15 转换指令使用 //2015-06-24 15:07:51 表示手机专用指令 手机专用指令 且是远程开门
                     {
                         //UDPQueue4Mobile
     					synchronized (UDPQueue4Mobile)         //加入同步安全机制
                         {
                             UDPQueue4Mobile.offer(validBytes);
                             RemoteEndPointQueue4Mobile.offer(session);
                         }
     					return;
                     }
     			    //2019-05-19 09:10:00 else if ((validBytes.length == 64) && ((validBytes[0] == 0x17)) && ((validBytes[1]) == 0x40) && ((validBytes[43] & 0xf0)==0x70)) //2018-02-12 17:21:57流水号为0x70000000-0x7FFFFFFF 2015-06-24 15:12:40 控制器的手机回复指令
         			else if ((((validBytes.length % WgUdpCommShort4Cloud.WGPacketSize) ==0)) && ((validBytes[0] == 0x17))  && ((validBytes[43] & 0xf0)==0x70)) //2019-05-19 09:10:13 转发指令 //2018-02-12 17:21:57流水号为0x70000000-0x7FFFFFFF 2015-06-24 15:12:40 控制器的手机回复指令
                    {
     			    	synchronized (UDPQueue4RemoteOpenDoor)         //加入同步安全机制
                        {
                            UDPQueue4RemoteOpenDoor.offer(validBytes);
                            RemoteEndPointQueue4RemoteOpenDoor.offer(session);
                        }
     			    	return;
                    }
   				     synchronized (queue)
    		         {
      				   queue.offer(validBytes);
    		         }
    				 long sn = WgUdpCommShort4Cloud.getLongByByte(validBytes, 4, 4);
    				 int iget = arrSNReceived.indexOf((int)sn);
		            if (iget < 0)
		            {
		            	if (WgUdpCommShort4Cloud.GetControllerType(sn) > 0)  //2018-03-21 12:00:12 有效的序列号
		            	{
		                arrSNReceived.add((int) sn);
		                long recordIndexGet = WgUdpCommShort4Cloud.getLongByByte(validBytes, 8, 4);
						WatchingServerHandler.arrRecordIndex.add((int) recordIndexGet); //2017-09-07 11:23:50 保存新值
                         				   		
		                wgControllerInfo con = new wgControllerInfo();	
		                con.update((int)sn, session,  validBytes);
		                arrControllerInfo.add(con); //2017-12-24 15:01:29
		            	}
  		            }
		            else
		            {
                //2015-06-13 15:00:41 更新操作
	    		        arrControllerInfo.get(iget).update((int)sn, session, validBytes);
		            }
    			}
    			else
    			{
    				//System.out.print("收到无效数据包: ????\r\n");
    			}
    			//System.out.println("");
    		}
        }

        @Override
        public void sessionClosed(IoSession session) throws Exception {
//            System.out.println("服务器端关闭session...");
        }

        @Override
        public void sessionCreated(IoSession session) throws Exception {
//            System.out.println("服务器端成功创建一个session...");
        }

        @Override
        public void sessionIdle(IoSession session, IdleStatus status)
                throws Exception {
           //  System.out.println("Session idle...");
        }

        @Override
        public void sessionOpened(IoSession session) throws Exception {
//            System.out.println("服务器端成功开启一个session...");
        }
    }
