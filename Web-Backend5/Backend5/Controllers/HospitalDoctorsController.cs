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
    public class HospitalDoctorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalDoctorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HospitalDoctors
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            var items = await this._context.HospitalDoctors
                .Include(h => h.Hospital)
                .Include(h => h.Doctor)
                .Where(x => x.HospitalId == hospital.Id)
                .ToListAsync();
            this.ViewBag.Hospital = hospital;
            return this.View(items);
        }

        // GET: HospitalDoctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospitalDoctor = await _context.HospitalDoctors
                .Include(h => h.Doctor)
                .Include(h => h.Hospital)
                .SingleOrDefaultAsync(m => m.HospitalId == id);
            if (hospitalDoctor == null)
            {
                return NotFound();
            }

            return View(hospitalDoctor);
        }

        // GET: HospitalDoctors/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            var items = await this._context.HospitalDoctors
                  .Include(x => x.Doctor)
                  .Include(x => x.Hospital)
                  .Where(x => x.HospitalId == hospitalId)
                  .Select(x => x.Doctor.Id)
                  .ToListAsync();
            var items2 = await this._context.Doctors.ToListAsync();
            List<Doctor> doctors = new List<Doctor>();
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
                    doctors.Add(i);
                }
            }
            this.ViewData["DoctorId"] = new SelectList(doctors, "Id", "Name");
            return this.View(new HospitalDoctorCreateModel());
        }

        // POST: HospitalDoctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalDoctorCreateModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this._context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var hospitalDoctor = new HospitalDoctor
                {
                    HospitalId = hospital.Id,
                    DoctorId = model.DoctorId
                };

                this._context.Add(hospitalDoctor);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewBag.Hospital = hospital;
            var items = await this._context.HospitalDoctors
                  .Include(x => x.Doctor)
                  .Include(x => x.Hospital)
                  .Where(x => x.HospitalId == hospitalId)
                  .Select(x => x.Doctor.Id)
                  .ToListAsync();
            var items2 = await this._context.Doctors.ToListAsync();
            List<Doctor> doctors = new List<Doctor>();
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
                    doctors.Add(i);
                }
            }
            this.ViewData["DoctorId"] = new SelectList(doctors, "Id", "Name");
            return this.View(model);
        }

        // GET: HospitalDoctors/Delete/5
        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? doctorId)
        {
            if (hospitalId == null || doctorId == null)
            {
                return this.NotFound();
            }

            var hospitalDoctor = await this._context.HospitalDoctors
                .Include(h => h.Hospital)
                .Include(h => h.Doctor)
                .SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.DoctorId == doctorId);
            if (hospitalDoctor == null)
            {
                return this.NotFound();
            }

            return this.View(hospitalDoctor);
        }

        // POST: HospitalDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 hospitalId, Int32 doctorId)
        {
            var hospitalDoctor = await this._context.HospitalDoctors.SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.DoctorId == doctorId);
            this._context.HospitalDoctors.Remove(hospitalDoctor);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { hospitalId = hospitalId });
        }

        private bool HospitalDoctorExists(int id)
        {
            return _context.HospitalDoctors.Any(e => e.HospitalId == id);
        }
    }
}
