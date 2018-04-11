using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad4
{
    public abstract class Shape
    {
        public abstract int CalculateArea();
    }

    public class Rectangle2 : Shape
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public override int CalculateArea()
        {
            return this.Width * this.Height;
        }
    }

    public class Square2 : Shape
    {
        public virtual int Side { get; set; }

        public override int CalculateArea()
        {
            return this.Side * this.Side;
        }
    }

    public class AreaCalculator2
    {
        public int CalculateArea(Shape shape)
        {
            return shape.CalculateArea();
        }
    }

    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
    }
    public class Square : Rectangle
    {
        public override int Width
        {
            get { return base.Width; }
            set { base.Width = base.Height = value; }
        }
        public override int Height
        {
            get { return base.Height; }
            set { base.Width = base.Height = value; }
        }
    }

    public class AreaCalculator
    {
        public int CalculateArea(Rectangle rect)
        {
            return rect.Width * rect.Height;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int w = 4, h = 5;
            Rectangle rect = new Square() { Width = w, Height = h };
            AreaCalculator calc = new AreaCalculator();
            Console.WriteLine("prostokąt o wymiarach {0} na {1} ma pole {2}",
            w, h, calc.CalculateArea(rect));

            Shape sq = new Square2() { Side=w };
            AreaCalculator2 calc2 = new AreaCalculator2();
            Console.WriteLine("kwadrat o wymiarach {0} na {1} ma pole {2}",
            w, w, calc2.CalculateArea(sq));

            Console.Read();
        }
    }
}
