using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class HospitalsController : Controller
    {
        private readonly ApplicationDbContext context;

        public HospitalsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Hospitals
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Hospitals.ToListAsync());
        }

        // GET: Hospitals/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            return this.View(hospital);
        }

        // GET: Hospitals/Create
        public IActionResult Create()
        {
            return this.View(new HospitalCreateModel());
        }

        // POST: Hospitals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospitalCreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var hospital = new Hospital
                {
                    Name = model.Name,
                    Address = model.Address,
                    Phones = new Collection<HospitalPhone>()
                };
                if (model.Phones != null)
                {
                    var phoneId = 1;
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        hospital.Phones.Add(new HospitalPhone
                        {
                            PhoneId = phoneId++,
                            Number = phone
                        });
                    }
                }

                this.context.Hospitals.Add(hospital);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: Hospitals/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            var model = new HospitalEditModel
            {
                Name = hospital.Name,
                Address = hospital.Address,
                Phones = String.Join(", ", hospital.Phones.OrderBy(x => x.PhoneId).Select(x => x.Number))
            };

            return this.View(model);
        }

        // POST: Hospitals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, HospitalEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                hospital.Name = model.Name;
                hospital.Address = model.Address;
                var phoneId = hospital.Phones.Any() ? hospital.Phones.Max(x => x.PhoneId) + 1 : 1;
                hospital.Phones.Clear();
                if (model.Phones != null)
                {
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        hospital.Phones.Add(new HospitalPhone
                        {
                            PhoneId = phoneId++,
                            Number = phone
                        });
                    }
                }

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: Hospitals/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return this.NotFound();
            }

            return this.View(hospital);
        }

        // POST: Hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var hospital = await this.context.Hospitals.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Hospitals.Remove(hospital);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
