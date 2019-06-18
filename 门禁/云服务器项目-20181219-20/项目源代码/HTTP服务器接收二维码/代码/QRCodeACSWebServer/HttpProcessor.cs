using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace QRCodeACSWebServer
{
	public class HttpProcessor
	{
		private const int BUF_SIZE = 4096;

		public WriteStringData writestring;

		public TcpClient socket;

		public HttpServer srv;

		private Stream inputStream;

		public StreamWriter outputStream;

		public string http_method;

		public string http_url;

		public string http_protocol_versionstring;

		public Hashtable httpHeaders = new Hashtable();

		private static int MAX_POST_SIZE = 10485760;

		public HttpProcessor(TcpClient s, HttpServer srv, WriteStringData write)
		{
			this.socket = s;
			this.srv = srv;
			this.writestring = (WriteStringData)Delegate.Combine(this.writestring, write);
		}

		private void writeLine(string line)
		{
			if (this.writestring != null)
			{
				this.writestring(line);
			}
		}

		private string streamReadLine(Stream inputStream)
		{
			string text = "";
			while (true)
			{
				int num = inputStream.ReadByte();
				if (num == 10)
				{
					break;
				}
				if (num != 13)
				{
					if (num == -1)
					{
						Thread.Sleep(1);
					}
					else
					{
						text += Convert.ToChar(num);
					}
				}
			}
			return text;
		}

		public void process()
		{
			this.inputStream = new BufferedStream(this.socket.GetStream());
			this.outputStream = new StreamWriter(new BufferedStream(this.socket.GetStream()));
			try
			{
				this.parseRequest();
				this.readHeaders();
				if (this.http_method.Equals("GET"))
				{
					this.handleGETRequest();
				}
				else if (this.http_method.Equals("POST"))
				{
					this.handlePOSTRequest();
				}
			}
			catch (Exception ex)
			{
				this.writeLine("Exception: " + ex.ToString());
				this.writeFailure();
			}
			this.outputStream.Flush();
			this.inputStream = null;
			this.outputStream = null;
			this.socket.Close();
		}

		public void parseRequest()
		{
			string text = this.streamReadLine(this.inputStream);
			string[] array = text.Split(new char[]
			{
				' '
			});
			if (array.Length != 3)
			{
				throw new Exception("invalid http request line");
			}
			this.http_method = array[0].ToUpper();
			this.http_url = array[1];
			this.http_protocol_versionstring = array[2];
			this.writeLine("starting: " + text);
		}

		public void readHeaders()
		{
			this.writeLine("readHeaders()");
			string text;
			while ((text = this.streamReadLine(this.inputStream)) != null)
			{
				if (text.Equals(""))
				{
					this.writeLine("got headers");
					return;
				}
				int num = text.IndexOf(':');
				if (num == -1)
				{
					throw new Exception("invalid http header line: " + text);
				}
				string text2 = text.Substring(0, num);
				int num2 = num + 1;
				while (num2 < text.Length && text[num2] == ' ')
				{
					num2++;
				}
				string text3 = text.Substring(num2, text.Length - num2);
				this.writeLine(string.Format("header: {0}:{1}", text2, text3));
				this.httpHeaders[text2] = text3;
			}
		}

		public void handleGETRequest()
		{
			this.srv.handleGETRequest(this);
		}

		public void handlePOSTRequest()
		{
			this.writeLine("get post data start");
			MemoryStream memoryStream = new MemoryStream();
			if (this.httpHeaders.ContainsKey("Content-Length"))
			{
				int num = Convert.ToInt32(this.httpHeaders["Content-Length"]);
				if (num > HttpProcessor.MAX_POST_SIZE)
				{
					throw new Exception(string.Format("POST Content-Length({0}) too big for this simple server", num));
				}
				byte[] buffer = new byte[4096];
				int i = num;
				while (i > 0)
				{
					this.writeLine(string.Format("starting Read, to_read={0}", i));
					int num2 = this.inputStream.Read(buffer, 0, Math.Min(4096, i));
					this.writeLine(string.Format("read finished, numread={0}", num2));
					if (num2 == 0)
					{
						if (i != 0)
						{
							throw new Exception("client disconnected during post");
						}
						break;
					}
					else
					{
						i -= num2;
						memoryStream.Write(buffer, 0, num2);
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
			}
			this.writeLine("get post data end");
			this.srv.handlePOSTRequest(this, new StreamReader(memoryStream));
		}

		public void writeSuccess(string content_type = "text/html")
		{
			this.outputStream.WriteLine("HTTP/1.0 200 OK");
			this.outputStream.WriteLine("Content-Type: " + content_type);
			this.outputStream.WriteLine("Connection: close");
			this.outputStream.WriteLine("");
		}

		public void writeFailure()
		{
			this.outputStream.WriteLine("HTTP/1.0 404 File not found");
			this.outputStream.WriteLine("Connection: close");
			this.outputStream.WriteLine("");
		}
	}
}
