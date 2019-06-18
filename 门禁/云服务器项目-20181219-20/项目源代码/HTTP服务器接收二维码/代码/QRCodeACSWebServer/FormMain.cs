using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace QRCodeACSWebServer
{
	public class FormMain : Form
	{
		private QRCodeHttpServer http;

		private Thread thread;

		private int count;

		private IContainer components;

		private Label 端口;

		private TextBox textBox1;

		private Button button1;

		private CheckBox checkBox1;

		private GroupBox groupBox1;

		private GroupBox groupBox2;

		private Button button2;

		private TextBox textBox2;

		private Button button3;

		private TextBox textBox3;

		private TextBox textBox4;

		private Label label1;

		private Label label2;

		public FormMain()
		{
			this.InitializeComponent();
		}

		private void WriteStringDataImp(string dataString)
		{
			base.Invoke(new EventHandler(delegate
			{
				this.textBox3.AppendText(dataString + "\r\n");
			}));
		}

		private string HandleRequestDataImp(string dataString)
		{
			base.Invoke(new EventHandler(delegate
			{
				this.textBox2.AppendText(dataString);
			}));
			string result = "";
			if (this.checkBox1.Checked)
			{
				result = "{\"result\":0}";
			}
			else
			{
                result = "{\"result\":1}";
            }
			base.Invoke(new EventHandler(delegate
			{
				this.textBox2.AppendText(result);
				this.count++;
				this.label2.Text = this.count.ToString();
			}));
			return result;
		}

		private string[] GetLocalIpv4()
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			StringCollection stringCollection = new StringCollection();
			IPAddress[] array = hostAddresses;
			for (int i = 0; i < array.Length; i++)
			{
				IPAddress iPAddress = array[i];
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					stringCollection.Add(iPAddress.ToString());
				}
			}
			string[] array2 = new string[stringCollection.Count];
			stringCollection.CopyTo(array2, 0);
			return array2;
		}

		private void OnClickStart(object sender, EventArgs e)
		{
			if (this.http == null)
			{
				this.count = 0;
				this.label2.Text = this.count.ToString();
				int port = int.Parse(this.textBox1.Text);
				this.http = new QRCodeHttpServer(port);
				QRCodeHttpServer expr_4B = this.http;
				expr_4B.WriteString = (WriteStringData)Delegate.Combine(expr_4B.WriteString, new WriteStringData(this.WriteStringDataImp));
				QRCodeHttpServer expr_72 = this.http;
				expr_72.qrcode_post_handle = (HandleRequestData)Delegate.Combine(expr_72.qrcode_post_handle, new HandleRequestData(this.HandleRequestDataImp));
				this.textBox4.Text = string.Format("http://{0}:{1}/", this.GetLocalIpv4()[0], this.textBox1.Text);
				this.thread = new Thread(new ThreadStart(this.http.listen));
				this.thread.Start();
				this.button1.Enabled = false;
				return;
			}
			this.http.is_active = false;
			this.thread.Abort();
			this.http = null;
			this.thread = null;
			this.button1.Text = "开始";
		}

		private void OnClickRequest(object sender, EventArgs e)
		{
			this.textBox2.Text = "";
			this.count = 0;
			this.label2.Text = this.count.ToString();
		}

		private void OnClickClearNet(object sender, EventArgs e)
		{
			this.textBox3.Text = "";
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.http != null)
			{
				this.http.is_active = false;
				Environment.Exit(0);
				this.http = null;
				this.thread = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.端口 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // 端口
            // 
            this.端口.AutoSize = true;
            this.端口.Location = new System.Drawing.Point(24, 22);
            this.端口.Name = "端口";
            this.端口.Size = new System.Drawing.Size(29, 12);
            this.端口.TabIndex = 0;
            this.端口.Text = "端口";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(64, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(69, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "8000";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "启动";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClickStart);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(64, 55);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "允许开门";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(678, 264);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "二维请求数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(103, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "清空";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClickRequest);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 46);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(666, 212);
            this.textBox2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Location = new System.Drawing.Point(12, 355);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(678, 265);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "网络请求数据";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "清空";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClickClearNet);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(6, 46);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(666, 213);
            this.textBox3.TabIndex = 1;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(265, 19);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(419, 21);
            this.textBox4.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(236, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "URL";
            // 
            // FormMain
            // 
            this.ClientSize = new System.Drawing.Size(702, 629);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.端口);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "二维码门禁测试服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormMain_Load(object sender, EventArgs e)
        {

        }
	}
}
