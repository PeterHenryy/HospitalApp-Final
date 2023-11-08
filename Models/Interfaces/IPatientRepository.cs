using HospitalApp.Models.Doctors;
using System.Collections.Generic;

namespace HospitalApp.Models.Interfaces
{
    public interface IPatientRepository
    {
        List<Doctor> GetDoctorsDropdown();
    }
}
