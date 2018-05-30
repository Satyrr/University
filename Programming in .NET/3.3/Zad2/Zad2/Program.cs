using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad2
{
    class Set : ArrayList
    {
        override public Object this[int index]
        {
            get
            {
                return base[index];
            }

            set
            {
                if (!base.Contains(value))
                    base[index] = value;
            }
        }

        public override int Add(object value)
        {
            if(!base.Contains(value))
                return base.Add(value);
            return -1;
        }

        public override void Insert(int index, object value)
        {
            if (!base.Contains(value))   
                base.Insert(index, value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Set s = new Set();

            s.Add(49);
            s.Add(2);
            s.Add(210);
            s.Add(2);
            s[0] = 10;
            s[2] = 20;
            s.Add(100);
            s[0] = 100;

            foreach(Object o in s)
            {
                Console.WriteLine(o);
            }

            Console.ReadLine();

        }
    }
}
