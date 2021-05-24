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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Placements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Placements.Include(p => p.Patient).Include(p => p.Ward);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // GET: Placements/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name");
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name");
            return View(new PlacementsCreateModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlacementsCreateModel model)
        {
            var check_placement = await this._context.Placements
                .Where(x => x.WardId == model.WardId)
                .Where(x => x.Bed == model.Bed)
                .ToListAsync();
            if(check_placement.Count>0)
                this.ModelState.AddModelError("Bed", "The bed is occupied");
            if (ModelState.IsValid)
            {
                
                var placement = new Placement
                {
                    Bed = model.Bed,
                    WardId = model.WardId,
                    PatientId = model.PatientId
                };
                this._context.Add(placement);
                await _context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name");
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name");
            return this.View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }
            var model = new PlacementsEditModel
            {
                Bed = placement.Bed,
                WardId = placement.WardId,
                PatientId = placement.PatientId
            };
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", placement.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", placement.WardId);
            return View(model);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, PlacementsEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this._context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                placement.Bed = model.Bed;
                placement.WardId = model.WardId;
                placement.PatientId = model.PatientId;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = placement.WardId, patientId=placement.PatientId });
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "Id", "Name", placement.PatientId);
            ViewData["WardId"] = new SelectList(_context.Wards, "Id", "Name", placement.WardId);
            return this.View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placement = await this._context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            this._context.Placements.Remove(placement);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { wardId = placement.WardId, patientId = placement.PatientId });
        }

        private bool PlacementExists(int id)
        {
            return _context.Placements.Any(e => e.Id == id);
        }
    }
}
