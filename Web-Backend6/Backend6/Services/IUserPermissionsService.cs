using System;
using Backend6.Models;

namespace Backend6.Services
{
    public interface IUserPermissionsService
    {
        Boolean CanEditPost(Post post);

        Boolean CanEditPostComment(PostComment postComment);
        Boolean CanEditForumCategory(ForumCategory forumCategory);
        Boolean CanEditForum(Forum forum);
        Boolean CanEditForumTopic(ForumTopic forumTopic);
        Boolean CanEditForumMessege(ForumMessage forumMessage);
    }
}