using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTest.Repositories;
using WebApiTest.Models;

namespace WebApiTest.Controllers
{
    [Produces("application/json")]
    [Route("api/Patients/{patientId}/Visits")]
    public class VisitsController : Controller
    {
        IVisitRepository _visits;
        IPatientRepository _patients;

        public VisitsController(IVisitRepository _visits, IPatientRepository _patients)
        {
            this._visits = _visits;
            this._patients = _patients;
        }

        // GET: api/Patients/{patientId}/Visits
        [HttpGet]
        public IActionResult Get(int patientId)
        {
            return Ok( new { Visits = _visits.FindByPatient(patientId), Links = GetVisitLinks(patientId) });
        }

        // GET: api/Patients/{patientId}/Visits/5
        [HttpGet("{id}", Name = "GetVisit")]
        public IActionResult Get(int patientId, int id)
        {
            Visit v = _visits.Find(patientId, id);
            if (v == null)
                return NotFound();

            return new ObjectResult(v);
        }

        // POST: api/Patients/{patientId}/Visits
        [HttpPost]
        public IActionResult Create(int patientId, [FromBody]Visit visitData)
        {
            if (visitData == null || visitData.PaymentStatus == null)
            {
                return BadRequest();
            }
            var v = _visits.AddToPatient(patientId, visitData);
            return CreatedAtAction("Get", new { id = v.ID }, v);
        }
        
        // PUT: api/Patients/{patientId}/Visits/5
        [HttpPut("{id}")]
        public IActionResult Put(int patientId, int id, [FromBody]Visit visitData)
        {
            if (visitData == null || visitData.ID != id)
            {
                return BadRequest();
            }

            Visit v =_visits.Update(patientId, id, visitData);
            if (v == null) return NotFound();
            return new NoContentResult();
        }

        // DELETE: api/Patients/{patientId}/Visits/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int patientId, int id)
        {
            Visit v = _visits.Find(patientId, id);
            if (v == null)
                return NotFound();

            _visits.Remove(patientId, id);
            return new NoContentResult();
        }
        // GET: api/Patients/{patientId}/Visits/5/PaymentStatus
        [HttpGet("{id}/PaymentStatus")]
        public IActionResult GetPaymentStatus(int patientId, int id)
        {           
            Visit v = _visits.Find(patientId, id);
            if (v == null)
                return NotFound();

            Payment payment = v.PaymentStatus;

            return new ObjectResult(payment);
        }

        // PUT: api/Patients/{patientId}/Visits/5/PaymentStatus
        [HttpPut("{id}/PaymentStatus")]
        public IActionResult PutPaymentStatus(int patientId, int id, [FromBody]Payment paymentStatus)
        {
            if (paymentStatus == null)
            {
                return BadRequest();
            }
            
            Visit v = _visits.Find(patientId, id);
            if(v == null)
            {
                return NotFound();
            }

            v.PaymentStatus = paymentStatus;
            return new NoContentResult();
        }

        private List<LinkInfo> GetVisitLinks(int patientId)
        {
            return new List<LinkInfo>()
            {
                new LinkInfo()
                {
                    Href = "/Patients/" + patientId + "/Visits/",
                    Rel = "create",
                    Method = "POST"
                }
            };
        }
    }
}
