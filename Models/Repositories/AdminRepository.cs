using HospitalApp.Data;
using HospitalApp.Models.Identity;
using HospitalApp.Models.Interfaces;
using System.Collections.Generic;

namespace HospitalApp.Models.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<AppUser> GetAllDoctors()
        {
            return null;
        }
    }
}
