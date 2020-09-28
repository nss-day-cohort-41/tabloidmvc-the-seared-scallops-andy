using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostTagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostTagRepository _postTagRepository;

        public PostTagController(ITagRepository tagRepository,
                                 IPostRepository postRepository,
                                 IPostTagRepository postTagRepository)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
        }
        // GET: PostTagController/Create
        public ActionResult Create(int id)
        {
            var allTags = _tagRepository.GetAllTags();
            var postTags = _postTagRepository.GetPostTagsByPostId(id);
            List<int> currentTagIds = new List<int>();
            foreach(PostTag pt in postTags)
            {
                currentTagIds.Add(pt.TagId);
            }
            foreach(Tags t in allTags)
            {
                var tagsId = t.Id;
                if(currentTagIds.Contains(tagsId))
                {
                    t.Selected = true;
                }
                else
                {
                    t.Selected = false;
                }
            }

            var post = _postRepository.GetPublishedPostById(id);
            var vm = new PostTagViewModel()
            {
                PostTagList = allTags,
                Post = post
            };
            return View(vm);
        }

        // POST: PostTagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create (PostTagViewModel vm, int id)
        {
            vm.Post = _postRepository.GetPublishedPostById(id);
            vm.PostTagList = _tagRepository.GetTagsByPostId(id);
            vm.AllTags = _tagRepository.GetAllTags();
            try
            {
                foreach(Tags t in vm.PostTagList)
                {
                    var postTag = _postTagRepository.GetPostTagbyPostWithTag(t.Id, id);
                    if(postTag != null)
                    {
                        _postTagRepository.DeletePostTag(postTag.Id);
                    }
                    
                }


                if (vm.IsSelected != null)
                {
                    foreach (int tagId in vm.IsSelected)

                    {
                        var aPostTag = new PostTag()
                        {
                            PostId = id,
                            TagId = tagId
                        };
                        _postTagRepository.AddTagToPost(aPostTag);



                    }
                    return RedirectToAction("UserPostDetails", "Post", new { id = id });
                }
                else
                {
                    return RedirectToAction("LastTagError", "PostTag", vm);
                }

             }
            catch
            {
                return View(vm);
            }
        }

        public ActionResult LastTagError(PostTagViewModel vm)
        {
            
            return View(vm);
        }
    }
}
