using HospitalApp.Helpers.Enums;
using HospitalApp.Models;
using HospitalApp.Models.Admins.ViewModels;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Identity;
using HospitalApp.Models.Patients;
using HospitalApp.Models.Patients.ViewModels;
using HospitalApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;
        private readonly UserService _userService;
        private readonly DoctorService _doctorService;
        private readonly AppUser _currentUser;

        public PatientController(PatientService patientService, UserService userService, DoctorService doctorService)
        {
            _patientService = patientService;
            _userService = userService;
            _doctorService = doctorService;
            _currentUser = userService.GetCurrentUser();
        }

        public IActionResult SelectInsuranceAndDiscount(BillAndTransactionDetailsViewModelForm billDetailsAndForm)
        {
            var bill = billDetailsAndForm.BillData;
            var percentageOff = Convert.ToDecimal(InsuranceTypesEnum.InsuranceCoverage.ElementAt(bill.InsuranceId - 1).Value);
            var discountedTotal = InsuranceTypesEnum.CalculatePercentage(bill.Total, percentageOff);
            bill.OriginalTotal = bill.Total;
            bill.Total = discountedTotal;
            var updatedBill = _patientService.UpdateBill(bill);
            return RedirectToAction("BillPayment", "Patient", new {appointmentId = bill.AppointmentId});
        }

        public IActionResult BillPayment(int appointmentId)
        {
            var bill = _patientService.GetBillByAppointmentId(appointmentId);
            BillAndTransactionDetailsViewModelForm billDetails = new BillAndTransactionDetailsViewModelForm();
            billDetails.BillData = bill;
            billDetails.BillItems = _patientService.GetBillItemsByBillId(bill.Id);

            return View(billDetails);
        }
        public IActionResult CreateTransaction(BillAndTransactionDetailsViewModelForm transactionFormInfo)
        {
            Transaction transacation = transactionFormInfo.TransactionForm;
            var isCreated = _patientService.CreateTransaction(transacation, transactionFormInfo.AppointmentId);
            if (!isCreated)
            {
                return RedirectToAction("BillDetails", new { appointmentId = transactionFormInfo.AppointmentId });
            }
			return RedirectToAction("MyAppointments");

		}

		public IActionResult MyAppointments()
        {
            IndexViewModel ivm = new IndexViewModel();
            ivm.ActiveAppointments = _patientService.GetActiveAppointmentsByUserId(_currentUser.Id);
            return View(ivm);
        }
         
        public IActionResult DoctorIndex()
        {
            var doctors = _patientService.GetAvaliablesAppointments().Select(x => x.Doctor).Distinct().ToList();
            return View(doctors);
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
            return RedirectToAction("doctorIndex");
        }
        [HttpGet]
        public IActionResult AddReview(int appointmentId)
        {
            Review review = new Review();
            review.AppointmentId = appointmentId;
            return View(review);
        }
        public IActionResult AddReview(Review review)
        {
            var isCreated = _patientService.CreateReview(review);
            return RedirectToAction("doctorIndex");
        }

        public IActionResult PatientBills()
        {
            List<Bill> patientBills = _patientService.GetPatientBills(_currentUser.Id);
            return View(patientBills);
        }

        public IActionResult AppointmentDetails(int appointmentID)
        {
            List<BillItem> billItems = new List<BillItem>();
            Appointment appointment = _patientService.GetAppointmentByID(appointmentID);
            Bill bill = _patientService.GetBillByAppointmentId(appointmentID);
            if (bill != null)
            {
                billItems = _patientService.GetBillItemsByBillId(bill.Id);
            }
            PROMIS10 promis10 = _patientService.GetPROMIS10ByAppointmentID(appointmentID);
            var appointmentDetailsViewModel = new AppointmentDetailsViewModel();
            appointmentDetailsViewModel.Appointment = appointment;
            appointmentDetailsViewModel.Bill = bill;
            appointmentDetailsViewModel.PROMIS10 = promis10;
            appointmentDetailsViewModel.BillItemsAdded = billItems;
            return View(appointmentDetailsViewModel);
        }

        public IActionResult DoctorAndAppointmentsDetails(int doctorID)
        {
            var doctorDetailsViewModel = new DoctorDetailsViewModel();
            doctorDetailsViewModel.Doctor = _doctorService.GetDoctorByID(doctorID);
            doctorDetailsViewModel.DoctorAppointments = _patientService.GetAppointmentsByDoctorID(doctorID).Where(x => !x.IsBooked && !x.IsRejected).ToList();
			doctorDetailsViewModel.OrganizeAppointmentsByMonths();
			doctorDetailsViewModel.Reviews = _patientService.GetPatientLatestReviewsOnDoctor(doctorID);
            return View(doctorDetailsViewModel);
        }
    }
}
