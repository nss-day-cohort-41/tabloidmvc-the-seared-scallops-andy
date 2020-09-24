using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        List<Comment> GetAllCommentsFromPost(int postId);
    }
}