using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;
using Backend6.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Backend6.Controllers
{
    [Authorize(Roles = ApplicationRoles.Administrators)]
    public class PostCategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public PostCategoriesController(ApplicationDbContext context)
        {
            this.context = context;    
        }

        // GET: PostCategories
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.PostCategories.ToListAsync());
        }

        // GET: PostCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postCategory = await this.context.PostCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return this.NotFound();
            }

            return this.View(postCategory);
        }

        // GET: PostCategories/Create
        public IActionResult Create()
        {
            return this.View(new PostCategoryEditModel());
        }

        // POST: PostCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCategoryEditModel model)
        {
            if (this.ModelState.IsValid)
            {
                var postCategory = new PostCategory
                {
                    Name = model.Name
                };

                this.context.PostCategories.Add(postCategory);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: PostCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postCategory = await this.context.PostCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return this.NotFound();
            }

            var model = new PostCategoryEditModel
            {
                Name = postCategory.Name
            };

            return this.View(model);
        }

        // POST: PostCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, PostCategoryEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postCategory = await this.context.PostCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                postCategory.Name = model.Name;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            return this.View(model);
        }

        // GET: PostCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postCategory = await this.context.PostCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postCategory == null)
            {
                return this.NotFound();
            }

            return this.View(postCategory);
        }

        // POST: PostCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var postCategory = await this.context.PostCategories.SingleOrDefaultAsync(m => m.Id == id);
            this.context.PostCategories.Remove(postCategory);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
