using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostTagRepository
    {

        public List<PostTag> GetPostTagsByPostId(int id);
        PostTag GetPostTagbyPostWithTag(int tagId, int postId);
        public void AddTagToPost(PostTag postTag);
        void DeletePostTag(int postTagId);
        void DeleteAssociatedPostTags(int tagId);
    }
}
