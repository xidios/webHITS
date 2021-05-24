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
using Microsoft.AspNetCore.Hosting;
using Backend6.Models.ViewModels;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace Backend6.Controllers
{
    public class ForumMessageAttachmentsController : Controller
    {
        private static readonly HashSet<String> AllowedExtensions = new HashSet<String> { ".jpg", ".jpeg", ".png", ".gif" };

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;
        private readonly IHostingEnvironment hostingEnvironment;

        public ForumMessageAttachmentsController(ApplicationDbContext _context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions, IHostingEnvironment hostingEnvironment)
        {
            this._context = _context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: ForumMessageAttachments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumMessageAttachment = await _context.ForumMessageAttachments
                .Include(f => f.ForumMessage)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (forumMessageAttachment == null)
            {
                return NotFound();
            }

            return View(forumMessageAttachment);
        }

        // GET: ForumMessageAttachments/Create
        public async Task<IActionResult> Create(Guid? forumMessageId)
        {
            if (forumMessageId == null)
            {
                return this.NotFound();
            }

            var forumMessage = await this._context.ForumMessages
                .SingleOrDefaultAsync(m => m.Id == forumMessageId);
            if (forumMessage == null || !this.userPermissions.CanEditForumMessege(forumMessage))
            {
                return this.NotFound();
            }

            this.ViewBag.ForumMessage = forumMessage;
            return this.View(new ForumMessageAttachmentsEditModel());
        }

        // POST: ForumMessageAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? forumMessageId, ForumMessageAttachmentsEditModel model)
        {
            if (forumMessageId == null)
            {
                return this.NotFound();
            }

            var forumMessage = await this._context.ForumMessages
                .SingleOrDefaultAsync(m => m.Id == forumMessageId);
            if (forumMessage == null || !this.userPermissions.CanEditForumMessege(forumMessage))
            {
                return this.NotFound();
            }

            var fileName = Path.GetFileName(ContentDispositionHeaderValue.Parse(model.File.ContentDisposition).FileName.Value.Trim('"'));
            var fileExt = Path.GetExtension(fileName);
            if (!ForumMessageAttachmentsController.AllowedExtensions.Contains(fileExt))
            {
                this.ModelState.AddModelError(nameof(model.File), "This file type is prohibited");
            }

            if (this.ModelState.IsValid)
            {
                var forumMessageAttachment = new ForumMessageAttachment
                {
                    ForumMessageId = forumMessage.Id,
                    Created = DateTime.UtcNow,
                    FileName = fileName
                };

                var attachmentPath = Path.Combine(this.hostingEnvironment.WebRootPath, "attachments", forumMessageAttachment.Id.ToString("N") + fileExt);
                forumMessageAttachment.FilePath = $"/attachments/{forumMessageAttachment.Id:N}{fileExt}";
                using (var fileStream = new FileStream(attachmentPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                {
                    await model.File.CopyToAsync(fileStream);
                }

                this._context.Add(forumMessageAttachment);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Details", "ForumTopics", new { id = forumMessage.ForumTopicID });
            }

            this.ViewBag.ForumMessage = forumMessage;
            return this.View(model);
        }

       
        
        // GET: ForumMessageAttachments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var messageAttachment = await this._context.ForumMessageAttachments
                .Include(p => p.ForumMessage)
                .ThenInclude(p=>p.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (messageAttachment == null || !this.userPermissions.CanEditForumMessege(messageAttachment.ForumMessage))
            {
                return this.NotFound();
            }

            return this.View(messageAttachment);
        }

        // POST: ForumMessageAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var messageAttachment = await this._context.ForumMessageAttachments
                .Include(p => p.ForumMessage)
                .ThenInclude(p => p.ForumTopic)
                .SingleOrDefaultAsync(m => m.Id == id);
            var topicId = messageAttachment.ForumMessage.ForumTopicID;
            if (messageAttachment == null || !this.userPermissions.CanEditForumMessege(messageAttachment.ForumMessage))
            {
                return this.NotFound();
            }

            this._context.ForumMessageAttachments.Remove(messageAttachment);
            await this._context.SaveChangesAsync();
            return RedirectToAction("Details", "ForumTopics", new { id = topicId });
        }

        private bool ForumMessageAttachmentExists(Guid id)
        {
            return _context.ForumMessageAttachments.Any(e => e.Id == id);
        }
    }
}
