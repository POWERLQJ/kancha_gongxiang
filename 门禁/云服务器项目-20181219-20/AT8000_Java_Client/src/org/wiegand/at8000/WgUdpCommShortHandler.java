package org.wiegand.at8000;

import java.util.Queue;

import org.apache.mina.core.buffer.IoBuffer;
import org.apache.mina.core.service.IoHandler;
import org.apache.mina.core.session.IdleStatus;
import org.apache.mina.core.session.IoSession;

public class WgUdpCommShortHandler implements IoHandler {

	public Queue<byte[]> queue;

	public WgUdpCommShortHandler() {
	}

	public WgUdpCommShortHandler(Queue<byte[]> queue) {
		super();
		this.queue = queue;
	}

	public void exceptionCaught(IoSession session, Throwable e)
			throws Exception {
		e.printStackTrace();
		session.close(true);
	}

	public void messageReceived(IoSession session, Object message)
			throws Exception {
		IoBuffer io = (IoBuffer) message;
		if (io.hasRemaining())
		{
			byte[] validBytes = new byte[io.remaining()];
			io.get(validBytes,0,io.remaining());
//2019-05-19 16:48:26			if ((validBytes.length == WgUdpCommShort4CloudClient.WGPacketSize)
//					&& (validBytes[0] == WgUdpCommShort4CloudClient.Type))  //型号固定
		if (((validBytes.length == WgUdpCommShort4CloudClient.WGPacketSize)
			||	(validBytes.length == WgUdpCommShort4CloudClient.WGPacket1024Size)) //2019-05-19 16:48:52 增加1024
							&& (validBytes[0] == WgUdpCommShort4CloudClient.Type))  //型号固定
			{
				synchronized (queue)
		         {
  				   queue.offer(validBytes);
		         }
			}
			else
			{
				//System.out.print("收到无效数据包: ????\r\n");
			}
			//System.out.println("");
		}
	}

	public void messageSent(IoSession session, Object message) throws Exception {

	}

	public void sessionClosed(IoSession session) throws Exception {

	}

	public void sessionCreated(IoSession session) throws Exception {

	}

	public void sessionIdle(IoSession session, IdleStatus idle)
			throws Exception {

	}

	public void sessionOpened(IoSession session) throws Exception {

	}

}
