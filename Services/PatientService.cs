using HospitalApp.Models;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Interfaces;
using HospitalApp.Models.Patients;
using HospitalApp.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            List<Appointment> appointment = _patientRepository.GetAvaliablesAppointments();
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
      

 
    }
}
