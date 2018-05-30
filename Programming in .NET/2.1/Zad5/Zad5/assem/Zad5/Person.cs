using System;

namespace Zad5
{
	public class Person
	{
		public int age;

		public string name;

		public string location;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public Person(string n, int a, string l)
		{
			this.age = a;
			this.name = n;
			this.location = l;
		}

		public void IncreaseAge()
		{
			this.age++;
		}

		public bool IsOlder(int a)
		{
			return a > this.age;
		}

		public string GetAge()
		{
			int num = this.age;
			if (num == 10)
			{
				return "ten";
			}
			if (num != 20)
			{
				return "other";
			}
			return "twenty";
		}
	}
}
