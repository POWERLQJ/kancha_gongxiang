using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace QRCodeACSWebServer
{
	public abstract class HttpServer
	{
		protected int port;

		private TcpListener listener;

		public bool is_active = true;

		protected WriteStringData writestring;

		public WriteStringData WriteString
		{
			get
			{
				return this.writestring;
			}
			set
			{
				this.writestring = (WriteStringData)Delegate.Combine(this.writestring, value);
			}
		}

		public HttpServer()
		{
		}

		public HttpServer(int port)
		{
			this.port = port;
		}

		public void listen()
		{
			try
			{
				this.listener = new TcpListener(this.port);
				this.listener.Start();
				while (this.is_active)
				{
					TcpClient s = this.listener.AcceptTcpClient();
					HttpProcessor @object = new HttpProcessor(s, this, this.writestring);
					Thread thread = new Thread(new ThreadStart(@object.process));
					thread.Start();
					Thread.Sleep(1);
				}
			}
			catch (Exception)
			{
			}
		}

		public abstract void handleGETRequest(HttpProcessor p);

		public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
	}
}
