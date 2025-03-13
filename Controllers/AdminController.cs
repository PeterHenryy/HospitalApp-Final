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
        private readonly AdminService _adminService;
        //private readonly IBlobService _blobService;
        private readonly UserService _userService;
        private readonly AppUser _currentUser;

        public AdminController(UserManager<AppUser> userManager, UserService userService, DoctorService doctorService, PatientService patientService, AdminService adminService)
        {
            _userManager = userManager; 
            _userService = userService;
            _currentUser = userService.GetCurrentUser();
            _doctorService = doctorService;
            _patientService = patientService;
            _adminService = adminService;
            //_blobService = blobService;
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
            appUser.ProfilePicture = "user-doctor-solid.svg";
            
			// Creating role 
			var role = UserRolesEnum.Doctor.ToString();

            // Creating user
            var userRegister = await _userManager.CreateAsync(appUser);

            // Assigning doctor role to user
            var assignRole = await _userManager.AddToRoleAsync(appUser, role);

            // Creating doctor instance and registering it to database
            Doctor newDoctor = doctor.DoctorDataForm;
            newDoctor.User = appUser;
            newDoctor.Bio = "Specialist at Ineza Physiotherapy Clinic";
            newDoctor.ProfilePictureURI = "/profilepics/user-doctor-solid.svg";

            bool addedDoctor = _doctorService.Create(newDoctor);
            return RedirectToAction("DoctorsIndex", "Admin");

        }

        public IActionResult DoctorsIndex()
        {
            List<Doctor> doctors = _doctorService.GetAllDoctors();
            List<decimal> revenues = new List<decimal>();
            foreach (var doctor in doctors)
            {
                decimal doctorRevenue = _adminService.CalculateDoctorRevenue(doctor.ID);
                revenues.Add(doctorRevenue);
            }
            ViewBag.DoctorRevenues = revenues;
            ViewBag.Revenue = _adminService.CalculateHospitalRevenue();
            return View(doctors);
        }
		public IActionResult PatientsIndex()
		{
			List<AppUser> patients = _doctorService.GetAllPatients();
			return View(patients);
		}

		public IActionResult Remove(int doctorID, int userID)
        {
            bool removedDoctor = _doctorService.Remove(doctorID);
            return RedirectToAction("DoctorsIndex", "Admin");
        }

        public async Task<IActionResult> DisplayPatients()
        {
            IList<AppUser> patients= await _userManager.GetUsersInRoleAsync(UserRolesEnum.Patient.ToString());
            return View(patients);
        }

        public IActionResult DoctorAppointments(int doctorID)
        {
            List<Appointment> doctorAppointments = _doctorService.GetAppointmentsByDoctor(doctorID).ToList();
            return View(doctorAppointments);
        }

        public async Task<IActionResult> AppointmentDetails(int appointmentID)
        {
            List<BillItem> billItems = new List<BillItem>();
            Appointment appointment = _doctorService.GetAppointmentById(appointmentID);
            appointment.User = await _userManager.FindByIdAsync(appointment.UserID.ToString());
            Bill bill = _doctorService.GetBillByAppointmentId(appointmentID);
            if(bill != null)
            {
                billItems = _doctorService.GetBillItemsByBillId(bill.Id);
                bill.Appointment.User = await _userManager.FindByIdAsync(appointment.UserID.ToString());
            }
            PROMIS10 promis10 = _patientService.GetPROMIS10ByAppointmentID(appointmentID);
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

        [HttpGet]
        public IActionResult CreateAppointment(int doctorId)
        {
            List<string> appointmentTimes = new List<string>();

            for (int hour = 1; hour <= 11; hour++)
            {
                appointmentTimes.Add($"{hour}:00 AM");
            }
            appointmentTimes.Add("12:00 PM");
            for (int hour = 1; hour <= 11; hour++)
            {
                appointmentTimes.Add($"{hour}:00 PM");
            }
            appointmentTimes.Add("12:00 AM");

            ViewBag.AppointmentTimes = appointmentTimes;
            Appointment appointment = new Appointment()
            {
                DoctorID = doctorId
            };
            return View(appointment);
        }

        [HttpPost]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            appointment.IsBooked = false;
            bool createdAppointment = _doctorService.CreateAppointment(appointment);
            if (createdAppointment)
            {
                return RedirectToAction("DoctorsIndex", "Admin");
            }
            return View();
        }
        public IActionResult CancelAppointment(int appointmentId)
        {
            var appointment = _patientService.GetAppointmentByID(appointmentId);
            appointment.IsRejected = true;
            var isUpdated = _patientService.UpdateAppointment(appointment);
            return RedirectToAction("DoctorAppointments", "Admin", new { doctorID = appointment.DoctorID });
        }
    }
}
