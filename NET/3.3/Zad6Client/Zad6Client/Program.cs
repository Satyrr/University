using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zad6Client
{
    public class Person
    {
        public string firstname, surname;
        public int age;
        public string city;

        public void setProperties(string f, string s, int a, string c)
        {
            this.firstname = f;
            this.surname = s;
            this.age = a;
            this.city = c;
        }

        public string Introduce()
        {
           return String.Format("Hi, my name is {0} {1}, I'm {2} and I live in {3}",
                 firstname, surname, age, city);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Person[] personList = new Person[5];

            personList[0] = new Person();
            personList[0].setProperties("Marcin", "Gruza", 22, "Wodzisław Śląski");
            personList[1] = new Person();
            personList[1].setProperties("Adam", "Nowak", 25, "Wrocław");
            personList[2] = new Person();
            personList[2].setProperties("Jan", "Kowalski", 15, "Warszawa");
            personList[3] = new Person();
            personList[3].setProperties("Anna", "Marek", 32, "Wrocław");
            personList[4] = new Person();
            personList[4].setProperties("Dawid", "Frąckowiak", 12, "Katowice");

            XmlSerializer listSerialize = new XmlSerializer(personList.GetType());

            sendXml(listSerialize, personList);
            Console.ReadLine();
        }

        static void sendXml(XmlSerializer listSerialize, Person[] p)
        {
            try
            {
                TcpClient tc = new TcpClient("127.0.0.1", 12345);

                using (StreamWriter sw = new  StreamWriter(tc.GetStream()))
                {
                    listSerialize.Serialize(sw, p);
                }

                tc.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("send error");
            }
        }
    }
}
