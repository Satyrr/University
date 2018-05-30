using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad3
{
    class Program
    {
        static void Main(string[] args)
        {
           
            List<string> lista = new List<string>();
            lista.Add("5");
            lista.Add("43");
            lista.Add("342");

            //ConvertAll
            List<int> lista_int = lista.ConvertAll( new Converter<string, int>(
                                                         (x) =>
                                                         int.Parse(x))
                                                         );

            Console.WriteLine(lista_int[0] + lista_int[1]);

            for (int i = 0; i < 20; i++)
            {
                lista_int.Add(i);
            }
            for (int i = 0; i < 30; i++)
            {
                lista_int.Add(i);
            }

            //FindAll
            Console.WriteLine("parzyste:");
            List<int> parzyste = lista_int.FindAll((x) =>
                                                    x%2 == 0 
                                                    );

            foreach(int i in parzyste)
            {
                Console.WriteLine(i);
            }
            //ForEach
            Console.WriteLine("ForEach(wypisanie elementow w wierszu):");
            parzyste.ForEach((x) => Console.Write(x + " " ) );


            parzyste.RemoveAll((x) => x < 5);
            Console.WriteLine("\nPo usunieciu elementow < 5:");
            foreach (int i in parzyste)
            {
                Console.WriteLine(i);
            }

            //Sort
            parzyste.Sort((x, y) => x - y);
            Console.WriteLine("\nPo posortowaniu:");
            foreach (int i in parzyste)
            {
                Console.WriteLine(i);
            }


            Console.ReadLine();
        }
    }
}
