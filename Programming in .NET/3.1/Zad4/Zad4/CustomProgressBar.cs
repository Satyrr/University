using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Zad4
{
    public partial class CustomProgressBar : UserControl
    {
        public CustomProgressBar()
        {
            InitializeComponent();
            max = 100;
            min = 0;
            val = 0;
            this.Paint += DrawProgress;
        }

        int max;
        public int Max {
            get
            {
                return max;
            }
            set
            {
                max = value;
                this.Refresh();
            }
        }

        int min;
        public int Min {
            get
            {
                return min;
            }
            set
            {
                min = value;
                this.Refresh();
            } 
         }

        int val;
        public int Value {
            get
            {
                return val;
            }
            set
            {
                val = value;
                this.Refresh();
            }
        }

        public void DrawProgress(object sender, EventArgs args)
        {
            int width = (int)(((Value - Min) / (double)(Max - Min)) * this.Width);
            if (width <= 0) return;
            Graphics gr = this.CreateGraphics();

            LinearGradientBrush br = new LinearGradientBrush(new Point(0, 0), new Point(width, 0), Color.Aqua, Color.Chocolate);
            

            
            gr.FillRectangle(br, 0, 0, width, this.Height);
        }
    }
}
