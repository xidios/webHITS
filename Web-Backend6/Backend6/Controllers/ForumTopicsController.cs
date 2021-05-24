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
using Backend6.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Backend6.Controllers
{
    public class ForumTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public ForumTopicsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
        }

        // GET: ForumTopics
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ForumTopics.Include(f => f.Forum);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ForumTopics/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Creator)
                .Include(f => f.Forum)
                .Include(f=>f.ForumMessages)
                .ThenInclude(f=>f.Creator)
                .Include(f => f.ForumMessages)
                .ThenInclude(f=>f.ForumMessageAttachments)                              
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null)
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // GET: ForumTopics/Create
        [Authorize]
        public async Task<IActionResult> Create(Guid? forumId)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (forumId == null)
                return this.NotFound();

            var forum = await _context.Forums
                .SingleOrDefaultAsync(x => x.Id == forumId);
            if (forum == null || user == null)
                return this.NotFound();
            ViewBag.Forum = forum;
            return View(new ForumTopicsCreateModel());
        }

        // POST: ForumTopics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumTopicsCreateModel model, Guid? forumId)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            if (forumId == null)
                return this.NotFound();

            var forum = await _context.Forums
                .SingleOrDefaultAsync(x => x.Id == forumId);
            if (forum == null || user == null)
                return this.NotFound();

            if (ModelState.IsValid)
            {
                DateTime now = DateTime.UtcNow;
                var forumTopic = new ForumTopic
                {

                    Name = model.Name,
                    Created = now,
                    ForumId = (Guid)forumId,
                    CreatorId = user.Id
                };
                this._context.Add(forumTopic);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "Forum", new { forumId = forumId });
            }

            ViewBag.Forum = forum;
            return this.View(model);
        }

        // GET: ForumTopics/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? forumTopicsId)
        {
            if (forumTopicsId == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == forumTopicsId);
            if (forumTopic == null || !this.userPermissions.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }
            var model = new ForumTopicsEditModel
            {
                Name = forumTopic.Name
            };
            ViewBag.ForumTopic = forumTopic;
            return View(model);
        }

        // POST: ForumTopics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? forumTopicsId, ForumTopicsEditModel model)
        {
            if (forumTopicsId == null)
            {
                return NotFound();
            }
            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == forumTopicsId);
            if (forumTopic == null || !this.userPermissions.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                DateTime now = DateTime.UtcNow;
                forumTopic.Name = model.Name;
                forumTopic.Modified = now;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", new { id = forumTopic.Id });
            }
            ViewBag.ForumTopic = forumTopic;
            return View(forumTopic);
        }

        // GET: ForumTopics/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumTopic = await _context.ForumTopics
                .Include(f => f.Forum)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumTopic == null || !this.userPermissions.CanEditForumTopic(forumTopic))
            {
                return NotFound();
            }

            return View(forumTopic);
        }

        // POST: ForumTopics/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
                return NotFound();

            var forumTopic = await _context.ForumTopics.SingleOrDefaultAsync(m => m.Id == id);

            if (forumTopic == null || !this.userPermissions.CanEditForumTopic(forumTopic))
                return NotFound();

            var forumId = forumTopic.ForumId;

            _context.ForumTopics.Remove(forumTopic);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details","Forum",new { forumId=forumId});
        }

        private bool ForumTopicExists(Guid id)
        {
            return _context.ForumTopics.Any(e => e.Id == id);
        }
    }
}
