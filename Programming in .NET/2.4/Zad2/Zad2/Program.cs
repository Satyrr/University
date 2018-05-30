using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    class MyTab : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static IEnumerable<int> YieldReturn()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }
        static void Main(string[] args)
        {
            var fileQ =
                from line in File.ReadLines("liczby.txt")
                where int.Parse(line) > 100
                orderby line descending
                select int.Parse(line);

            foreach(int i in fileQ)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("LINQ to obj:");

            var result = File.ReadLines("liczby.txt").
                Where(num => int.Parse(num) > 100).
                OrderByDescending(x => int.Parse(x));

            foreach(string i in result)
            {
                Console.WriteLine(i);
            }
            Console.ReadLine();
        }
    }
}
