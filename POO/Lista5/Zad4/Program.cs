using System;
using System.Collections;

namespace Zad4
{
    class IntComparerAdapter : IComparer
    {
        Func<int, int, int> _comparison;
        public IntComparerAdapter(Func<int, int, int> comparison)
        {
            _comparison = comparison;
        }

        public int Compare(object x, object y)
        {
            return _comparison((int)x, (int)y);
        }
    }

    class Program
    {
        static int IntComparer(int x, int y)
        {
            return x.CompareTo(y);
        }
        static void Main(string[] args)
        {
            ArrayList a = new ArrayList() { 1, 5, 3, 3, 2, 4, 3 };
            a.Sort( new IntComparerAdapter(IntComparer) );

            foreach (int x in a)
                Console.WriteLine(x);
            Console.Read();
        }
    }
}
