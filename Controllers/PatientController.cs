using HospitalApp.Models;
using HospitalApp.Models.Identity;
using HospitalApp.Models.Patients;
using HospitalApp.Models.Patients.ViewModels;
using HospitalApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HospitalApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;
        private readonly UserService _userService;
        private readonly AppUser _currentUser;

        public PatientController(PatientService patientService, UserService userService)
        {
            _patientService = patientService;
            _userService = userService;
            _currentUser = userService.GetCurrentUser();
        }
        public IActionResult DisplayUserCreditCards()
        {
            List<CreditCard> creditCards = _patientService.GetSpecificUserCards(_currentUser.Id);
            return View(creditCards);
        }

        [HttpGet]
        public IActionResult CreateCreditCard()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCreditCard(CreditCard creditCard)
        {
            creditCard.UserID = _currentUser.Id;
            bool createdCard = _patientService.CreateCreditCard(creditCard);
            if (createdCard)
            {
                return RedirectToAction("DisplayUserCreditCards", "Patient");
            }
            return View();
        }
        //[HttpPost]
        public IActionResult SelectInsuranceAndDiscount(BillDetailsViewModelForm billDetailsAndForm)
        {

            return View();
        }


        public IActionResult DeleteCreditCard(int creditCardID)
        {
            bool deletedCard = _patientService.DeleteCreditCard(creditCardID);
            return RedirectToAction("DisplayUserCreditCards", "Patient");
        }
        public IActionResult BillDetails(int appointmentId)
        {
            var bill = _patientService.GetBillByAppointmentId(appointmentId);
            BillDetailsViewModelForm billDetails = new BillDetailsViewModelForm();
            billDetails.BillData = bill;
            billDetails.BillItems = _patientService.GetBillItemsByBillId(bill.Id);

            return View(billDetails);
        }
        public IActionResult MyAppointments()
        {
            IndexViewModel ivm = new IndexViewModel();
            ivm.ActiveAppointments = _patientService.GetActiveAppointmentsByUserId(_currentUser.Id);
            return View(ivm);
        }
        public IActionResult AppointmentIndex()
        {
            var appointments = _patientService.GetAvaliablesAppointments();
            return View(appointments);
        }
        [HttpGet]
        public IActionResult BookAppointment(int appointmentId, PROMIS10 promis)
        {
            var appointment = _patientService.GetAppointmentByID(appointmentId);
            appointment.UserID = _currentUser.Id;
            promis.AppointmentId = appointmentId;

            CreateAppointmentViewModel appointmentForm = new CreateAppointmentViewModel();
            appointmentForm.Appointment = appointment;
            return View(appointmentForm);
        }
        [HttpPost]
        public IActionResult BookAppointment(Appointment appointment, List<int> Answers, List<int> QuestionIds, int AppointmentId)
        {
            // Getting PROMIS object validated & ready to be stored in database
            var Promis10 = _patientService.ConvertListToPROMIS10(Answers);

            var createdAppointment = _patientService.BookAppointment(appointment, Promis10);
            if (!createdAppointment)
            {
                return View(appointment);
            }
            return RedirectToAction("AppointmentIndex");
        }

    }
}
