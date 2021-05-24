using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class DoctorPatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorPatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorPatients
        public async Task<IActionResult> Index(Int32? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            var items = await this._context.DoctorPatients
                .Include(h => h.Doctor)
                .Include(h => h.Patient)
                .Where(x => x.DoctorId == doctor.Id)
                .ToListAsync();
            this.ViewBag.Doctor = doctor;
            return this.View(items);
        }


        // GET: DoctorPatients/Create
        public async Task<IActionResult> Create(Int32? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Doctor = doctor;
            var items = await this._context.DoctorPatients
                  .Include(x => x.Doctor)
                  .Include(x => x.Patient)
                  .Where(x => x.DoctorId == doctorId)
                  .Select(x => x.Patient.Id)                  
                  .ToListAsync();
            var items2 = await this._context.Patients.ToListAsync();
            List<Patient> patients = new List<Patient>();
            foreach(var i in items2) {
                bool flag = false;
                foreach (var j in items)
                {
                    if (i.Id == j)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag) 
                {
                    patients.Add(i);
                }
            }




            this.ViewData["PatientId"] = new SelectList(patients, "Id", "Name");
            return this.View(new DoctorPatientsCreateModel());
        }

        // POST: DoctorPatients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? doctorId, DoctorPatientsCreateModel model)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var doctorPatient = new DoctorPatient
                {
                    DoctorId = doctor.Id,
                    PatientId = model.PatientId
                };

                this._context.Add(doctorPatient);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { doctorId = doctor.Id });
            }

            var items = await this._context.DoctorPatients
                 .Include(x => x.Doctor)
                 .Include(x => x.Patient)
                 .Where(x => x.DoctorId == doctorId)
                 .Select(x => x.Patient.Id)
                 .ToListAsync();
            var items2 = await this._context.Patients.ToListAsync();
            List<Patient> patients = new List<Patient>();
            foreach (var i in items2)
            {
                bool flag = false;
                foreach (var j in items)
                {
                    if (i.Id == j)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    patients.Add(i);
                }
            }
            this.ViewBag.Doctor = doctor;
            this.ViewData["PatientId"] = new SelectList(patients, "Id", "Name", model.PatientId);
            return this.View(model);
        }
        public async Task<IActionResult> Delete(Int32? doctorId, Int32? patientId)
        {
            if (doctorId == null || patientId == null)
            {
                return this.NotFound();
            }

            var doctorPatient = await this._context.DoctorPatients
                .Include(h => h.Doctor)
                .Include(h => h.Patient)
                .SingleOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            if (doctorPatient == null)
            {
                return this.NotFound();
            }

            return this.View(doctorPatient);
        }

        // POST: HospitalDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32? doctorId, Int32? patientId)
        {
            var doctorPatient = await this._context.DoctorPatients.SingleOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            this._context.DoctorPatients.Remove(doctorPatient);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { doctorId = doctorId });
        }

        private bool DoctorPatientExists(int id)
        {
            return _context.DoctorPatients.Any(e => e.DoctorId == id);
        }
    }
}
