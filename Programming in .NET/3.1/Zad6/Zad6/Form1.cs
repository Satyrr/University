using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad6
{
    public partial class Form1 : Form
    {
        BackgroundWorker bw;

        public Form1()
        {
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 10000;

            bw = new BackgroundWorker();
            bw.DoWork += PrimeTesterBW;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.WorkerReportsProgress = true;
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblResult.Text = "Ile pierwszych?: " + e.Result.ToString();
            //throw new NotImplementedException();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ParameterizedThreadStart(PrimeTester));
            newThread.Start(tbxZakres.Text);
        }

        private void btnBackgroundWorker_Click(object sender, EventArgs e)
        {
            bw.RunWorkerAsync(int.Parse(tbxZakres.Text));
        }

        private void PrimeTester(object max)
        {
            int res = 0;
            int number = int.Parse(max.ToString());
            for (int i = 2; i <= number; i++)
            {
                if (isPrime(i) == 1) res++;
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = (int)(((double)i / number) * progressBar1.Maximum);
                });
                
            }

            lblResult.Invoke((MethodInvoker)delegate {
                lblResult.Text = "Ile pierwszych?: " + res.ToString();
            });
            
        }

        private void PrimeTesterBW(object sender, DoWorkEventArgs e)
        {
            int res = 0;
            int number = (int)e.Argument;
            for (int i = 2; i <= number; i++)
            {
                if (isPrime(i) == 1) res++;
                bw.ReportProgress(i*100/number);
            }

            //lblResult.Text = "aaa";
            //lblResult.Invoke((MethodInvoker)delegate {
            //    lblResult.Text = "Ile pierwszych?: " + res.ToString();
            //});
            e.Result = res;

        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = (int)((e.ProgressPercentage/100.0)*progressBar1.Maximum);
        }

        private int isPrime(int x)
        {
            for (int i = 2; i * i <= x; i++)
                if (x % i == 0) return 0;

            return 1;
        }
    }
}
