using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad2
{
    public interface IPrototype<T>
    {
        T Clone();
    }

    public class FigurePrototype : IPrototype<FigurePrototype>
    {
        public static FigurePrototype Square;
        public static FigurePrototype Rectangle;
        public static FigurePrototype Circle;

        static FigurePrototype()
        {
            Square = new FigurePrototype();
            Panel panel = new Panel();
            panel.Paint += drawSquare;
            panel.Width = 54;
            panel.Height = 54;
            Square.PaintMethod = drawSquare;
            Square.Figure = panel;

            Rectangle = new FigurePrototype();
            panel = new Panel();
            panel.Paint += drawRectangle;
            panel.Width = 74;
            panel.Height = 54;
            Rectangle.PaintMethod = drawRectangle;
            Rectangle.Figure = panel;

            Circle = new FigurePrototype();
            panel = new Panel();
            panel.Paint += drawCircle;
            panel.Width = 54;
            panel.Height = 54;
            Circle.PaintMethod = drawCircle;
            Circle.Figure = panel;
        }

        private static void drawSquare(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 4.0f);

            g.DrawRectangle(pen, new Rectangle(2, 2, 50, 50));
        }

        private static void drawRectangle(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 4.0f);

            g.DrawRectangle(pen, new Rectangle(2, 2, 70, 50));
        }

        private static void drawCircle(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 4.0f);

            g.DrawEllipse(pen, 2, 2, 50, 50);
        }
     

        public Panel Figure
        {
            get; set;
        }
        public PaintEventHandler PaintMethod;

        public FigurePrototype Clone()
        {
            FigurePrototype f = new FigurePrototype();
            Panel panel = new Panel();
            panel.Paint += this.PaintMethod;
            panel.Width = this.Figure.Width;
            panel.Height = this.Figure.Height;
            panel.Location = this.Figure.Location;
            f.Figure = panel;

            return f;
        }
    }
}
