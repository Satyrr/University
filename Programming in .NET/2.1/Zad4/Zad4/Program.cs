using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Zad4
{
    class Program
    {
        public static void InspectClass(Object obj)
        {
            Type type = obj.GetType();

            foreach(MethodInfo method in type.GetMethods(BindingFlags.Public |
                                                         BindingFlags.Instance))
            {
                if (method.GetParameters().Length == 0 &&
                    method.ReturnType == typeof(int))
                {
                    Console.WriteLine(method.ToString());
                    if (method.GetCustomAttribute(Type.GetType("Zad4.Oznakowane")) != null)
                    {
                        TestClass t = new TestClass();
                        Console.WriteLine(method.Invoke(t,null));
                    }
                }
                    

            }
        }

        static void Main(string[] args)
        {
            TestClass t = new TestClass();
            InspectClass(t);

            Console.ReadLine();
        }
    }

    class TestClass
    {
        public void FunkcjaPubliczna(int a)
        {
            Console.Write(a);
        }

        [Oznakowane]
        public int FunkcjaPubliczna2()
        {
            Console.Write("To jest oznakowana funkcja publiczna nr 2");
            return 3 + 1;
        }

        public int FunkcjaPubliczna3()
        {
            Console.Write(3);
            return 3 + 1;
        }

        private void FunkcjaPrywatna(int a)
        {
            Console.Write(a);
        }

        private int FunkcjaPrywatna2()
        {
            Console.Write(5);
            return 7 + 1;
        }

        private int FunkcjaBezargumentowa()
        {
            Console.Write(6);
            return 4;
        }

        public static int FunkcjaStatyczna()
        {
            return 10;
        }
    }

    class Oznakowane : Attribute
    {

    }
}
