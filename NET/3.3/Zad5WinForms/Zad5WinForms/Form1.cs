using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad5WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public async void FtpRequest(string uri)
        {
            FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(uri);
            fwr.Method = WebRequestMethods.Ftp.ListDirectory;

            FtpWebResponse response = (FtpWebResponse) await fwr.GetResponseAsync();

            printResponse(response.StatusDescription, response);
        }
        public async void HttpRequest(string uri)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(uri);
            hwr.Method = WebRequestMethods.Http.Get;

            HttpWebResponse response = (HttpWebResponse)await hwr.GetResponseAsync();

            printResponse(response.StatusDescription, response);
        }

        public void WebClientRequest(string uri, string file)
        {
            WebClient wc = new WebClient();

            wc.DownloadFileAsync(new Uri(uri), file);

            textBox1.Text = "Downloaded to file " + file;
        }

        public async void HttpClientRequest(string uri)
        {
            HttpClient hc = new HttpClient();

            string result = await hc.GetStringAsync(uri);

            textBox1.Text = result;
        }

        public async void HttpListenerStart()
        {
            HttpListener hl = new HttpListener();
            hl.Prefixes.Add("http://localhost:8080/");

            hl.Start();

            textBox1.Text = "Listening on http://localhost:8080/...";
            HttpListenerContext hlc = await hl.GetContextAsync();
            HttpListenerRequest hlr = hlc.Request;

            HttpListenerResponse response = hlc.Response;

            string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            // You must close the output stream.
            output.Close();
            hl.Stop();
        }

        public async void TcpListenerStart()
        {
            textBox1.Text = "";
            TcpListener tl = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                tl = new TcpListener(localAddr, 12345);

                Byte[] bytes = new Byte[256];
                String data = null;

                tl.Start();

                TcpClient client = await tl.AcceptTcpClientAsync();

                NetworkStream stream = client.GetStream();

                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    textBox1.Text += data;

                }
                client.Close();
            }
            catch (SocketException e)
            {
                textBox1.Text = String.Format("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                tl.Stop();
            }
        }

        private void TcpClientMessage()
        {
            TcpClient tc = null;
                
            tc = new TcpClient("127.0.0.1", 12345);

            string message = tcpClientTbx.Text;
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = tc.GetStream();

            stream.Write(data, 0, data.Length);
            stream.Close();
            tc.Close();
        }

        private void SendMail()
        {
            SmtpClient sc = new SmtpClient("pl-war-dns02.chello.pl");

            MailAddress from = new MailAddress("marcin@example.com",
               "Marcin " + (char)0xD8 + " Gruza",
               System.Text.Encoding.UTF8);

            MailAddress to = new MailAddress("gruzi12@gmail.com");

            MailMessage message = new MailMessage(from, to);

            message.Body = tcpClientTbx.Text;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test message 1";
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            sc.SendAsync(message, "msg sent");

            message.Dispose();
        }


        private void printResponse(string status, WebResponse res)
        {
            Stream str = res.GetResponseStream();
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            textBox1.Text = status + "\n\n" + text;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FtpRequest("ftp://ftp.icm.edu.pl/");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            HttpRequest("http://www.ii.uni.wroc.pl/~wzychla/");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WebClientRequest("http://www.ii.uni.wroc.pl/~wzychla/files/ProgramowaniePodWindows.pdf", "ProgramowaniePodWindows.pdf");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HttpClientRequest("https://google.pl/");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HttpListenerStart();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TcpListenerStart();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TcpClientMessage();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SendMail();
        }
    }
}
