using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    class Program
    {


        static void Main(string[] args)
        {
            Program p = new Program();
            p.liczby();
            Console.ReadLine();
        }

        void liczby()
        {
            for(int i=1; i < 100000; i++)
            {
                int k = i;
                int sum = 0;

                for (; k > 0; k = k / 10)
                {
                    if(k%10 == 0 || i%(k%10) != 0)
                    {
                        break;
                    }
                    sum += k % 10;
                }

                if (k == 0 && i % sum == 0)
                    Console.Write(i.ToString() + "\n");
            }

            return;
        }
    }
}
