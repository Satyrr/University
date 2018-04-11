using System;

namespace Zad5
{
	public class Program
	{
		private static void Main(string[] args)
		{
			Person person = new Person("Adam", 19, "Polska");
			Person expr_23 = new Person("Jann", 55, "Polska");
			expr_23.Name = "Jan";
			Console.WriteLine(person.IsOlder(18));
			Console.WriteLine(expr_23.GetAge());
			Year.ev += new IncreaseAge(person.IncreaseAge);
			Console.WriteLine(person.age);
			Year.NextYear();
			Console.WriteLine(person.age);
			Grid expr_7A = new Grid(10, 10);
			expr_7A.DrawRow(3);
			expr_7A[3][6] = 100;
			expr_7A.DrawRow(3);
			Console.ReadLine();
		}
	}
}
