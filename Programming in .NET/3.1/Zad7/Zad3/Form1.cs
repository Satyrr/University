using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad3
{
    public partial class Form1 : Form
    {
        Point formCenter;
        public Form1()
        {
            InitializeComponent();
            InitializeData();
        }

        public void InitializeData()
        {
            
            formCenter = new Point();

            this.ResizeEnd += ResizeInvalidate;
            this.Paint += DrawClock;
            Thread refThread = new Thread(this.RefreshClock);
            refThread.Start();

        }

        private void RefreshClock()
        {
            while(true)
            {
                Thread.Sleep(1000);
                this.Invalidate();
            }
           
        }

        private void ResizeInvalidate(object sender, EventArgs args)
        {
            this.Invalidate();
        }

        private void DrawClock(object sender, EventArgs args)
        {
            Graphics formGraphics = this.CreateGraphics();
            formCenter.X = this.Width / 2;
            formCenter.Y = this.Height / 2;
            
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black);
            pen.Width = 5;

            formGraphics.FillEllipse(brush, new Rectangle(formCenter.X - 5, formCenter.Y - 5, 10, 10));
            formGraphics.DrawEllipse(pen, new Rectangle(formCenter.X - 100, formCenter.Y - 100, 200, 200));

            for (int i = 0; i < 60; i ++)
            {
                int x1 = (int)(100 * Math.Sin((i*6 / 180.0) * Math.PI));
                int x2 = (int)(90 * Math.Sin((i*6 / 180.0) * Math.PI));
                int y1 = (int)(100 * Math.Cos((i*6 / 180.0) * Math.PI));
                int y2 = (int)(90 * Math.Cos((i*6 / 180.0) * Math.PI));

                if (i % 5 == 0)
                    pen.Width = 4;
                else
                    pen.Width = 2;

                formGraphics.DrawLine(pen, new Point(formCenter.X + x1, formCenter.Y + y1), new Point(formCenter.X + x2, formCenter.Y + y2));
            }

            int seconds = DateTime.Now.Second;
            int minutes = DateTime.Now.Minute;
            int hours = DateTime.Now.Hour;

            pen.Width = 2;
            int x = (int)(90 * Math.Sin((seconds * 6.0 * Math.PI) / 180.0));
            int y = (int)(90 * Math.Cos((seconds * 6.0 * Math.PI) / 180.0));
            formGraphics.DrawLine(pen, formCenter.X, formCenter.Y, formCenter.X + x, formCenter.Y - y);

            pen.Width = 3;
            x = (int)(70 * Math.Sin((minutes * 6.0 * Math.PI) / 180.0));
            y = (int)(70 * Math.Cos((minutes * 6.0 * Math.PI) / 180.0));
            formGraphics.DrawLine(pen, formCenter.X, formCenter.Y, formCenter.X + x, formCenter.Y - y);

            pen.Width = 5;
            x = (int)(50 * Math.Sin(((hours % 12) * 30.0 / 180.0) * Math.PI));
            y = (int)(50 * Math.Cos(((hours % 12) * 30.0 / 180.0) * Math.PI));
            formGraphics.DrawLine(pen, formCenter.X, formCenter.Y, formCenter.X + x, formCenter.Y - y);
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            WebClient w = new WebClient();
            string x = w.DownloadString("http://thecodinglove.com");
            MessageBox.Show("Dlugosc strony " + x.Length.ToString());
            x = w.DownloadString("https://www.youtube.com/watch?v=QxHvdJin-o4");
            MessageBox.Show("Dlugosc strony " + x.Length.ToString());
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            HttpClient h = new HttpClient();
            downloadSites(h);
        }

        private async Task downloadSites(HttpClient h)
        {
            string x = await h.GetStringAsync("https://msdn.microsoft.com");
            MessageBox.Show("Dlugosc strony " + x.Length.ToString());
            x = await h.GetStringAsync("https://www.youtube.com/watch?v=QxHvdJin-o4");
            MessageBox.Show("Dlugosc strony " + x.Length.ToString());
        }
    }
}
