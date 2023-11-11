using HospitalApp.Models.Repositories;
using System;
using System.Collections.Generic;

namespace HospitalApp.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepository;

        public AdminService(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public decimal CalculateDoctorRevenue(int doctorID)
        {
            decimal revenue = _adminRepository.CalculateDoctorRevenue(doctorID);
            return revenue;
        }

        public decimal CalculateHospitalRevenue()
        {
            decimal revenue = _adminRepository.CalculateHospitalRevenue();
            return revenue;
        }

        public static bool HasOccurred(DateTime eventDateTime)
        {
            // Get the current date and time
            DateTime currentDateTime = DateTime.Now;

            // Compare the eventDateTime with the currentDateTime
            return eventDateTime <= currentDateTime;
        }
    }
}
