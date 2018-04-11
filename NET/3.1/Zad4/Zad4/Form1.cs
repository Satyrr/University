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

namespace Zad4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            int value = int.Parse(tbxValue.Text);
            int max = int.Parse(tbxMax.Text);
            int min = int.Parse(tbxMin.Text);

            customProgressBar.Max = max;
            customProgressBar.Min = min;
            customProgressBar.Value = value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int value = customProgressBar.Value;
            int min = customProgressBar.Min;
            int max = customProgressBar.Max;
            if (min > max) return;

            for(int i = min; i < max; i++)
            {
                customProgressBar.Value = i;
                Thread.Sleep(30);
            }
            for (int i = max; i > min; i--)
            {
                customProgressBar.Value = i;
                Thread.Sleep(30);
            }
            customProgressBar.Value = value;
        }
    }
}
