using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    //Roles ensures only Admin can access the routes
    [Authorize(Roles = "Admin")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository, ICategoryRepository categoryRepository)
        {
            _userProfileRepository = userProfileRepository;
            _categoryRepository = categoryRepository;
        }
        // GET: ProfileController
        public IActionResult Index()
        {
            List<UserProfile> users = _userProfileRepository.GetAllUsers(); 
            return View(users);
        }

        // GET: ProfileController/Details/5
        public ActionResult Details(int id)
        {
            var user = _userProfileRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: ProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProfileController/Create
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

        // GET: ProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = new UserProfileAdminEditViewModel()
            {
                Profile = _userProfileRepository.GetById(id),
                Types = _userProfileRepository.GetAllTypes(),
            };
            if (vm.Profile == null)
            {
                return NotFound();
            }
            
            return View(vm);
        }

        // POST: ProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserProfile profile)
        {
            try
            {
                _userProfileRepository.UpdateUserProfile(profile);

                //if the submitted change was to demote a user, check to make sure they were not the last admin
                if (profile.UserTypeId == 2)
                    {
                    var numOfAdmins = _userProfileRepository.CheckForAdmins();
                    if(numOfAdmins.Count == 0)
                        {
                            //Change the user type back to admin and re-submit
                            profile.UserTypeId = 1;
                        try
                        {
                            _userProfileRepository.UpdateUserProfile(profile);
                            return RedirectToAction("LastAdminError", "UserProfile", profile);
                        }
                        catch
                        {
                            return NotFound();
                        }
                        }
                    }
                return RedirectToAction("Index", "UserProfile");
            }
            catch
                {
                    var vm = new UserProfileAdminEditViewModel()
                    {
                        Profile = profile,
                        Types = _userProfileRepository.GetAllTypes(),
                    };
                    return View(vm);
                }
        }

        // GET: ProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProfileController/Delete/5
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

        //View shown if the someone attempted to demote the only Admin in the system
        public ActionResult LastAdminError(UserProfile profile)
        {
            return View(profile);
        }
    }
}
