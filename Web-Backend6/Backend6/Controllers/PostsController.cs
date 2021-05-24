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
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionsService userPermissions;

        public PostsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IUserPermissionsService userPermissions)
        {
            this.context = context;
            this.userManager = userManager;
            this.userPermissions = userPermissions;
        }

        // GET: Posts
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var items = await this.context.Posts
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .Include(p => p.Attachments)
                .Include(p => p.Comments)
                .ToListAsync();
            return this.View(items);
        }

        // GET: Posts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .Include(p => p.Attachments)
                .Include(p => p.Comments)
                .ThenInclude(p => p.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var postCategories = await this.context.PostCategories.OrderBy(x => x.Name).ToListAsync();
            this.ViewData["CategoryId"] = new SelectList(postCategories, "Id", "Name");
            return this.View(new PostEditModel());
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostEditModel model)
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            if (this.ModelState.IsValid && user != null)
            {
                var now = DateTime.UtcNow;
                var post = new Post
                {
                    CreatorId = user.Id,
                    Created = now,
                    Modified = now,
                    CategoryId = model.CategoryId,
                    Title = model.Title,
                    Text = model.Text
                };

                this.context.Posts.Add(post);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            var postCategories = await this.context.PostCategories.OrderBy(x => x.Name).ToListAsync();
            this.ViewData["CategoryId"] = new SelectList(postCategories, "Id", "Name");
            return this.View(model);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null || !this.userPermissions.CanEditPost(post))
            {
                return this.NotFound();
            }

            var model = new PostEditModel
            {
                CategoryId = post.CategoryId,
                Title = post.Title,
                Text = post.Text
            };

            var postCategories = await this.context.PostCategories.OrderBy(x => x.Name).ToListAsync();
            this.ViewData["CategoryId"] = new SelectList(postCategories, "Id", "Name");
            this.ViewBag.Post = post;
            return this.View(model);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, PostEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null || !this.userPermissions.CanEditPost(post))
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                post.CategoryId = model.CategoryId;
                post.Title = model.Title;
                post.Text = model.Text;
                post.Modified = DateTime.UtcNow;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }

            var postCategories = await this.context.PostCategories.OrderBy(x => x.Name).ToListAsync();
            this.ViewData["CategoryId"] = new SelectList(postCategories, "Id", "Name");
            this.ViewBag.Post = post;
            return this.View(model);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts
                .Include(p => p.Category)
                .Include(p => p.Creator)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (post == null || !this.userPermissions.CanEditPost(post))
            {
                return this.NotFound();
            }

            return this.View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var post = await this.context.Posts.SingleOrDefaultAsync(m => m.Id == id);

            if (post == null || !this.userPermissions.CanEditPost(post))
            {
                return this.NotFound();
            }

            this.context.Posts.Remove(post);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}
