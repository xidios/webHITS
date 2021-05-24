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
    public class ForumController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public ForumController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(Guid? forumId)
        {
            if (forumId == null)
                return this.NotFound();
            var items = await this._context.Forums
                .Where(x=>x.Id==forumId)
                .ToListAsync();
            return this.View(items);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? forumId)
        {
            if (forumId == null)
                return this.NotFound();

            var forum = await this._context.Forums
                .Include(x=>x.ForumTopics)               
                .ThenInclude(x=>x.Creator)
                .Include(x=>x.ForumTopics)
                .ThenInclude(x=>x.ForumMessages)
                .ThenInclude(x=>x.Creator)
                .Where(x => x.Id == forumId)
                .SingleOrDefaultAsync();

            if (forum == null)
            {
                return this.NotFound();
            }

            return this.View(forum);
        }
        // GET: Forum/Create
        [Authorize]
        public async Task<IActionResult> Create(Guid? forumCategoryId)
        {
            if (forumCategoryId == null)
                return this.NotFound();


            var forumCategory = await this._context.ForumCategories
               .SingleOrDefaultAsync(m => m.Id == forumCategoryId);

            if (forumCategory == null || !this.userPermissions.CanEditForumCategory(forumCategory))
            {
                return this.NotFound();
            }
            ViewBag.ForumCategory = forumCategory;
            return this.View(new ForumCreateModel());
        }

        // POST: Forum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? forumCategoryId, ForumCreateModel model)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (forumCategoryId == null)
                return this.NotFound();

            var forumCategory = await this._context.ForumCategories
                .SingleOrDefaultAsync(x => x.Id == forumCategoryId);

            if (forumCategory == null || user == null || !User.IsInRole(ApplicationRoles.Administrators))
                return this.NotFound();
            if (this.ModelState.IsValid)
            {
                var forum = new Forum
                {
                    ForumCategoryId = (Guid)forumCategoryId,
                    Name = model.Name,
                    Description = model.Description

                };
                this._context.Add(forum);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index");

            }
            ViewBag.ForumCategory = forumCategory;
            return this.View(model);
        }

        // GET: Forum/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums.SingleOrDefaultAsync(m => m.Id == id);
            if (forum == null || !this.userPermissions.CanEditForum(forum))
            {
                return NotFound();
            }
            var model = new ForumEditModel
            {
                Name = forum.Name,
                Description = forum.Description
            };
            ViewBag.ForumCategory = forum.ForumCategory;
            return View(model);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, ForumEditModel model)
        {
            if (id == null)
            {
                return NotFound();
            }
            var forum = await _context.Forums
                .SingleOrDefaultAsync(x => x.Id == id);

            if(forum == null | !this.userPermissions.CanEditForum(forum))
                return NotFound();

            if (ModelState.IsValid)
            {
                forum.Name = model.Name;
                forum.Description = model.Description;
                await this._context.SaveChangesAsync();
                return RedirectToAction("Index","ForumCategories");
            }
            ViewBag.ForumCategory = forum.ForumCategory;
            return View(model);
        }

        // GET: Forum/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? forumId)
        {
            if (forumId == null)
            {
                return NotFound();
            }

            var forum = await _context.Forums
                .Include(f => f.ForumCategory)
                .SingleOrDefaultAsync(m => m.Id == forumId);

            if (forum == null || !this.userPermissions.CanEditForum(forum))
            {
                return NotFound();
            }

            return View(forum);
        }

        // POST: Forum/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? forumId)
        {
            if (forumId == null)
            {
                return this.NotFound();
            }

            var forum = await this._context.Forums.SingleOrDefaultAsync(m => m.Id == forumId);

            if (forum == null || !this.userPermissions.CanEditForum(forum))
            {
                return this.NotFound();
            }
            
            _context.Forums.Remove(forum);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ForumExists(Guid id)
        {
            return _context.Forums.Any(e => e.Id == id);
        }
    }
}
