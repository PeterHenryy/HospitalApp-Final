using HospitalApp.Helpers.Enums;
using HospitalApp.Models;
using HospitalApp.Models.Admins.ViewModels;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Doctors.ViewModelForm;
using HospitalApp.Models.Identity;
using HospitalApp.Models.Patients;
using HospitalApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;

        public AdminController(UserManager<AppUser> userManager, DoctorService doctorService, PatientService patientService)
        {
            _userManager = userManager;
            _doctorService = doctorService;
            _patientService = patientService;
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

        public async Task<IActionResult> DisplayPatients()
        {
            IList<AppUser> patients= await _userManager.GetUsersInRoleAsync(UserRolesEnum.Patient.ToString());
            return View(patients);
        }

        public IActionResult DoctorAppointments(int doctorID)
        {
            List<Appointment> doctorAppointments = _doctorService.GetAppointmentsByDoctor(doctorID);
            return View(doctorAppointments);
        }

        public async Task<IActionResult> AppointmentDetails(int appointmentID)
        {
            Appointment appointment = _doctorService.GetAppointmentById(appointmentID);
            Bill bill = _doctorService.GetBillByAppointmentId(appointmentID);
            bill.Appointment.User = await _userManager.FindByIdAsync(appointment.UserID.ToString());
            PROMIS10 promis10 = _patientService.GetPROMIS10ByAppointmentID(appointmentID);
            List<BillItem> billItems = _doctorService.GetBillItemsByBillId(bill.Id);
            List<Doctor> doctors = _doctorService.GetAllDoctors();
            var appointmentDetailsViewModel = new AppointmentDetailsViewModel();
            appointmentDetailsViewModel.Appointment = appointment;
            appointmentDetailsViewModel.Bill = bill;
            appointmentDetailsViewModel.PROMIS10 = promis10;
            appointmentDetailsViewModel.BillItemsAdded = billItems;
            appointmentDetailsViewModel.Doctors = doctors;
            return View(appointmentDetailsViewModel);
        }
        [HttpPost]
        public IActionResult ReassignDoctorToAppointment(AppointmentDetailsViewModel appointmentDetailsViewModel)
        {
            Appointment appointment = _doctorService.GetAppointmentById(appointmentDetailsViewModel.AppointmentID);
            appointment.DoctorID = appointmentDetailsViewModel.DoctorID;
            bool updatedAppointment = _patientService.UpdateAppointment(appointment);
            return RedirectToAction("AppointmentDetails", "Admin", new { appointmentID = appointmentDetailsViewModel.AppointmentID });
        }
    }
}
