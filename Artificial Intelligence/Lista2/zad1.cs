using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AILista2
{
	class MainClass
	{
		List<List<Tuple<int, int>>> possible_block_positions(int seq_len, List<int> block_lens)
		{
			int block_num = block_lens.Count;
			Queue<List<Tuple<int, int>>> q = new Queue<List<Tuple<int, int>>>();
			int pos = 0;
			var positions = new List<List<Tuple<int, int>>> ();

			while(pos + block_lens.Sum () +  block_lens.Count - 1 <= seq_len )
			{
				q.Enqueue (new List<Tuple<int, int>>() { new Tuple<int,int> (pos, block_lens[0]) });
				pos += 1;
			}

			while (q.Count != 0)
			{
				var state = q.Dequeue ();
				int state_len = state.Count;

				if (state_len != block_num) {
					pos = state.Last ().Item1 + state.Last ().Item2 + 1;
					while (pos + block_lens.Skip (state_len).Sum () + block_lens.Skip (state_len).Count () - 1 <= seq_len) {
						var new_state = state.ToList ();
						new_state.Add (new Tuple<int, int>(pos, block_lens[state_len]));
						q.Enqueue (new_state);
						pos += 1;
					}
				}
				else
				{
					positions.Add (state);
				}
			}

			return positions;

		}

		int opt_dist(int[] seq, Tuple<int, int>[][] states)
		{
			int min_swaps = 9999;
			int seq_sum = seq.Sum ();
			int block_sum = 0;
			for (int i = 0; i < states.ElementAt (0).Count (); i++)
				block_sum += states.ElementAt (0).ElementAt (i).Item2;

			foreach (var state in states) {
				int swaps = 0;
				foreach (var s in state) {
					int sum = 0;
					for (int i = s.Item1; i < s.Item1 + s.Item2; i++)
						sum += seq [i];
					swaps += s.Item2 - sum;
				}

				swaps += seq_sum + swaps - block_sum;
				min_swaps = swaps < min_swaps ? swaps : min_swaps;
				if (min_swaps == 0)
					return 0;
			}
			return min_swaps;
		}

		void random_fill(int[,] image)
		{
			Random rnd = new Random();
			for(int x=0; x<image.GetLength (0); x++)
				for(int y=0; y<image.GetLength (1); y++)
					image[x,y] = rnd.Next (0,2);
		}

		void draw_image(int[,] image)
		{
			StringBuilder builder = new StringBuilder();
			for (int x = 0; x < image.GetLength (0); x++) {
				for (int y = 0; y < image.GetLength (1); y++) {
					builder.Append (image[x,y] == 1? '#' : '.');
				}
				builder.AppendLine ();
			}
			System.IO.StreamWriter file = new System.IO.StreamWriter(@"zad_output.txt");
			file.Write(builder.ToString());
			file.Close ();
		}
			
		void print_image(int[,] image)
		{
			StringBuilder builder = new StringBuilder();
			for (int x = 0; x < image.GetLength (0); x++) {
				for (int y = 0; y < image.GetLength (1); y++) {
					builder.Append (image[x,y] == 1? '#' : '.');
				}
				builder.AppendLine ();
			}
			//Console.Write(builder.ToString()); 
		}

		List<Tuple<int,int>> get_invalid_rows_cols(int[] col_opt_dists, int[] row_opt_dists)
		{
			var invalid = new List<Tuple<int,int>> ();

			for (int col_num = 0; col_num < col_opt_dists.Length; col_num++) {
				if (col_opt_dists [col_num] > 0) {
					invalid.Add (new Tuple<int, int> (-1, col_num));
				}
			}

			for (int row_num = 0; row_num < row_opt_dists.Length; row_num++) {
				if (row_opt_dists [row_num] > 0) {
					invalid.Add (new Tuple<int, int> (row_num, -1));
				}
			}

			return invalid;
		}

		int[] getColumn(int[,] image, int col_num)
		{
			int[] column = new int[image.GetLength (0)];
			for (int row = 0; row < image.GetLength (0); row++)
				column [row] = image [row, col_num];

			return column;
		}

		int[] getRow(int[,] image, int row_num)
		{
			int[] row = new int[image.GetLength (1)];
			for (int col = 0; col < image.GetLength (1); col++)
				row [col] = image [row_num, col];

			return row;
		}
			
		void WalkSat(List<List<int>> row_descriptions, List<List<int>> col_descriptions)
		{
			Random rnd = new Random ();
			int rows = row_descriptions.Count;
			int cols = col_descriptions.Count;

			int[,] image = new int[rows, cols];
			random_fill (image);

			var rows_block_positions = new List<Tuple<int, int>[][]> ();
			var cols_block_positions = new List<Tuple<int, int>[][]> ();
			foreach(List<int> row_desc in row_descriptions)
			{
				var b = possible_block_positions (cols, row_desc);
				Tuple<int, int>[][] r = b.Select (s => s.ToArray ()).ToArray ();//new Tuple<int,int>[b.Count, b[0].Count];
				rows_block_positions.Add (r);
			}

			foreach(List<int> col_desc in col_descriptions)
			{
				var b = possible_block_positions (rows, col_desc);
				Tuple<int, int>[][] c = b.Select (s => s.ToArray ()).ToArray ();
				cols_block_positions.Add (c);
			}

			int[] col_opt_dists = new int[cols];
			for (int i = 0; i < cols; i++)
				col_opt_dists[i] = opt_dist (getColumn(image, i), cols_block_positions[i]);
			int[] row_opt_dists = new int[rows];
			for (int i = 0; i < rows; i++)
				row_opt_dists[i] = opt_dist (getRow (image, i), rows_block_positions[i]);
			
			int iterations = 0;

			while (true) {
				//print_image (image);
				List<Tuple<int,int>> invalid_rc = get_invalid_rows_cols (col_opt_dists, row_opt_dists);
				if (invalid_rc.Count == 0) {
					Console.WriteLine ("found in:" + iterations.ToString ());
					print_image (image);
					break;
				}
				Tuple<int, int> selected = invalid_rc[rnd.Next (0, invalid_rc.Count)];

				int min_dist = 99999;
				Tuple<int, int> min_dist_pos = new Tuple<int, int>(-1, -1);

				int num = 0;
				while (num < (selected.Item1 == -1 ? rows : cols)) {
					int row_num = selected.Item2== -1 ? selected.Item1 : num;
					int col_num = selected.Item1 == -1 ? selected.Item2 : num;

					int[] row = getRow (image, row_num);
					int[] col = getColumn (image, col_num);
					int old_row_dist = row_opt_dists [row_num];
					int old_col_dist = col_opt_dists [col_num];
					int orig_dist = old_col_dist + old_row_dist;

					col [row_num] = (col [row_num] + 1) % 2;
					row [col_num] = (row [col_num] + 1) % 2;
					int new_row_dist = opt_dist (row, rows_block_positions [row_num]);
					int new_col_dist = opt_dist (col, cols_block_positions [col_num]);
					int dist = new_row_dist + new_col_dist;

					if (dist - orig_dist == min_dist && old_row_dist > new_row_dist && old_col_dist > new_col_dist && rnd.NextDouble () < 0.5f) {
						min_dist_pos = new Tuple<int,int> (row_num, col_num);
					} else if (dist - orig_dist < min_dist && old_row_dist >= new_row_dist && old_col_dist >= new_col_dist) {
						min_dist_pos = new Tuple<int,int> (row_num, col_num);
						min_dist = dist - orig_dist;
					} else if (dist - orig_dist < min_dist && rnd.NextDouble () < 0.15f) {
						min_dist_pos = new Tuple<int,int> (row_num, col_num);
						min_dist = dist - orig_dist;
					
					}
					num += 1;
				}
				int x = min_dist_pos.Item1; int y = min_dist_pos.Item2;
				if (x == -1 && y == -1) {
					continue;
				}
				image [x, y] = (image [x, y] + 1) % 2;
				row_opt_dists [x] = opt_dist (getRow (image, x), rows_block_positions [x]);
				col_opt_dists [y] = opt_dist (getColumn (image, y), cols_block_positions [y]);

				iterations += 1;
				if (iterations % 1000 == 0)
					Console.WriteLine (iterations);
				if (iterations > rows * cols * 200) {
					print_image (image);
					random_fill (image);
					iterations = 0;
				}
			}
			draw_image (image);


		}
		public static void Main (string[] args)
		{
			System.IO.StreamReader file = new System.IO.StreamReader(@"zad_input.txt");

			List<List<int>> row = new List<List<int>> ();
			List<List<int>> col = new List<List<int>> ();
			int row_num = 0;
			string line = "";
			int idx = 0;
			while (!file.EndOfStream) {
				line = file.ReadLine ();
				string[] nums = line.Split (new Char[] { ' ' });

				if (idx == 0) {
					row_num = int.Parse (nums [0]);
				} else if (row.Count < row_num) {
					row.Add (nums.Select (s => int.Parse (s)).ToList ());
				} else {
					col.Add (nums.Select(s=> int.Parse (s)).ToList ());
				}

				idx += 1;
			}
				
			new MainClass().WalkSat (row, col);
		}
	}
}
