using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    class Program
    {
        int Foo(int x, int y)
        {
            int res = 0;
            for (int i = 0; i < 6000000; i++)
            {
                res += (i * x + i * y) % 4;
            }
            return res;
        }
        dynamic Foo2(dynamic x, dynamic y)
        {
            int res = 0;
            for (int i = 0; i < 6000000; i++)
            {
                res += (i * x+ i * y) % 4;
            }

            return res;
        }

        static void Main(string[] args)
        {
            Program p = new Zad1.Program();
            DateTime Start = DateTime.Now;
            p.Foo(5, 3);
            DateTime End = DateTime.Now;
            TimeSpan Czas = End - Start;
            Console.WriteLine("standard: " + Czas);

            Start = DateTime.Now;
            p.Foo2(5, 3);
            End = DateTime.Now;
            Czas = End - Start;
            Console.WriteLine("dynamic: " + Czas);
            Console.ReadLine();
        }
    }
}
