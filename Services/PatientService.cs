using HospitalApp.Models;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Interfaces;
using HospitalApp.Models.Patients;
using HospitalApp.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Services
{
    public class PatientService
    {
        private readonly PatientRepository _patientRepository;

        public PatientService(PatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public List<Doctor> GetDoctorsDropdown()
        {
            List<Doctor> doctors = _patientRepository.GetDoctorsDropdown();
            return doctors;
        }
        public List<Appointment> GetAvaliablesAppointments()
        {
            List<Appointment> appointment = _patientRepository.GetAvailableAppointments();
            return appointment;
        }
        public List<BillItem> GetBillItemsByBillId(int billId)
        {
            List<BillItem> billItems = _patientRepository.GetBillItemsByBillId(billId);
            return billItems;
        }
        public List<Appointment> GetActiveAppointmentsByUserId(int userId)
        {
            List<Appointment> appointments = _patientRepository.GetActiveAppointmentsByUserId(userId);

            // Grabbing attached bill to appointment
            foreach(var appointment in appointments)
            {
                appointment.AttachedBill = GetBillByAppointmentId(appointment.ID);
            }
            return appointments;
        }
        public Appointment GetAppointmentByID(int appointmentId)
        {
            Appointment appointment = _patientRepository.GetAppointmentByID(appointmentId);
            return appointment;
        }
        public Bill GetBillByAppointmentId(int appointmentId)
        {
            Bill bill = _patientRepository.GetBillByAppointmentId(appointmentId);
            return bill;
        }
        public bool BookAppointment(Appointment appointment, PROMIS10 promis10)
        {
            promis10.AppointmentId = appointment.ID;
            return _patientRepository.BookAppointment(appointment, promis10);
        }
        public PROMIS10 ConvertListToPROMIS10(List<int> answers)
        {
            PROMIS10 newPROMIS10 = new PROMIS10()
            {
                Answer1 = answers[0],
                Answer2 = answers[1],
                Answer3 = answers[2],
                Answer4 = answers[3],
                Answer5 = answers[4],
                Answer6 = answers[5],
                Answer7 = answers[6] 
            };
            return newPROMIS10;
        }
        public bool UpdateBill(Bill bill)
        {
            return _patientRepository.UpdateBill(bill);
        }

        public PROMIS10 GetPROMIS10ByAppointmentID(int appointmentID)
        {
            PROMIS10 promis10 = _patientRepository.GetPROMIS10ByAppointmentID(appointmentID);
            return promis10;
        }

        public bool UpdateAppointment(Appointment appointment)
        {
            bool updatedAppointment = _patientRepository.UpdateAppointment(appointment);
            return updatedAppointment;
        }
        public bool CreateReview(Review review)
        {
            return _patientRepository.CreateReview(review);
        }
        public bool CreateTransaction(Transaction transaction, int appointmentId)
        {
            var isCreated =  _patientRepository.CreateTransaction(transaction);
            if (isCreated)
            {
                var appointment = GetAppointmentByID(appointmentId);
                appointment.IsPaid = true;
                var isUpdated = _patientRepository.UpdateAppointment(appointment);
                return isUpdated;
            }
            return isCreated;
        }

        public List<Bill> GetPatientBills(int patientID)
        {
            List<Bill> patientBills = _patientRepository.GetPatientBills(patientID);
            return patientBills;
        }

        public List<Appointment> GetAppointmentsByDoctorID(int doctorID)
        {
            List<Appointment> doctorAppointments = _patientRepository.GetAvailableAppointments().Where(x => x.DoctorID == doctorID ).ToList();
            return doctorAppointments;
        }

        public List<Review> GetReviewsByDoctorID(int doctorID)
        {
            List<Review> doctorReviews = _patientRepository.GetReviewsByDoctorID(doctorID);
            return doctorReviews;
        }

        public List<Review> GetPatientLatestReviewsOnDoctor(int doctorID)
        {
            List<Review> patientLatestReviews = GetReviewsByDoctorID(doctorID)
                                                    .GroupBy(x => x.Appointment.UserID)
                                                        .Select(group => group.OrderByDescending(x => x.Appointment.AppointmentDate).FirstOrDefault())
                                                            .ToList();
            return patientLatestReviews;
        }
    }
}
