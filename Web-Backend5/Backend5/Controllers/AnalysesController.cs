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
    public class AnalysesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalysesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analyses
        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .Include(w=>w.Analyses)
                .ThenInclude(w=>w.Lab)
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            //var analysis = await this._context.Analyses
            //    .Include(w => w.Patient)
            //    .Include(w=>w.Lab)
            //    .Where(x => x.PatientId == patientId)
            //    .ToListAsync();


            return this.View(patient.Analyses);
        }

        // GET: Analyses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyses
                .Include(a => a.Patient)
                .Include(a=>a.Lab)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return NotFound();
            }

            return View(analysis);
        }

        // GET: Analyses/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name");
            return this.View(new AnalysesCreateModel());
        }

        // POST: Analyses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, AnalysesCreateModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }           

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);
            var lab = await this._context.Labs
               .SingleOrDefaultAsync(x => x.Id == model.LabId);
            var analysis1 = await this._context.Analyses
                .Where(x => x.PatientId == patientId)
                .ToListAsync();
            foreach (var i in analysis1)
            {
                if (i.Date == model.Date)
                    this.ModelState.AddModelError("Date", "Same dates");
            }
            
            if (patientId == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var analysis = new Analysis
                {
                    PatientId = patient.Id,
                    Type = model.Type,
                    Date = model.Date,
                    Status = model.Status,
                    LabId = model.LabId
                    //Lab = lab                   
                };

                this._context.Add(analysis);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name");
            return this.View(model);
        }

        // GET: Analyses/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var analysis = await this._context.Analyses.SingleOrDefaultAsync(m => m.Id == id);
            var lab = await this._context.Labs
               .SingleOrDefaultAsync(x => x.Id == analysis.LabId);
            if (analysis == null)
            {
                return this.NotFound();
            }

            var model = new AnalysesEditModel
            {
                Type = analysis.Type,
                Date = analysis.Date,
                Status = analysis.Status,
                LabId = analysis.LabId,
                Lab = lab                              
            };
            ViewBag.PatientId = analysis.PatientId;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name");
            return this.View(model);
        }

        // POST: Analyses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, AnalysesEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var analysis = await this._context.Analyses.SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                analysis.Type = model.Type;
                analysis.Date = model.Date;
                analysis.Status = model.Status;
                analysis.LabId = model.LabId;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = analysis.PatientId });
            }
            ViewBag.PatientId = analysis.PatientId;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name");
            return this.View(model);
        }

        // GET: Analyses/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var analysis = await this._context.Analyses
                .Include(w => w.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return this.NotFound();
            }

            return this.View(analysis);
        }

        // POST: Analyses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var analysis = await this._context.Analyses.SingleOrDefaultAsync(m => m.Id == id);
            this._context.Analyses.Remove(analysis);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { patientId = analysis.PatientId });
        }

        private bool AnalysisExists(int id)
        {
            return _context.Analyses.Any(e => e.Id == id);
        }
    }
}
