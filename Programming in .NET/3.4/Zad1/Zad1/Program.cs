using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Zad1
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xml = new XmlDocument();
            XmlTextReader tr = new XmlTextReader("dane.xml");

            // XmlValidatingReader reader = new XmlValidatingReader(tr);
            XmlReader reader = XmlReader.Create(new StreamReader("dane.xml"));

            XmlSchema schema = XmlSchema.Read();

            reader.Settings.Schemas.Add(schema);
            
            while ( reader.Read())
            {

            }

            //reader.ValidationType = ValidationType.Schema;
            //reader.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);
            //xml.Load(reader);

            Console.WriteLine("Wczytano plik.");
            Console.ReadLine();
        }

        public static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            Console.WriteLine("*Błąd walidacji*");
            Console.WriteLine("\tWaznosc: {0}", args.Severity);
            Console.WriteLine("\tInfo: {0}", args.Message);
        }
    }
}
