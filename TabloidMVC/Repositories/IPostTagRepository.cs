using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostTagRepository
    {
        List<Tags> GetPostTags(int id);
        List<Tags> GetTagsRemainderByPost(int id);
        void AddTagToPost(int PostId, int TagId);
    }
}
