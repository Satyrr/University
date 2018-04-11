using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Repositories
{
    public interface IVisitRepository
    {
        List<Visit> FindByPatient(int patientId);
        Visit Find(int patientId, int id);
        Visit AddToPatient(int patientId, Visit visit);
        Visit Update(int patientId, int id, Visit visit);
        void Remove(int patientId, int id);
    }
}
