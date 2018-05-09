using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    public class Person
    {
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Birthdate { get; set; }
        public string Address { get; set; }

        public Person(string s, string f, string b, string a)
        {
            this.Surname = s;
            this.Firstname = f;
            this.Birthdate = b;
            this.Address = a;
        }
    }
}
