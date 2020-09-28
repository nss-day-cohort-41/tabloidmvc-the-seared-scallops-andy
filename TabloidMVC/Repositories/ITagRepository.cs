using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
      List<Tags> GetAllTags();
      Tags GetTagById(int id);
      List<Tags> GetTagsByPostId(int id);
      void AddTag(Tags tag);
      void DeleteTag(int id);
        void UpdateTag(Tags tag);
    }
}

