using HospitalApp.Models.Doctors;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;


namespace HospitalApp.Models.Patients.ViewModels
{
    public class DoctorDetailsViewModel
    {
        public Doctor Doctor { get; set; }
        public List<Appointment> DoctorAppointments { get; set; }
        public Dictionary<int, List<Appointment>> AppointmentsByMonths { get; set; }
        public List<Review> Reviews { get; set; }

        public string CalculateDoctorAverageRating()
        {
            double averageRating = Reviews.Sum(x => x.Rating) / (double)Reviews.Count();
            return averageRating.ToString("F1");
		}
		public void OrganizeAppointmentsByMonths()
		{
			AppointmentsByMonths = new Dictionary<int, List<Appointment>>();

			foreach (var appointment in DoctorAppointments)
			{
				int month = appointment.AppointmentDate.Month;

				if (!AppointmentsByMonths.ContainsKey(month))
				{
					AppointmentsByMonths[month] = new List<Appointment>();
				}

				AppointmentsByMonths[month].Add(appointment);
			}
		}
		public string GetMonthNameByNumber(int month)
		{
			return CultureInfo.CurrentCulture.
				DateTimeFormat.GetMonthName
				(month);
		}


	}
}
