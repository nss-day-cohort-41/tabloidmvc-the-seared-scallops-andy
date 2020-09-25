using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        // GET: PostTagController
        public ActionResult ManageAdd(int id)
        {
            int userProfileId = GetCurrentUserProfileId();
            var post = _postRepository.GetUserPostById(id, userProfileId);
            var tags = _postTagRepository.GetTagsRemainderByPost(post.Id);
            var vm = new PostTagViewModel()
            {
                Post = post,
                TagList = tags
            };

            if (post == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageAdd(int postId, int tagId)
        {
            int userProfileId = GetCurrentUserProfileId();
            var post = _postRepository.GetUserPostById(postId, userProfileId);
            var tags = _postTagRepository.GetTagsRemainderByPost(post.Id);
            var vm = new PostTagViewModel()
            {
                Post = post,
                TagList = tags
            };
            try
            {
                _postTagRepository.AddTagToPost(postId, tagId);
                return RedirectToAction("UserPostDetails", "Post", new { @id = postId });
            }
            catch (Exception ex)
            {
                return View(vm);
            }
        }
        // GET: PostTagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostTagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostTagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostTagController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostTagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PostTagController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostTagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
