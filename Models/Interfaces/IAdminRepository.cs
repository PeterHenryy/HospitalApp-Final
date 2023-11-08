using HospitalApp.Models.Identity;
using System.Collections.Generic;

namespace HospitalApp.Models.Interfaces
{
    public interface IAdminRepository
    {
        IEnumerable<AppUser> GetAllDoctors();
    }
}
