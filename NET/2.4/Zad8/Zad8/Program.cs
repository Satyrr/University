using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad8
{
    class Program
    {

        public static Func<A, R> Y<A, R>(Func<Func<A, R>, Func<A, R>> f)
        {
            Func<A, R> g = null;
            g = f(a => g(a));
            return g;
        }


        static void Main(string[] args)
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            foreach (var item in
            list.Select(i => Y<int, int>(f => x => (x <= 1) ? x : f(x - 2) + f(x - 1))(i)))
                Console.WriteLine(item);

            Console.ReadLine();
        }
        
    }
}

