using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "Kobyła ma mały bok....";
            string str2 = "To nie jest palindrom";

            Console.WriteLine(str.IsPalindrome());
            Console.WriteLine(str2.IsPalindrome());
            Console.ReadLine();
        }
    }

    public static class MyExtensions
    {
        public static bool IsPalindrome(this string str)
        {
            string[] arrayStr = str.ToLower().Split(new char[] { '.', ',', ' ' });
            string concatString = string.Join("", arrayStr);
            char[] revTab = concatString.ToCharArray();
            Array.Reverse(revTab);
            string revString = new string(revTab);

            return revString == concatString;
        }
    }
}
