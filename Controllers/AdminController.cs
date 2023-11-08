using HospitalApp.Helpers.Enums;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Doctors.ViewModelForm;
using HospitalApp.Models.Identity;
using HospitalApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DoctorService _doctorService;

        public AdminController(UserManager<AppUser> userManager, DoctorService doctorService)
        {
            _userManager = userManager;
            _doctorService = doctorService;
        }

        [HttpGet]
        public IActionResult RegisterDoctor()
        {
            var registerDoctorForm = new RegisterDoctorAndUserViewModelForm();
            return View(registerDoctorForm);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> RegisterDoctor(RegisterDoctorAndUserViewModelForm doctor)
        {
            // Creating AppUser and registering to database with "Doctor" role
            AppUser appUser = doctor.UserDataForm;

            // Creating role 
            var role = UserRolesEnum.Doctor.ToString();

            // Creating user
            var userRegister = await _userManager.CreateAsync(appUser);

            // Assigning doctor role to user
            var assignRole = await _userManager.AddToRoleAsync(appUser, role);

            // Creating doctor instance and registering it to database
            Doctor newDoctor = doctor.DoctorDataForm;
            newDoctor.User = appUser;
            bool addedDoctor = _doctorService.Create(newDoctor);
            return RedirectToAction("DoctorsIndex", "Admin");

        }

        public IActionResult DoctorsIndex()
        {
            List<Doctor> doctors = _doctorService.GetAllDoctors();
            return View(doctors);
        }

        public async Task<IActionResult> Delete(int doctorID, int userID)
        {
            AppUser user = _userManager.FindByIdAsync(userID.ToString()).Result;
            bool deletedDoctor = _doctorService.Delete(doctorID);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("DoctorsIndex", "Admin");
        }

    }
}
