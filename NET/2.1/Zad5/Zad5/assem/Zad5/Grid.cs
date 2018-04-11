using System;

namespace Zad5
{
	public class Grid
	{
		private int rows;

		private int cols;

		private int[][] tab;

		public int[] this[int i]
		{
			get
			{
				return this.tab[i];
			}
		}

		public int this[int i, int j]
		{
			get
			{
				return this.tab[i][j];
			}
			set
			{
				this.tab[i][j] = value;
			}
		}

		public Grid(int r, int c)
		{
			this.rows = r;
			this.cols = c;
			this.initTable();
		}

		private void initTable()
		{
			this.tab = new int[this.rows][];
			Random random = new Random();
			for (int i = 0; i < this.rows; i++)
			{
				this.tab[i] = new int[this.cols];
				for (int j = 0; j < this.cols; j++)
				{
					this.tab[i][j] = random.Next(0, 100);
				}
			}
		}

		public void DrawRow(int i)
		{
			for (int j = 0; j < this.cols; j++)
			{
				Console.Write(this.tab[i][j].ToString() + " ");
			}
			Console.Write("\n");
		}
	}
}
