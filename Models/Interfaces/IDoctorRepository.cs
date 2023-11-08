using HospitalApp.Models.Doctors;
using System.Collections.Generic;

namespace HospitalApp.Models.Interfaces
{
    public interface IDoctorRepository
    {
        bool Create(Doctor doctor);
        bool Delete(int doctorID);
        Doctor GetDoctorByID(int doctorID);
        List<Doctor> GetAllDoctors();
        bool CreateAppointment(Appointment appointment);
    }
}
