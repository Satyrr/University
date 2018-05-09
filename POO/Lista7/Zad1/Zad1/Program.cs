using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zad1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            List<Person> studentList = new List<Person>()
            {
                new Person("Pabacki", "Adam", "1955-05-05", "Wrocław"),
                new Person("Pabacka", "Marta",  "1975-05-10", "Wrocław"),
            };

            List<Person> lecturerList = new List<Person>()
            {
                new Person("Kowalski", "Marek", "1975-05-10", "Wrocław"),
                new Person("Nowak", "Jan", "1955-05-05", "Wrocław"),
            };
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(studentList, lecturerList));
        }
    }
}
