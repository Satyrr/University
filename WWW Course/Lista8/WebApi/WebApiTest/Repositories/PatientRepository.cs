using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private int counter = 6;
        private List<Patient> _patients;

        public PatientRepository()
        {
            _patients = new List<Patient>()
        {
            new Patient()
            {
                ID = 1, Firstname = "Marek", Surname = "Nowak", PhoneNumber="563636726", Visits = new VisitListWrapper() { PatientID = 1}
            },
            new Patient()
            {
                ID = 2, Firstname = "Adam", Surname = "Kowalski", PhoneNumber="597348254", Visits = new VisitListWrapper(){ PatientID = 2}
            },
            new Patient()
            {
                ID = 3, Firstname = "Marcelina", Surname = "Bury", PhoneNumber="764821649", Visits = new VisitListWrapper(){ PatientID = 3}
            },
            new Patient()
            {
                ID = 4, Firstname = "Jan", Surname = "Dziura", PhoneNumber="667211468", Visits = new VisitListWrapper(){ PatientID = 4}
            },
            new Patient()
            {
                ID = 5, Firstname = "Ewa", Surname = "Wójt", PhoneNumber="888673135", Visits = new VisitListWrapper(){ PatientID = 5}
            },
            new Patient()
            {
                ID = 6, Firstname = "Małgorzata", Surname = "Nowak", PhoneNumber="878649731", Visits = new VisitListWrapper(){ PatientID = 6}
            }
        };
        }
        public Patient Add(Patient patient)
        {
            this.counter++;
            patient.ID = this.counter;

            patient.Visits = new VisitListWrapper() { PatientID = patient.ID };

            _patients.Add(patient);
            return patient;
        }

        public Patient Find(int id)
        {
            return _patients.Find(p => p.ID == id);
        }

        public List<Patient> FindAll()
        {
            return _patients;
        }

        public void Remove(int id)
        {
            _patients.RemoveAll(p => p.ID == id);
        }

        public Patient Update(int id, Patient patient)
        {
            var p = _patients.Find(e => e.ID == id);
            if (p != null)
            {
                if (!string.IsNullOrEmpty(p.Firstname)) p.Firstname = patient.Firstname;
                if (!string.IsNullOrEmpty(p.Surname)) p.Surname = patient.Surname;
                if (!string.IsNullOrEmpty(p.PhoneNumber)) p.PhoneNumber = patient.PhoneNumber;
            }
            return p;
        }
    }
}
