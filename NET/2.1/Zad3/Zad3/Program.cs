/* Wiktor Zychla, 2003 */
using System;
using System.Reflection;

namespace Example
{
    /// <summary>
    /// Klasa zawierajaca glowne metody prorgramu
    /// </summary>
    public class CExample
    {
        /// <summary>
        /// Metoda demonstrujaca dostep do prywatnych skladowych
        /// zewnetrznych klas
        /// </summary>
        public static void PrivateAccess()
        {
            Person p1 = new Person("Jan", 59);
            Person p2 = new Person("Alina", 39);

            Type person = Type.GetType("Example.Person");
            foreach (MethodInfo member in person.GetRuntimeMethods())
            {
                Console.WriteLine("\tSkladowa: {0}", member);
            }

            /*
            MethodInfo method = person.GetRuntimeMethod("ShowPeopleNumber", new Type[] { });
            if (method != null)
                method.Invoke(p1, null);
            */

            
            MethodInfo method = person.GetMethod("ShowPeopleNumber", BindingFlags.NonPublic | BindingFlags.Static);
            if (method != null)
                method.Invoke(p1, null);
            

            // private static method ShowPeopleNumber call (Czemu nie dziala na Type, a na TypeInfo tak?)
            method = person.GetTypeInfo().GetDeclaredMethod("ShowPeopleNumber");
            if (method != null)
                method.Invoke(p1, null);

            

            // private method Introduce call
            method = person.GetTypeInfo().GetDeclaredMethod("Introduce");
            if (method != null)
                method.Invoke(p1, null);

            //private property
            PropertyInfo property = person.GetTypeInfo().GetDeclaredProperty("Name");
            Console.WriteLine(property.GetValue(p1));
            property.SetValue(p1, "Adam");
            Console.WriteLine(property.GetValue(p1));
        }

        /// <summary>
        /// Metoda porownujaca wydajnosc dostepu do skladowych klasy
        /// za pomoca refleksji oraz w standardowy sposob
        /// </summary>
        public static void EfficiencyCompare()
        {
            //Testy probne
            TestClass.testval = 0;
            for(int i = 0; i<1000000; i++)
            {
                TestClass.Calculate();
            }
            //Standard
            DateTime Start = DateTime.Now;

            for (int i = 0; i < 6000000; i++)
            {
                TestClass.Calculate();
            }

            Type eff = Type.GetType("Example.TestClass");
            MethodInfo calc = eff.GetTypeInfo().GetDeclaredMethod("Calculate");
            DateTime End = DateTime.Now;
            TimeSpan Czas = End - Start;
            Console.WriteLine( "Standard: " + Czas );
            //Refleksja
            Start = DateTime.Now;

            for (int i = 0; i < 6000000; i++)
            {
                calc.Invoke(null, null);
            }

            End = DateTime.Now;
            Czas = End - Start;
            Console.WriteLine("Refleksja : " + Czas);

        }

        /// <summary>
        /// Glowna funkcja programu
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            PrivateAccess();

            EfficiencyCompare();

            Console.ReadLine();
        }

    }

    /// <summary>
    /// Klasa zawierajaca funkcje do testowania wydajnosci 
    /// </summary>
    public class TestClass
    {
        /// <summary>
        /// Zmienna uzywana do testowania wydajnosci
        /// </summary>
        public static int testval;
        
        /// <summary>
        /// Funkcja wykonujaca proste obliczenia
        /// </summary>
        public static void Calculate()
        { 
            testval += 100;
            testval *= 5 % 3;
            testval += 94;
            testval += 3;
        }
    }

    /// <summary>
    /// Klasa reprezentujaca osobe
    /// </summary>
    public class Person
    {
        static private int peopleNumber;
        private string name;
        private int age;

        /// <summary>
        /// Konstruktor osoby.
        /// </summary>
        /// <param name="n">Nazwa osoby</param>
        /// <param name="a">Wiek osoby</param>
        public Person(string n, int a)
        {
            this.name = n;
            this.age = a;
            peopleNumber++;
        }

        private string Name
        {
            get { return name; }

            set { name = value; }
        }

        private void Introduce()
        {
            Console.WriteLine("Imie: " + name + ", wiek: " + age.ToString());
        }

        private static void ShowPeopleNumber()
        {
            Console.WriteLine(peopleNumber);
        }
    }
}
