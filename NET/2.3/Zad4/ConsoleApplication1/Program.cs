using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class ListHelper
    {
        public static List<TOutput> ConvertAll<T, TOutput>(
        List<T> list,
        Converter<T, TOutput> converter)
        {
            List<TOutput> NewList = new List<TOutput>();

            foreach(T t in list)
            {
                NewList.Add(converter(t));
            }
            return NewList;
        }

        public static List<T> FindAll<T>(
        List<T> list,
        Predicate<T> match)
        {
            List<T> NewList = new List<T>();
            
            foreach (T t in list)
            {
                if (match(t)) NewList.Add(t);
            }
            return NewList;

        }


        public static void ForEach<T>(List<T> list, Action<T> action)
        {
            foreach(T t in list)
            {
                action(t);
            }

        }


        public static int RemoveAll<T>(
        List<T> list,
        Predicate<T> match)
        {
            int removed = 0;
            for(int i = 0; i<list.Count(); i++)
            {
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    removed++;
                }
            }
            return removed;

        }
        public static void Sort<T>(
        List<T> list,
        Comparison<T> comparison)
        {
            for(int i = 0; i < list.Count(); i++)
            {
                int min_index = i;
                for (int j = i; j < list.Count(); j++)
                {
                    if(comparison(list[min_index], list[j]) > 0 )
                    {
                        min_index = j;
                    }
                }
                T temp = list[i];
                list[i] = list[min_index];
                list[min_index] = temp;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<int> lista = new List<int>();

            lista.Add(5);
            lista.Add(19);
            lista.Add(1);
            lista.Add(95);
            lista.Add(101);

            for(int i = 5; i<10; i++)
            {
                lista.Add(i * 3);
            }

            foreach(int i in lista)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("Parzyste:");
            List<int> parzyste = ListHelper.FindAll(lista, (x) => x % 2 == 0);
            foreach (int i in parzyste)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("ForEach:");
            ListHelper.ForEach(lista, (x) => Console.WriteLine(x));

            Console.WriteLine("Usuniecie parzystych:");
            ListHelper.RemoveAll(lista, (x) => x % 2 == 0);
            foreach (int i in lista)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("Posortowane:");
            ListHelper.Sort(lista, (x, y) => x - y);
            foreach (int i in lista)
            {
                Console.WriteLine(i);
            }

            List<string> lista_string = ListHelper.ConvertAll(lista,
                                                 (x) => x.ToString());
            foreach (string i in lista_string)
            {
                string napis = "liczba: " + i;
                Console.WriteLine(napis);
            }

            Console.ReadLine();
        }
    }
}
