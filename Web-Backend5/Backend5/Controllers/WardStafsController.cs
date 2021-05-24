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
    public class WardStafsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStafsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardStafs
        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            var wardStafs = await this._context.WardStafs
                .Include(w => w.Ward)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            return this.View(wardStafs);
        }

        // GET: WardStafs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaf = await _context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaf == null)
            {
                return NotFound();
            }
            
            return View(wardStaf);
        }

        // GET: WardStafs/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            return this.View(new WardStafCreateModel());
        }

        // POST: WardStafs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, WardStafCreateModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                var wstaf = new WardStaf
                {
                    WardId=ward.Id,
                    Name = model.Name,
                    Postion = model.Postion
                };
                this._context.WardStafs.Add(wstaf);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { wardId= ward.Id});
            }
            this.ViewBag.Ward = ward;
            return this.View(model);
        }

        // GET: WardStafs/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaf = await _context.WardStafs.SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaf == null)
            {
                return this.NotFound();
            }
            var model = new WardStafEditModel { 
                Name = wardStaf.Name,
                Postion=wardStaf.Postion
            };
            ViewBag.WardId = wardStaf.WardId;
            return this.View(model);
        }

        // POST: WardStafs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardStafEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }
            var wardStaf = await this._context.WardStafs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaf == null)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                wardStaf.Name = model.Name;
                wardStaf.Postion = model.Postion;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new {wardId = wardStaf.WardId });
            }
            ViewBag.WardId = wardStaf.WardId;
            return this.View(model);
        }

        // GET: WardStafs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaf = await _context.WardStafs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaf == null)
            {
                return NotFound();
            }
            return View(wardStaf);
        }

        // POST: WardStafs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 wardId,int id)
        {
            var wardStaf = await _context.WardStafs.SingleOrDefaultAsync(m => m.Id == id);
            _context.WardStafs.Remove(wardStaf);
            await _context.SaveChangesAsync();
            return this.RedirectToAction("Index", new {wardId = wardId });
        }

        private bool WardStafExists(int id)
        {
            return _context.WardStafs.Any(e => e.Id == id);
        }
    }
}
