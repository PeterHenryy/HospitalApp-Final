using HospitalApp.Models.Doctors;
using System.Collections.Generic;
using System.Linq;

namespace HospitalApp.Models.Patients.ViewModels
{
    public class DoctorDetailsViewModel
    {
        public Doctor Doctor { get; set; }
        public List<Appointment> DoctorAppointments { get; set; }
        public List<Review> Reviews { get; set; }

        public string CalculateDoctorAverageRating()
        {
            double averageRating = Reviews.Sum(x => x.Rating) / (double)Reviews.Count();
            return averageRating.ToString("F1");
        }

    }
}
