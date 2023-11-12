using HospitalApp.Data;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Interfaces;
using HospitalApp.Models.Patients;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Models.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool CreateReview(Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        public bool CreateTransaction(Transaction transaction)
        {
            try
            {
                _context.Transactions.Add(transaction);
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
        public List<Doctor> GetDoctorsDropdown()
        {
            List<Doctor> doctors = _context.Doctors.Include(x => x.User).ToList();
            return doctors;
        }
        public List<BillItem> GetBillItemsByBillId(int billId)
        {
            List<BillItem> billItems = _context.BillItems.Where(x => x.BillId == billId).ToList();
            return billItems;
        }
        public List<Appointment> GetAvailableAppointments()
        {
            List<Appointment> appointment = _context.Appointments
                                                        .Include(x => x.Doctor)
                                                            .Include(x => x.Doctor.User)
                                                                .Include(x => x.User)
                                                                    .Where(x => x.IsBooked == false).ToList();
            return appointment;
        }
        public List<Appointment> GetActiveAppointmentsByUserId(int userId)
        {
            List<Appointment> appointment = _context.Appointments
                                                 .Include(x => x.User)
                                                    .Where(x => x.IsBooked == true && x.IsPaid == false && x.UserID == userId).ToList();
            return appointment;
        }
        public Bill GetBillByAppointmentId(int appointmentId)
        {
            var bill = _context.Bills
                        .Include(x => x.Appointment) 
                            .FirstOrDefault(x => x.AppointmentId == appointmentId);
            return bill;
        }
        public Appointment GetAppointmentByID(int appointmentId)
        {
            Appointment appointment = _context.Appointments.FirstOrDefault(x => x.ID == appointmentId);
            return appointment;
        }
        public bool BookAppointment(Appointment appointment, PROMIS10 promis10)
        {
            try
            {
                _context.Appointments.Update(appointment);
                _context.PROMIS10s.Add(promis10);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public PROMIS10 GetPROMIS10ByAppointmentID(int appointmentID)
        {
            PROMIS10 promis10 = _context.PROMIS10s.SingleOrDefault(x => x.AppointmentId == appointmentID);
            return promis10;
        }

        public bool UpdateAppointment(Appointment appointment)
        {
            try
            {
                _context.Appointments.Update(appointment);
                _context.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public List<Bill> GetPatientBills(int patientID)
        {
            List<Bill> patientBills = _context.Bills.Include(x => x.Appointment).Where(x => x.Appointment.UserID == patientID && x.Appointment.IsPaid).ToList();
            return patientBills;
        }
    }
}
