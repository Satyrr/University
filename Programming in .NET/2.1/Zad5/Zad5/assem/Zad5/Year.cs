using System;
using System.Runtime.CompilerServices;

namespace Zad5
{
	public class Year
	{
		public static int year = 2000;

		[method: CompilerGenerated]
		[CompilerGenerated]
		public static event IncreaseAge ev;

		public static void NextYear()
		{
			Year.year++;
			Year.ev();
		}
	}
}
