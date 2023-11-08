using HospitalApp.Models.Doctors;
using System.Collections.Generic;

namespace HospitalApp.Models.Patients.ViewModels
{
    public class CreateAppointmentViewModel
    {
        public Appointment Appointment { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
