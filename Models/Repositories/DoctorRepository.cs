using HospitalApp.Data;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Models.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Create(Doctor doctor)
        {
            try
            {
                _context.Doctors.Add(doctor);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool Delete(int doctorID)
        {
            try
            {
                Doctor doctor = GetDoctorByID(doctorID);
                _context.Doctors.Remove(doctor);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            List<Appointment> appointment = _context.Appointments.
                                                Where(x => x.Doctor.ID == doctorId)
                                                    .Include(x => x.User)
                                                            .ToList();
            return appointment;
        }

        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = _context.Doctors.Include(x => x.User).ToList();
            return doctors;
        }

        public Doctor GetDoctorByID(int doctorID)
        {
            Doctor doctor = _context.Doctors.Include(x => x.User).SingleOrDefault(x => x.ID == doctorID);
            return doctor;
        }
        public Bill GetBillById(int billId)
        {
            Bill bill = _context.Bills
                                    .Include(x => x.Appointment)
                                            .SingleOrDefault(x => x.Id == billId);
            return bill;
        }
        public Appointment GetAppointmentById(int appointmentId)
        {
            Appointment appointment = _context.Appointments
                                         .Include(x => x.Doctor)
                                            .SingleOrDefault(x => x.ID == appointmentId);
            return appointment;
        }
        public List<BillItem> GetBillItemsByBillId(int billId)
        {
            List<BillItem> billItems = _context.BillItems.Where(x => x.BillId == billId).ToList();
            return billItems;
        }

        public bool CreateAppointment(Appointment appointment)
        {
            try
            {
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool CreateBillItems(BillItem bill)
        {
            try
            {
                _context.BillItems.Add(bill);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool UpdateBill(Bill bill)
        {
            try
            {
                _context.Bills.Update(bill);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public bool UpdateDoctor(Doctor doctor)
        {
            try
            {
                _context.Doctors.Update(doctor);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public Bill GetUpdatedBillByAppointmentId(int appointmentId)
        {
            Bill bill = _context.Bills.Include(x => x.Appointment).FirstOrDefault(x => x.AppointmentId == appointmentId);
            return bill;
        }
        public Bill CreateBill(Bill bill)
        {
            try
            {
                _context.Bills.Add(bill);
                _context.SaveChanges();
                return bill;
            }
            catch (System.Exception)
            {

                return null;
            }
        }
        public Doctor GetDoctorByUserID(int userID)
        {
            Doctor doctor = _context.Doctors.SingleOrDefault(x => x.UserID == userID);
            return doctor;
        }
        public void RejectAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            _context.SaveChanges();
        }
    }
}
