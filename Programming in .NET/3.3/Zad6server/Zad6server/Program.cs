using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Zad6server
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
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void Main(string[] args)
        {
            string data = receiveData();
            //Console.WriteLine(data);

            
            Console.ReadLine();
        }

        static string receiveData()
        {
            TcpListener tl = null;
            try
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                tl = new TcpListener(localAddr, 12345);

                Byte[] bytes = new Byte[10000];
                String data = null;

                tl.Start();

                TcpClient client = tl.AcceptTcpClient();

                NetworkStream stream = client.GetStream();
                XmlSerializer xs = new XmlSerializer(typeof(Person[]));

              
                Person[] personList = (Person[])xs.Deserialize(stream);

                Console.WriteLine(personList[0].Introduce());
                
                client.Close();
                
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("receive error");
                return "";
            }
        }
    }
}