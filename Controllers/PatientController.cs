using HospitalApp.Helpers.Enums;
using HospitalApp.Models;
using HospitalApp.Models.Identity;
using HospitalApp.Models.Patients;
using HospitalApp.Models.Patients.ViewModels;
using HospitalApp.Services;
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
        private readonly AppUser _currentUser;

        public PatientController(PatientService patientService, UserService userService)
        {
            _patientService = patientService;
            _userService = userService;
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
            return RedirectToAction("BillDetails", "Patient", new {appointmentId = bill.AppointmentId});
        }
        public IActionResult BillDetails(int appointmentId)
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
