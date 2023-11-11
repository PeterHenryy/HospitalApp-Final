using HospitalApp.Data;
using HospitalApp.Models.Doctors;
using HospitalApp.Models.Identity;
using HospitalApp.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Models.Repositories
{
    public class AdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Bill> GetPaidBills()
        {
            List<Bill> bills = _context.Bills.Include(x => x.Appointment).Where(x => x.Appointment.IsPaid).ToList();
            return bills;
        }

        public List<Bill> GetBillsByDoctorID(int doctorID)
        {
            List<Bill> bills = _context.Bills.Include(x => x.Appointment).Where(x => x.Appointment.DoctorID == doctorID).ToList();
            return bills;
        }

        public decimal CalculateDoctorRevenue(int doctorID)
        {
            List<Bill> doctorBills = GetBillsByDoctorID(doctorID);
            decimal revenue = 0;
            if(doctorBills != null)
            {
                revenue = doctorBills.Where(x => x.Appointment.IsPaid).Sum(x => x.OriginalTotal);
            }
            return revenue;
        }

        public decimal CalculateHospitalRevenue()
        {
            List<Bill> paidBills = GetPaidBills();
            decimal revenue = 0;
            if(paidBills != null)
            {
                revenue = paidBills.Sum(x => x.OriginalTotal);
            }
            return revenue;
        }
    }
}
