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
    public class WardsController : Controller
    {
        private readonly ApplicationDbContext context;

        public WardsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Wards
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            var wards = await this.context.Wards
                .Include(w => w.Hospital)
                .Where(x => x.HospitalId == hospitalId)
                .ToListAsync();

            return this.View(wards);
        }

        // GET: Wards/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .Include(w => w.Hospital)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            return this.View(ward);
        }

        // GET: Wards/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Hospital = hospital;
            return this.View(new WardCreateModel());
        }

        // POST: Wards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, WardCreateModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var ward = new Ward
                {
                    HospitalId = hospital.Id,
                    Name = model.Name
                };

                this.context.Add(ward);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewBag.Hospital = hospital;
            return this.View(model);
        }

        // GET: Wards/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            var model = new WardEditModel
            {
                Name = ward.Name
                
            };
            ViewBag.HospitalId = ward.HospitalId;

            return this.View(model);
        }

        // POST: Wards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                ward.Name = model.Name;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = ward.HospitalId });
            }

            return this.View(model);
        }

        // GET: Wards/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .Include(w => w.Hospital)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (ward == null)
            {
                return this.NotFound();
            }
            ViewBag.HospitalId = ward.HospitalId;
            return this.View(ward);
        }

        // POST: Wards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var ward = await this.context.Wards.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Wards.Remove(ward);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { hospitalId = ward.HospitalId });
        }
    }
}
