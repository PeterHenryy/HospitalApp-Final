using HospitalApp.Models.Identity;
using System.Collections.Generic;

namespace HospitalApp.Models.Doctors.ViewModelForm
{
    public class RegisterDoctorAndUserViewModelForm
    {
        public Doctor DoctorDataForm { get; set; }
        public AppUser  UserDataForm { get; set; }
        public List<string> DoctorRoles = new List<string>
        {
            "Cardiovascular",
            "Pulmonary",
            "Orthopedics",
            "Geriatrics",
            "Neurology",
            "Oncology",
            "Pediatrics",
            "Sports",
            "Clinical_Cardiac_Electrophysiology",
            "Pediatric_Physical_Therapy",
            "Wound_Management",
            "Vestibular",
            "Occupational_Therapy"
        };
    }
}
