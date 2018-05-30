using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Models;

namespace WebApiTest.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        int visitCounter;
        IPatientRepository _patients;
        List<Visit> _visits = new List<Visit>()
        {
            new Visit() { PatientID = 1, ID = 1, VisitDateTime = DateTime.Now.AddDays(-2.3), PaymentStatus = new Payment
                { Status="refunded", Amount=200.0, PatientID = 1, VisitID = 1 } },
            new Visit() { PatientID = 1, ID = 2, VisitDateTime = DateTime.Now.AddDays(5.3), PaymentStatus = new Payment
                { Status="paid", Amount=2030.0, PatientID = 1, VisitID = 2 } },
            new Visit() { PatientID = 2, ID = 3, VisitDateTime = DateTime.Now.AddDays(3.1), PaymentStatus = new Payment
                { Status="pending", Amount=400.0, PatientID = 2, VisitID = 3 } },
            new Visit() { PatientID = 3, ID = 4, VisitDateTime = DateTime.Now.AddDays(7.0), PaymentStatus = new Payment
                { Status="paid", Amount=100.0, PatientID = 3, VisitID = 4 } },
            new Visit() { PatientID = 4, ID = 5, VisitDateTime = DateTime.Now.AddDays(8.4), PaymentStatus = new Payment
                { Status="pending", Amount=500.0, PatientID = 4, VisitID = 5 } },
            new Visit() { PatientID = 5, ID = 6, VisitDateTime = DateTime.Now.AddDays(9.3), PaymentStatus = new Payment
                { Status="pending", Amount=300.0, PatientID = 5, VisitID = 6 } },
            new Visit() { PatientID = 6, ID = 7, VisitDateTime = DateTime.Now.AddDays(2.5), PaymentStatus = new Payment
                { Status="paid", Amount=600.0, PatientID = 6, VisitID = 7 } },
        };

        public VisitRepository(IPatientRepository _patients)
        {
            this._patients = _patients;
            _patients.Find(1).Visits.Visits.Add(_visits[0]);
            _patients.Find(1).Visits.Visits.Add(_visits[1]);
            _patients.Find(2).Visits.Visits.Add(_visits[2]);
            _patients.Find(3).Visits.Visits.Add(_visits[3]);
            _patients.Find(4).Visits.Visits.Add(_visits[4]);
            _patients.Find(5).Visits.Visits.Add(_visits[5]);
            _patients.Find(6).Visits.Visits.Add(_visits[6]);
            this.visitCounter = _visits.Count;
        }

        public Visit AddToPatient(int patientId, Visit visit)
        {
            this.visitCounter++;
            visit.ID = this.visitCounter;
            visit.PatientID = patientId;
            visit.PaymentStatus.VisitID = visit.ID;
            visit.PaymentStatus.PatientID = patientId;

            Patient p = _patients.Find(patientId);
            if (p != null)
            {
                p.Visits.Visits.Add(visit);
                return visit;
            }
            else
            {
                return null;
            }   
        }

        public Visit Find(int patientId ,int id)
        {
            Patient p = _patients.Find(patientId);
            if (p == null)
                return null;

            return p.Visits.Visits.Find(e => e.ID == id);

        } 

        public List<Visit> FindByPatient(int patientId)
        {
            Patient p = _patients.Find(patientId);
            if (p == null)
                return null;

            return p.Visits.Visits;
        }

        public void Remove(int patientId, int id)
        {
            Patient p = _patients.Find(patientId);

            if (p == null) return;
            p.Visits.Visits.RemoveAll(e => e.ID == id);
        }

        public Visit Update(int patientId, int id, Visit visit)
        {
            Patient p = _patients.Find(patientId);
            if (p == null)
                return null;

            Visit v = p.Visits.Visits.Find(e => e.ID == id);
            if (v != null)
            {
                if (v.PaymentStatus != null) v.PaymentStatus = visit.PaymentStatus;
                if (v.VisitDateTime != null) v.VisitDateTime = visit.VisitDateTime;
            }
            return v;
        }
    }
}
