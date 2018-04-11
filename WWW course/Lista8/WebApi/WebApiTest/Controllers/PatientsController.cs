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
    [Route("api/Patients")]
    public class PatientsController : Controller
    {
        IPatientRepository _patients;
        IVisitRepository _visits;

        public PatientsController(IPatientRepository _patients, IVisitRepository _visits)
        {
            this._patients = _patients;
            this._visits = _visits;
        }

        // GET: api/Patients
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Patients = _patients.FindAll(), links = GetPatientsLinks() });
        }

        // GET: api/Patients/5
        [HttpGet("{id}", Name = "GetPatient")]
        public IActionResult Get(int id)
        {
            Patient p = _patients.Find(id);
            if (p == null)
                return NotFound();

            return Ok(p);
        }
        
        // POST: api/Patients
        [HttpPost]
        public IActionResult Create([FromBody]Patient patientData)
        {
            if (patientData == null)
            {
                return BadRequest();
            }
            var v = patientData.Visits;

            var p = _patients.Add(patientData);
            if(v != null)
            {
                foreach(var visit in v.Visits)
                    _visits.AddToPatient(p.ID, visit);
            }
            return CreatedAtAction("Get", new { id = p.ID }, p);
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Patient patientData)
        {
            if (patientData == null || patientData.ID != id)
            {
                return BadRequest();
            }

            var p = _patients.Find(id);
            if (p == null)
            {
                return NotFound();
            }

            _patients.Update(id, patientData);
            return new NoContentResult();
        }


        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var patient = _patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            _patients.Remove(id);
            return new NoContentResult();
        }

        private List<LinkInfo> GetPatientsLinks()
        {
            return new List<LinkInfo>()
            {
                new LinkInfo()
                {
                    Href = "/Patients/",
                    Rel = "create",
                    Method = "POST"
                }
            };
        }
    }
}
