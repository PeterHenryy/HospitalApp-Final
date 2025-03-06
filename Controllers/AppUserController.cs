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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUserName = await _userManager.FindByNameAsync(appUser.UserName);
                if(existingUserName != null)
                {
                    ModelState.AddModelError("userName", "A user with this username already exists");
                    return View(appUser);
                }
                var existingUserEmail = await _userManager.FindByEmailAsync(appUser.Email);
                if (existingUserEmail != null)
                {
                    // If email is already taken, add a model error
                    ModelState.AddModelError("Email", "This email address is already in use");
                    return View(appUser); // Return the view with the model and error message
                }

                // Set the default profile picture
                appUser.ProfilePicture = "user-profile-pic.svg";

                // Register the user
                var userRegister = await _userManager.CreateAsync(appUser, appUser.Password);
                if (userRegister.Succeeded)
                {
                    // Assign the role after successful registration
                    var role = UserRolesEnum.Patient.ToString();
                    var assignRole = await _userManager.AddToRoleAsync(appUser, role);
                    if (assignRole.Succeeded)
                    {
                        // Redirect to the Login page if everything goes well
                        return RedirectToAction("Login", "AppUser");
                    }
                    else
                    {
                        // If role assignment fails, add errors and return to the view
                        foreach (var error in assignRole.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(appUser);
                    }
                }
                else
                {
                    // If user registration fails, add errors and return to the view
                    foreach (var error in userRegister.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(appUser);
                }
            }

            // If the model state is not valid, return to the view
            return View(appUser);
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
            if (ModelState.IsValid && appLogin.Username != null)
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
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Doctor"))
                        {
                            return RedirectToAction("MyAppointments", "Doctor"); 
                        }
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("DoctorsIndex", "Admin");

                        }
                        return RedirectToAction("DoctorIndex", "Patient");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid password.");
                    return View(appLogin);
                }
                catch (Exception e)
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
