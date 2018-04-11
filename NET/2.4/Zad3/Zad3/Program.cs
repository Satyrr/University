using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    class Program
    {
        static void Main(string[] args)
        {
            var surnameFirstLetters = from surname in File.ReadLines("nazwiska.txt")
                                      group surname by surname.ElementAt(0) into g
                                      select g.Key;

            foreach (var group in surnameFirstLetters)
            {
                Console.WriteLine(group);
            }

            Console.ReadLine();
        }
    }
}
