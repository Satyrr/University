using Lista7WWW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lista7WWW.Services
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person GetById(int id);
    }

    public class PersonRepository : IPersonRepository
    {
        List<Person> _persons;

        public PersonRepository()
        {
            _persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "Jan", LastName = "Kowalski", Address = "ul. Trawiasta 10" },
                new Person { Id = 2, FirstName = "Agata", LastName = "Zielony", Address = "ul. Koszykowa 12" },
                new Person { Id = 3, FirstName = "Ewa", LastName = "Szumska", Address = "ul. Zaciszna 3" },
                new Person { Id = 4, FirstName = "Marek", LastName = "Nowak", Address = "ul. Piaskowa 10" },
                new Person { Id = 5, FirstName = "Monika", LastName = "Bury", Address = "ul. Kokoszycka 12" },
                new Person { Id = 6, FirstName = "Adam", LastName = "Mokry", Address = "ul. Opolska 3" },
                new Person { Id = 7, FirstName = "Bartek", LastName = "Kowalski", Address = "ul. Trawiasta 10" },
                new Person { Id = 8, FirstName = "Michał", LastName = "Zielony", Address = "ul. Koszykowa 12" },
                new Person { Id = 9, FirstName = "Jakub", LastName = "Szumska", Address = "ul. Zaciszna 3" },
                new Person { Id = 10, FirstName = "Agnieszka", LastName = "Nowak", Address = "ul. Piaskowa 10" },
                new Person { Id = 11, FirstName = "Paweł", LastName = "Bury", Address = "ul. Kokoszycka 12" },
                new Person { Id = 12, FirstName = "Bożena", LastName = "Mokry", Address = "ul. Opolska 3" }
            };
        }
        public IEnumerable<Person> GetAll()
        {
            return _persons;
        }

        public Person GetById(int id)
        {
            foreach (var p in _persons)
                if (p.Id == id)
                    return p;
            return null;
        }

    }
}
