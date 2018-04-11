using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Zad5
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> accounts = File.ReadLines("accounts.txt");
            IEnumerable<string> people = File.ReadLines("people.txt");

            var mapped = from person in people
                         join acc in accounts on
                             person.Split(',').ElementAt(2).Replace(" ", string.Empty) equals
                             acc.Split(',').ElementAt(0).Replace(" ", string.Empty)
                         select person + ", " + acc.Split(',').ElementAt(1);

            foreach(string p in mapped)
            {
                Console.WriteLine(p);
            }

            Console.ReadLine();
        }
    }
}
