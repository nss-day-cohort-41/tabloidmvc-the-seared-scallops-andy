using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository, 
            IPostRepository postRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }
        public IActionResult Details(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                    return NotFound();   
            }
            return View(category);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            try
            {
                _categoryRepository.AddCategory(category);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View(category);
            }
        }
        

        //Start checking here
        public IActionResult UserPostDetails(int id)
        {
            int userId = GetCurrentUserProfileId();
            var post = _postRepository.GetUserPostById(id, userId);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }



        //public IActionResult Create()
        //{
        //    var vm = new PostCreateViewModel();
        //    vm.CategoryOptions = _categoryRepository.GetAll();
        //    return View(vm);
        //}

        //[HttpPost]
        //public IActionResult Create(PostCreateViewModel vm)
        //{
        //    try
        //    {
        //        vm.Post.CreateDateTime = DateAndTime.Now;
        //        vm.Post.IsApproved = true;
        //        vm.Post.UserProfileId = GetCurrentUserProfileId();

        //        _postRepository.Add(vm.Post);

        //        return RedirectToAction("Details", new { id = vm.Post.Id });
        //    }
        //    catch
        //    {
        //        vm.CategoryOptions = _categoryRepository.GetAll();
        //        return View(vm);
        //    }
        //}

        public IActionResult Delete(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Category category)
        {
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _categoryRepository.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }

        public IActionResult Edit(int id)
        {
            Category category = _categoryRepository.GetCategoryById(id);
            if (category ==null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            try
            {
                _categoryRepository.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(category);
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


    }
}
