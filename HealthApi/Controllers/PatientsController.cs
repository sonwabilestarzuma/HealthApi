using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthApi.Data;
using HealthApi.Model;
using Microsoft.AspNetCore.Cors;

namespace HealthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("HealthPolicy")]
    public class PatientsController : ControllerBase
    {
        private readonly HealthContext _context;

        public PatientsController(HealthContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public IEnumerable<Patient> GetPatients()
        {
            return  _context.Patients
                        .Include(a => a.Ailments)
                        .Include(m => m.Medications);
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            // getting all the patients with their medications and ailments 

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var patient = await _context.Patients
                .Include(i => i.Ailments)
                .Include(m => m.Medications)
                .FirstOrDefaultAsync(i => i.PatientId == id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.PatientId }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Patient>> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientId == id);
        }

        //// Get api/patients/3/medication 
        //[HttpGet("{int:id}/medication")]
        //public async Task<IActionResult> GetMedications(int id)
        //{
        //    var patient = await _context.Patients
        //        .Include(m => m.Medications)
        //        .FirstOrDefaultAsync(i => i.PatientId == id);

        //    // the patient is not found 
        //    if (patient == null)
        //        return NotFound();

        //        return Ok(patient.Medications);
        //}
    }
}
