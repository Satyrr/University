using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> FindAll();
        Patient Find(int id);
        Patient Add(Patient patient);
        Patient Update(int id, Patient patient);
        void Remove(int id);
    }
}
