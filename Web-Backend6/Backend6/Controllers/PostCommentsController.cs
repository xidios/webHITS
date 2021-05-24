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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Backend6.Controllers
{
    [Authorize]
    public class PostCommentsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public PostCommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            this.context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
        }

        // GET: PostComments/Create
        public async Task<IActionResult> Create(Guid? postId)
        {
            if (postId == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts
                .SingleOrDefaultAsync(m => m.Id == postId);
            if (post == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Post = post;
            return this.View(new PostCommentEditModel());
        }

        // POST: PostComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid? postId, PostCommentEditModel model)
        {
            if (postId == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts
                .SingleOrDefaultAsync(m => m.Id == postId);
            if (post == null)
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.HttpContext.User);

            if (this.ModelState.IsValid)
            {
                var now = DateTime.UtcNow;
                var comment = new PostComment
                {
                    PostId = post.Id,
                    CreatorId = user.Id,
                    Created = now,
                    Modified = now,
                    Text = model.Text
                };
                
                this.context.Add(comment);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Details", "Posts", new { id = post.Id });
            }

            this.ViewBag.Post = post;
            return this.View(model);
        }

        // GET: PostComments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postComment = await this.context.PostComments
                .Include(x => x.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postComment == null || !this.userPermissions.CanEditPostComment(postComment))
            {
                return this.NotFound();
            }

            var model = new PostCommentEditModel
            {
                Text = postComment.Text
            };

            this.ViewBag.Post = postComment.Post;
            return this.View(model);
        }

        // POST: PostComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, PostCommentEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postComment = await this.context.PostComments
                .Include(x => x.Post)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (postComment == null || !this.userPermissions.CanEditPostComment(postComment))
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                postComment.Text = model.Text;
                postComment.Modified = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Details", "Posts", new { id = postComment.PostId });
            }

            this.ViewBag.Post = postComment.Post;
            return this.View(model);
        }

        // GET: PostComments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postComment = await this.context.PostComments
                .Include(p => p.Creator)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (postComment == null || !this.userPermissions.CanEditPostComment(postComment))
            {
                return this.NotFound();
            }

            return this.View(postComment);
        }

        // POST: PostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var postComment = await this.context.PostComments
                .Include(p => p.Creator)
                .Include(p => p.Post)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (postComment == null || !this.userPermissions.CanEditPostComment(postComment))
            {
                return this.NotFound();
            }

            this.context.PostComments.Remove(postComment);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Details", "Posts", new { id = postComment.PostId });
        }
    }
}
