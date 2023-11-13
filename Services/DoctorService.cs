using HospitalApp.Models;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Services
{
    public class DoctorService
    {
        private readonly DoctorRepository _doctorRepository;

        public DoctorService(DoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            List<Appointment> appointment = _doctorRepository.GetAppointmentsByDoctor(doctorId)
                                                                    .Where(x => x.IsBooked)
                                                                        .ToList();
            return appointment;
        }
        public Bill GetBillById(int billId)
        {
            Bill bill = _doctorRepository.GetBillById(billId);
            return bill;
        }
        public void RejectAppointmentById(int appointmentId)
        {
            Appointment appointment = _doctorRepository.GetAppointmentById(appointmentId);
            appointment.IsRejected = true;
            _doctorRepository.RejectAppointment(appointment);
        }
        public List<BillItem> GetBillItemsByBillId(int billId)
        {
            List<BillItem> billItems = _doctorRepository.GetBillItemsByBillId(billId);
            return billItems;
        }
        public Appointment GetAppointmentById(int appointmentId)
        {
            Appointment appointment = _doctorRepository.GetAppointmentById(appointmentId);
            return appointment;
        }
        public bool Create(Doctor doctor)
        {
            bool createdDoctor = _doctorRepository.Create(doctor);
            return createdDoctor;
        }
        public bool UpdateBill(Bill bill)
        {
            // This column indicates if bill is ready to be seen by Patient
            var isCreated = _doctorRepository.UpdateBill(bill);
            return isCreated;

        }
        public bool SendBillToPatient(Bill bill)
        {
            // This column indicates if bill is ready to be seen by Patient
            bill.IsDoctorApproved = true;
            var isCreated = _doctorRepository.UpdateBill(bill);
            return isCreated;

        }
        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = _doctorRepository.GetAllDoctors();
            return doctors;
        }

        public bool Delete(int doctorID)
        {
            bool deletedDoctor = _doctorRepository.Delete(doctorID);
            return deletedDoctor;
        }


        public Doctor GetDoctorByID(int doctorID)
        {
            Doctor doctor = _doctorRepository.GetDoctorByID(doctorID);
            return doctor;
        }

        public bool CreateAppointment(Appointment appointment)
        {
            bool createdAppointment = _doctorRepository.CreateAppointment(appointment);
            return createdAppointment;
        }
        public Bill CreateBill(Bill bill)
        {
            var createdBill = _doctorRepository.CreateBill(bill);
            var updatedBill = _doctorRepository.GetUpdatedBillByAppointmentId(bill.AppointmentId);
            return updatedBill;
        }

        public Bill GetBillByAppointmentId(int appointmentId)
        {
            Bill bill = _doctorRepository.GetUpdatedBillByAppointmentId(appointmentId);
            return bill;
        }
        public bool CreateBillItems(BillItem bill)
        {
            bool createdBill = _doctorRepository.CreateBillItems(bill);
            return createdBill;
        }
        public Doctor GetDoctorByUserID(int userID)
        {
            Doctor doctor = _doctorRepository.GetDoctorByUserID(userID);
            return doctor;
        }
    }
}
