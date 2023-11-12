using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models.Patients
{
    public class Review
    {
        // Properties
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; } // 1-5

        // References
        [ForeignKey("Appointments")]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}
