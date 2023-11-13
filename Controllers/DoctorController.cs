using HospitalApp.Models;
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
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly UserService _userService;
        private readonly AppUser _currentUser;
        private readonly UserManager<AppUser> _userManager;

        public DoctorController(DoctorService doctorService, UserService userService, UserManager<AppUser> userManager)
        {
            _doctorService = doctorService;
            _userService = userService;
            _currentUser = userService.GetCurrentUser();
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateAppointment()
        {
            List<string> appointmentTimes = new List<string>();

            for (int hour = 1; hour <= 12; hour++)
            {
                appointmentTimes.Add($"{hour}:00 AM");
            }

            for (int hour = 1; hour <= 12; hour++)
            {
                appointmentTimes.Add($"{hour}:00 PM");
            }

            ViewBag.AppointmentTimes = appointmentTimes;
            return View();
        }

        [HttpPost]
        public IActionResult CreateAppointment(Appointment appointment)
        {
            Doctor doctor = _doctorService.GetDoctorByUserID(_currentUser.Id);
            appointment.DoctorID = doctor.ID;
            appointment.IsBooked = false;
            bool createdAppointment = _doctorService.CreateAppointment(appointment);
            if (createdAppointment)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
         
        public IActionResult MyAppointments()
        {
            var doctorId = _doctorService.GetDoctorByUserID(_currentUser.Id).ID;
            var appointments = _doctorService.GetAppointmentsByDoctor(doctorId);
            return View(appointments);
        }
        public async Task<IActionResult> BillingForAppointment(int appointmentId)
        {
            // Grabbing appointment Object which we are billing
            var appointment = _doctorService.GetAppointmentById(appointmentId);
   
            // Bill that will be passed in
            var billObject = new Bill(); // Temp holder

            // Checking if bill exists by appointment Id
            var billExist = _doctorService.GetBillByAppointmentId(appointmentId);

            // If Bill doesns't exist, create a new one & pass it in
            if (billExist == null)
            {
                // Creating NEW Bill object for first time
                Bill bill = new Bill();
                bill.Appointment = appointment;
                bill.AppointmentId = appointment.ID;
                bill.Total = appointment.InitialTotal;
                bill.OriginalTotal = appointment.InitialTotal;
                // Creating Bill to have id as reference for BillItems
                billObject = _doctorService.CreateBill(bill);
            }
            else
            {
                // If already exist in the system, pass it in
                billObject = billExist;
            }
            // We are grabbing all related items/medication to this bill & updating the price on Bill
            var billItems = _doctorService.GetBillItemsByBillId(billObject.Id);

            // Creating ViewModel & passing info in
            BillViewModel billViewModel = new BillViewModel();
            billObject.Appointment.User = await _userManager.FindByIdAsync(appointment.UserID.ToString());
            billViewModel.BillInfo = billObject;
            billViewModel.BillItemsAdded = billItems;
            billViewModel.BillInfo.Total = billObject.Total;
            return View(billViewModel);
        }
        public IActionResult CreateBillItems(BillViewModel billViewModel)
        {
            // Creating Bill Item
            BillItem newbillItem = billViewModel.BillItemForm;
            bool createdBill = _doctorService.CreateBillItems(newbillItem);

            // Getting Bill
            var bill = _doctorService.GetBillById(newbillItem.BillId);
            // Updating Total
            bill.Total = bill.Total + newbillItem.Price;
            // Updating Record in Database
            _doctorService.UpdateBill(bill);
            // Redirect
            return RedirectToAction("BillingForAppointment", new { appointmentId = billViewModel.BillInfo.AppointmentId });
        }
        public IActionResult SendBillToPatient(BillViewModel bill)
        {
            // This column indicates if bill is ready to be seen by Patient
            var isCreated = _doctorService.SendBillToPatient(bill.BillForm);
            if (!isCreated)
            {
                return RedirectToAction("BillingForAppointment");
            }
            return RedirectToAction("MyAppointments");

        }
        public IActionResult RejectAppointment(int appointmentId)
        {
            _doctorService.RejectAppointmentById(appointmentId);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
