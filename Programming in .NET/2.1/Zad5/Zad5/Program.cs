using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad5
{
    public delegate void IncreaseAge();

    public class Program
    {

        static void Main(string[] args)
        {
            Person adam = new Person("Adam", 19, "Polska");
            Person jan = new Person("Jann", 55, "Polska");
            jan.Name = "Jan";

            Console.WriteLine(adam.IsOlder(18));
            Console.WriteLine(jan.GetAge());

            Year.ev += new IncreaseAge(adam.IncreaseAge);
            Console.WriteLine(adam.age);
            Year.NextYear();
            Console.WriteLine(adam.age);

            Grid grid = new Grid(10, 10);
            grid.DrawRow(3);
            grid[3][6] = 100;
            grid.DrawRow(3);

            Console.ReadLine();
        }
    }

    public class Person
    {
        public int age;
        public string name;
        public string location;

        public string Name
        {
            get {  return name; }

            set { name = value; }
        }

        public Person(string n, int a, string l)
        {
            age = a;
            name = n;
            location = l;
        }

        public void IncreaseAge()
        {
            age++;
        }

        public bool IsOlder(int a)
        {
            if (a > age) return true;
            return false;
        }

        public string GetAge()
        {
            switch(age)
            {
                case 10:
                    return "ten";
                case 20:
                    return "twenty";
                default:
                    return "other";
            }
        }
    }

    public class Year
    {
        public static event IncreaseAge ev;
        public static int year = 2000;

        public static void NextYear()
        {
            year++;
            ev();
        }
    }

    public class Grid
    {
        int rows, cols;
        int[][] tab;

        public Grid(int r, int c)
        {
            rows = r;
            cols = c;

            initTable();
        }

        void initTable()
        {
            tab = new int[rows][];

            Random rnd = new Random();

            for (int i = 0; i < rows; i++)
            {
                tab[i] = new int[cols];
                for (int k = 0; k < cols; k++)
                {
                    tab[i][k] = rnd.Next(0, 100);
                }
            }
        }

        public int[] this[int i]
        {
            get
            {
                return tab[i];
            }
        }

        public int this[int i, int j]
        {
            get
            {
                return tab[i][j];
            }
            set
            {
                tab[i][j] = value;
            }
        }

        public void DrawRow(int i)
        {
            for (int k = 0; k < cols; k++)
                Console.Write(tab[i][k].ToString() + " ");
            Console.Write("\n");
        }


    }
}
