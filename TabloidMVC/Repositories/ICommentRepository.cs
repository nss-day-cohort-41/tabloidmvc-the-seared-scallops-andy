using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        void DeleteComment(int id);
        List<Comment> GetAllCommentsFromPost(int postId);
        Comment GetcommentById(int id);
    }
}