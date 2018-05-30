using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zad6
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> file = File.ReadLines("log2.txt");

            var groups = 
                file
                    .GroupBy(x => x.Split(' ').ElementAt(1))
                    .OrderByDescending(x => x.Count()).
                Take(3);

            foreach(var g in groups)
            {
                Console.Write(g.Key + " " + g.Count() + "\n");
            }
            Console.ReadLine();
        }
    }
}
