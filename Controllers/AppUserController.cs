using HospitalApp.Helpers.Enums;
using HospitalApp.Models.Identity;
using HospitalApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HospitalApp.Controllers
{
    public class AppUserController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly UserService _userService;
        private readonly AppUser _currentUser;

        public AppUserController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManger, UserService userService)
        {
            _userManager = userManger;
            _userService = userService;
            _signInManager = signInManager;
            _currentUser = userService.GetCurrentUser();

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<RedirectToActionResult> Register(AppUser appUser)
        {
            appUser.ProfilePicture = "user-profilepic.png";
            var role = UserRolesEnum.Patient.ToString();
            var userRegister = await _userManager.CreateAsync(appUser);
            var assignRole = await _userManager.AddToRoleAsync(appUser, role);

            return RedirectToAction("Login", "AppUser");
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AppLogin appLogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(appLogin.Username);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Username not found.");
                    return View(appLogin);
                }

                try
                {
                    bool correctPassword = user.Password.Equals(appLogin.Password);
                    if (correctPassword)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("DoctorIndex", "Patient");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid password.");
                    return View(appLogin);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during login.");
                    return View(appLogin);
                }
            }

            return View(appLogin);
        }




        public async Task<RedirectToActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "AppUser");
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View(_currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AppUser updatedUser)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                updatedUser.ProfilePicture = files[0].FileName;
                _userService.HandleUserProfilePicture(files);
            }
            updatedUser.SecurityStamp = Guid.NewGuid().ToString();
            AppUser mappedUser = await _userService.MapUserUpdates(updatedUser, _currentUser);
            var user = await _userManager.UpdateAsync(mappedUser);
            return RedirectToAction("DoctorIndex", "Patient");
        }



    }
}
