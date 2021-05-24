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
using Microsoft.AspNetCore.Identity;
using Backend6.Services;
using Microsoft.AspNetCore.Authorization;

namespace Backend6.Controllers
{
    public class ForumCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public ForumCategoriesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
        }
        [AllowAnonymous]
        // GET: ForumCategories
        public async Task<IActionResult> Index()
        {

            var items = await this._context.ForumCategories
                .Include(p => p.Forums)
                .ThenInclude(p => p.ForumTopics)
                .ToListAsync();
            return this.View(items);
        }

        // GET: ForumCategories/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null)
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // GET: ForumCategories/Create
        [Authorize]
        public IActionResult Create()
        {
            return View(new ForumCategoriesCreate());
        }

        // POST: ForumCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumCategoriesCreate model)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            if (ModelState.IsValid || user != null || User.IsInRole(ApplicationRoles.Administrators))
            {
                var category = new ForumCategory
                {
                    Name = model.Name
                };
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return this.View();
        }

        // GET: ForumCategories/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null || !this.userPermissions.CanEditForumCategory(forumCategory))
            {
                return NotFound();
            }
            var model = new ForumCategoriesEdit
            {
                Name = forumCategory.Name
            };
            return View(model);
        }

        // POST: ForumCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumCategoriesEdit model)
        {
            var forumCategory = await _context.ForumCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (id == null)
            {
                return NotFound();
            }
            if (forumCategory == null || !this.userPermissions.CanEditForumCategory(forumCategory))
                return NotFound();
            if (ModelState.IsValid)
            {
                forumCategory.Name = model.Name;
                await this._context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return this.View(model);
        }

        // GET: ForumCategories/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumCategory = await _context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null || !this.userPermissions.CanEditForumCategory(forumCategory))
            {
                return NotFound();
            }

            return View(forumCategory);
        }

        // POST: ForumCategories/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var forumCategory = await _context.ForumCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumCategory == null || !this.userPermissions.CanEditForumCategory(forumCategory))
            {
                return NotFound();
            }
            _context.ForumCategories.Remove(forumCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ForumCategoryExists(Guid id)
        {
            return _context.ForumCategories.Any(e => e.Id == id);
        }
    }
}
