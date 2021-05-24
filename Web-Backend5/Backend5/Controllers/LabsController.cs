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
    public class LabsController : Controller
    {
        private readonly ApplicationDbContext context;

        public LabsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Labs
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Labs.ToListAsync());
        }

        // GET: Labs/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lab == null)
            {
                return this.NotFound();
            }

            return this.View(lab);
        }

        // GET: Labs/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Labs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LabCreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var lab = new Lab
                {
                    Name = model.Name,
                    Address = model.Address,
                    Phones = new Collection<LabPhone>()
                };
                if (model.Phones != null)
                {
                    var phoneId = 1;
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        lab.Phones.Add(new LabPhone
                        {
                            PhoneId = phoneId++,
                            Number = phone
                        });
                    }
                }

                this.context.Add(lab);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: Labs/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (lab == null)
            {
                return this.NotFound();
            }

            var model = new LabEditModel
            {
                Name = lab.Name,
                Address = lab.Address,
                Phones = String.Join(", ", lab.Phones.OrderBy(x => x.PhoneId).Select(x => x.Number))
            };

            return this.View(model);
        }

        // POST: Labs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, LabEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .Include(x => x.Phones)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (lab == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                lab.Name = model.Name;
                lab.Address = model.Address;
                var phoneId = lab.Phones.Any() ? lab.Phones.Max(x => x.PhoneId) + 1 : 1;
                lab.Phones.Clear();
                if (model.Phones != null)
                {
                    foreach (var phone in model.Phones.Split(',').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)))
                    {
                        lab.Phones.Add(new LabPhone
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

        // GET: Labs/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var lab = await this.context.Labs
                .SingleOrDefaultAsync(m => m.Id == id);
            if (lab == null)
            {
                return this.NotFound();
            }

            return this.View(lab);
        }

        // POST: Labs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var lab = await this.context.Labs.SingleOrDefaultAsync(m => m.Id == id);
            this.context.Labs.Remove(lab);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
