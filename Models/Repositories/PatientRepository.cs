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
        public List<Appointment> GetAvaliablesAppointments()
        {
            List<Appointment> appointment = _context.Appointments.Where(x => x.IsBooked == false).ToList();
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
    }
}
