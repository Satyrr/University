using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    class EffTest
    {

        public static void ListTest<T>(T val)
        {
            int Attempts = 10000000;
            ArrayList Alist = new ArrayList();
            List<T> List = new List<T>();

            DateTime Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                Alist.Add(i);
            }

            DateTime End = DateTime.Now;
            TimeSpan Time = End - Start;
            Console.WriteLine("Array list addition time: " + Time.ToString());

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                List.Add(val);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("List addition time: " + Time.ToString());

            
            ArrayList CopyList = new ArrayList();

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                CopyList.Add(Alist[i]);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Array List lookup time: " + Time.ToString());

            ArrayList CopyList2 = new ArrayList();

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                CopyList2.Add(List[i]);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("List lookup time: " + Time.ToString());

            Start = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                Alist.RemoveAt(0);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Array List remove time: " + Time.ToString());

            Start = DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                List.RemoveAt(0);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("List remove time: " + Time.ToString());


        }

        public static void HashTest<T,K>(T[] keys, K[] vals, int Attempts)
        {
            Hashtable Hash = new Hashtable();
            Dictionary<T,K> Dict = new Dictionary<T, K>();


            //Addition time
            DateTime Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                Hash.Add(i, i * 2);
            }

            DateTime End = DateTime.Now;
            TimeSpan Time = End - Start;
            Console.WriteLine("Hash addition time: " + Time.ToString());

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                Dict.Add(keys[i], vals[i]);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Dictionary addition time: " + Time.ToString());


            // Lookup time
            ArrayList CopyList = new ArrayList();

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                CopyList.Add(Hash[i]);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Hash List lookup time: " + Time.ToString());

            ArrayList CopyList2 = new ArrayList();

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                CopyList2.Add(Dict[keys[i]]);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Dictionary lookup time: " + Time.ToString());


            // Removal time
            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                Hash.Remove(i);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Hash List remove time: " + Time.ToString());

            Start = DateTime.Now;
            for (int i = 0; i < Attempts; i++)
            {
                Dict.Remove(keys[i]);
            }

            End = DateTime.Now;
            Time = End - Start;
            Console.WriteLine("Dictionary remove time: " + Time.ToString());


        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EffTest.ListTest<int>(20);

            int Attempts = 5000000;
            int[] keys = new int[Attempts];
            double[] vals = new double[Attempts];

            for(int i = 0; i < Attempts; i++)
            {
                keys[i] = i;
            }

            for (int i = 0; i < Attempts; i++)
            {
                vals[i] = 9493.2/i;
            }

            Console.WriteLine("\n\n");
            EffTest.HashTest<int,double>(keys, vals, Attempts);


            Console.ReadLine();
        }
    }
}
