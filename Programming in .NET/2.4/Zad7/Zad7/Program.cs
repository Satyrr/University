using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad7
{
    class Program
    {
        static void Main(string[] args)
        {
            var item = new { Field1 = "The value", Field2 = 5 };
            var item2 = new { Field1 = "The value2", Field2 = 10 };

            var anonList = new[] { item, item2 }.ToList();

            foreach(var v in anonList)
            {
                Console.WriteLine(v.Field1);
                Console.WriteLine(v.Field2);

            }

            Console.ReadLine();
        }
    }
}
