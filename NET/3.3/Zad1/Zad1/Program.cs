using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    class Complex :IFormattable
    {
        private double a, b;

        public Complex(double real, double im)
        {
            this.a = real;
            this.b = im;
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.a + c2.a, c1.b + c2.b);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.a*c2.a - c1.b*c2.b, c1.b*c2.a + c1.a*c2.b);
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.a - c2.a, c1.b - c2.b);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch(format)
            {
                case "w":
                    return String.Format("[{0},{1}]", this.a, this.b);
                default:
                    return String.Format("{0}+{1}i", this.a, this.b);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Complex c1 = new Zad1.Complex(4, 50);
            Complex c2 = new Zad1.Complex(3, 6);

            Complex z = new Complex(4, 3);
            Console.WriteLine(String.Format("{0}", z));
            Console.WriteLine(String.Format("{0:d}", z));
            Console.WriteLine(String.Format("{0:w}", z));

            Console.ReadLine();
        }
    }
}
