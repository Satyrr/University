using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid(4, 4);
            grid.DrawRow(2);

            int[] row = grid[2];
            for(int i = 0; i < 4; i++)
            {
                Console.Write(row[i].ToString() + " ");
            }
            Console.Write("\n");
            Console.WriteLine(grid[2,0]);

            Console.ReadLine();
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

            for (int i=0; i<rows; i++)
            {
                tab[i] = new int[cols];
                for(int k=0; k<cols; k++)
                {
                    tab[i][k] = rnd.Next(0,100);
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
