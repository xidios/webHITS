using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend6.Data;
using Backend6.Models;
using Microsoft.AspNetCore.Identity;
using Backend6.Services;
using Backend6.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Backend6.Controllers
{
    public class ForumMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public ForumMessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            _context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
        }

        // GET: ForumMessages
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ForumMessages.Include(f => f.Creator).Include(f => f.ForumTopic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ForumMessages/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.Creator)
                .Include(f => f.ForumTopic)
                .Include(f=>f.ForumMessageAttachments)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null)
            {
                return NotFound();
            }

            return View(forumMessage);
        }
        [Authorize]
        // GET: ForumMessages/Create
        public async Task<IActionResult> Create(Guid? forumTopicId)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (forumTopicId == null)
                return NotFound();

            var forumTopic = await _context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == forumTopicId);

            if (forumTopic == null || user == null)
                return NotFound();

            ViewBag.ForumTopic = forumTopic;
            return View(new ForumMessagesCreateModel());
        }

        // POST: ForumMessages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumMessagesCreateModel model, Guid? forumTopicId)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (forumTopicId == null)
                return NotFound();

            var forumTopic = await _context.ForumTopics
                .SingleOrDefaultAsync(x => x.Id == forumTopicId);

            if (forumTopic == null || user == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                DateTime now = DateTime.UtcNow;
                var forumMessage = new ForumMessage
                {

                    Text = model.Text,
                    Created = now,
                    CreatorId = user.Id,
                    ForumTopicID = (Guid)forumTopicId
                };
                this._context.Add(forumMessage);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "ForumTopics", new { id = forumTopic.Id });

            }
            ViewBag.ForumTopic = forumTopic;
            return View(model);
        }

        // GET: ForumMessages/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages.SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null || !this.userPermissions.CanEditForumMessege(forumMessage))
            {
                return NotFound();
            }

            var model = new ForumMessagesEditModel
            {
                Text = forumMessage.Text
            };
            ViewBag.ForumMessage = forumMessage;
            return View(model);
        }

        // POST: ForumMessages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ForumMessagesEditModel model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages.SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null || !this.userPermissions.CanEditForumMessege(forumMessage))
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                forumMessage.Text = model.Text;
                forumMessage.Modified = DateTime.UtcNow;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "ForumTopics", new { id = forumMessage.ForumTopicID });
            }
            ViewBag.ForumMessage = forumMessage;
            return View(model);
        }

        // GET: ForumMessages/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null || !this.userPermissions.CanEditForumMessege(forumMessage))
            {
                return NotFound();
            }
            ViewBag.ForumMessage = forumMessage;
            return View(forumMessage);
        }

        // POST: ForumMessages/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessage = await _context.ForumMessages
                .Include(f => f.Creator)
                .Include(f => f.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessage == null || !this.userPermissions.CanEditForumMessege(forumMessage))
            {
                return NotFound();
            }
            var forumTopicsId = forumMessage.ForumTopicID;
            _context.ForumMessages.Remove(forumMessage);
            await _context.SaveChangesAsync();
            ViewBag.ForumMessage = forumMessage;
            return RedirectToAction("Details", "ForumTopics", new { id = forumTopicsId });
        }

        private bool ForumMessageExists(Guid id)
        {
            return _context.ForumMessages.Any(e => e.Id == id);
        }
    }
}
