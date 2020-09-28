using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Tools;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReadTimeCalculator _readTimeCalculator;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, IReadTimeCalculator readTimeCalculator)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _readTimeCalculator = readTimeCalculator;
        }

        public IActionResult Index()
        {
            var controllers = _postRepository.GetAllPublishedPosts();
            return View(controllers);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            post.Readtime = _readTimeCalculator.CalculateReadTime(post.Content);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);

                
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult UserPostDetails(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetUserPostById(id, userId);
            post.Readtime = _readTimeCalculator.CalculateReadTime(post.Content);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        public IActionResult Edit(int id)
        {
            PostCreateViewModel vm = new PostCreateViewModel();

            vm.CategoryOptions = _categoryRepository.GetAll();
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();

                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            vm.Post = post;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.UpdatePost(post);
                return RedirectToAction("UserPosts", "Post");
            }
            catch
            {
                PostCreateViewModel vm = new PostCreateViewModel();

                vm.CategoryOptions = _categoryRepository.GetAll();
                vm.Post = post;

                return View(vm);
            }
        }

        public IActionResult UserPosts()
        {
            int user = GetCurrentUserProfileId();
            List<Post> userPosts = _postRepository.GetAllUserPost(user);

            return View(userPosts);
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        public IActionResult simpleDelete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult simpleDelete(int id, Post post)
        {
            try
            {
                _postRepository.DeletePost(id);
                return RedirectToAction("UserPosts", "Post");
            }
            catch (Exception ex)
            {
                return View(post);
            }

        }
    }
}