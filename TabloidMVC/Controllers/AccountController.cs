
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserProfile profile)
        {
            profile.CreateDateTime = DateTime.Now;
            profile.IdIsActive = 0;
            profile.UserTypeId = 2;
            var credentials = new Credentials()
            {
                Email = profile.Email
            };
            try
            {
                _userProfileRepository.AddNew(profile);
                return RedirectToAction("NewAccount", "Account", credentials);
            }
            catch
            {
                return View(profile);
            }
        }
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }
            //Checks to see if the account has been deactivated
            else if (userProfile.IdIsActive == 1)
            {
                return RedirectToAction("Deactivated", "Account");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
            };

            //Adds Role to user credentials if the user is an Administrator. Admin role will show more menu options
            if (userProfile.UserTypeId == 1)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> NewAccount(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }
            //Checks to see if the account has been deactivated
            else if (userProfile.IdIsActive == 1)
            {
                return RedirectToAction("Deactivated", "Account");
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Deactivated()
        {
            return View();
        }

        public async Task<IActionResult> AccountNewlyDeactivatedAsync()
        {
            await HttpContext.SignOutAsync();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DemotedToAuthor()
        {
            RemoveAdminClaim();
            return View();
        }
        public void RemoveAdminClaim()
        {
            var user = User;
            var identity = user.Identity as ClaimsIdentity;
            var claim = (from c in user.Claims
                         where c.Value == "Admin"
                         select c).Single();
            identity.RemoveClaim(claim);
        }
    }
}