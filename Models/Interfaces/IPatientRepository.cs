using HospitalApp.Models.Doctors;
using HospitalApp.Models.Patients;
using System.Collections.Generic;

namespace HospitalApp.Models.Interfaces
{
    public interface IPatientRepository
    {
        List<Doctor> GetDoctorsDropdown();
        PROMIS10 GetPROMIS10ByAppointmentID(int appointmentID);
    }
}
