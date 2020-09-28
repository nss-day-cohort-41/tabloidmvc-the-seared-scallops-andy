using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
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

        //GET only active users and list
        public IActionResult Index()
        {
            if (!_userProfileRepository.VerifyAdminStatus(GetCurrentUserProfileId()))
            {
                return RedirectToAction("AccountChangedRecently", "UserProfile");
            }
            List<UserProfile> users = _userProfileRepository.GetAllUsers(0); 
            return View(users);
        }

        //GET deactivated users

        public IActionResult ShowDeactivated()
        {
            if (!_userProfileRepository.VerifyAdminStatus(GetCurrentUserProfileId()))
            {
                return RedirectToAction("AccountChangedRecently", "UserProfile");
            }
            List<UserProfile> deactivatedUsers = _userProfileRepository.GetAllUsers(1);
            if (deactivatedUsers.Count == 0)
            {
                return RedirectToAction("NoDeactivated", "UserProfile");
            }
            return View(deactivatedUsers);
        }

        // GET: ProfileController/Details/5
        public ActionResult Details(int id)
        {
            Verify();
            var user = _userProfileRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        

        // GET: ProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            if (!_userProfileRepository.VerifyAdminStatus(GetCurrentUserProfileId()))
            {
                return RedirectToAction("AccountChangedRecently", "UserProfile");
            }
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
            //Verify that the current user's Admin status hasn't changed recently
            if (!_userProfileRepository.VerifyAdminStatus(GetCurrentUserProfileId()))
            {
                return RedirectToAction("AccountChangedRecently", "UserProfile");
            }
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
                            profile.IdIsActive = 0;
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
                //Verify that the account to be deactivated is not the last Admin
                else if (profile.IdIsActive == 1)
                {
                    var numOfAdmins = _userProfileRepository.CheckForActiveAdmins();
                    if (numOfAdmins.Count == 0)
                    {
                        //Change the user type back to active and re-submit
                        
                        profile.IdIsActive = 0;
                        try
                        {
                            _userProfileRepository.UpdateUserProfile(profile);
                            return RedirectToAction("LastAdminDeactivateError", "UserProfile", profile);
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

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

//Error views//
        //View shown if the someone attempted to demote the only Admin in the system
        public ActionResult LastAdminError(UserProfile profile)
        {
            return View(profile);
        }

        public ActionResult LastAdminDeactivateError(UserProfile profile)
        {
            return View(profile);
        }

        public ActionResult NoDeactivated()
        {
            return View();
        }

        public ActionResult AccountChangedRecently()
        {
            try
            {
                UserProfile currentUser = _userProfileRepository.GetById(GetCurrentUserProfileId());
                if (currentUser.UserTypeId == 2)
                {
                    return RedirectToAction("DemotedToAuthor", "Account");
                }
                else if (currentUser.IdIsActive == 1)
                {
                    return RedirectToAction("AccountNewlyDeactivated", "Account");
                }
                return RedirectToAction("AdminError", "UserProfile");
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult AdminError()
        {
            return View();
        }

        public ActionResult Verify()
        {
            if (!_userProfileRepository.VerifyAdminStatus(GetCurrentUserProfileId()))
            {
                return RedirectToAction("AccountChangedRecently", "UserProfile");
            }

            return null;
        }
    }
}
