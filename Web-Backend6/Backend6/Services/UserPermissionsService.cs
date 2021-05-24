using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Backend6.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public UserPermissionsService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        private HttpContext HttpContext => this.httpContextAccessor.HttpContext;

        public Boolean CanEditPost(Post post)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == post.CreatorId;
        }

        public Boolean CanEditPostComment(PostComment postComment)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == postComment.CreatorId;
        }
        public Boolean CanEditForumCategory(ForumCategory forumCategory)
        {
            
            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return false;
        }
        public Boolean CanEditForum(Forum forum)
        {

            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return false;
        }
        public Boolean CanEditForumTopic(ForumTopic forumTopic)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == forumTopic.CreatorId;
        }

        public Boolean CanEditForumMessege(ForumMessage forumMessage)
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.HttpContext.User.IsInRole(ApplicationRoles.Administrators))
            {
                return true;
            }

            return this.userManager.GetUserId(this.httpContextAccessor.HttpContext.User) == forumMessage.CreatorId;
        }

    }
}
