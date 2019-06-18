using System;
using System.IO;

namespace QRCodeACSWebServer
{
	public class QRCodeHttpServer : HttpServer
	{
		public HandleRequestData qrcode_post_handle;

		public QRCodeHttpServer()
		{
		}

		public QRCodeHttpServer(int port) : base(port)
		{
		}

		private void writeLine(string data)
		{
			if (this.writestring != null)
			{
				this.writestring(data);
			}
		}

		public override void handleGETRequest(HttpProcessor p)
		{
			if (p.http_url.Equals("/Test.png"))
			{
				Stream stream = File.Open("../../Test.png", FileMode.Open);
				p.writeSuccess("image/png");
				stream.CopyTo(p.outputStream.BaseStream);
				p.outputStream.BaseStream.Flush();
			}
			this.writeLine(string.Format("request: {0}", p.http_url));
			p.writeSuccess("text/html");
			p.outputStream.WriteLine("<html><body><h1>test server</h1>");
			p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
			p.outputStream.WriteLine("url : {0}", p.http_url);
			p.outputStream.WriteLine("<form method=post action=/form>");
			p.outputStream.WriteLine("<input type=text name=foo value=foovalue>");
			p.outputStream.WriteLine("<input type=submit name=bar value=barvalue>");
			p.outputStream.WriteLine("</form>");
		}

		public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
		{
			this.writeLine(string.Format("POST request: {0}", p.http_url));
			string text = inputData.ReadToEnd();
			if (this.qrcode_post_handle != null)
			{
				text = this.qrcode_post_handle(text);
				p.writeSuccess("text/html");
				p.outputStream.WriteLine(text);
				return;
			}
			p.writeSuccess("text/html");
			p.outputStream.WriteLine("<html><body><h1>test server</h1>");
			p.outputStream.WriteLine("<a href=/test>return</a><p>");
			p.outputStream.WriteLine("postbody: <pre>{0}</pre>", text);
		}
	}
}
