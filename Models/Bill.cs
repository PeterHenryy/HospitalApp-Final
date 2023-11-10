using HospitalApp.Models.Doctors;
using HospitalApp.Models.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace HospitalApp.Models
{
    public class Bill
    {
        [Key]
        public int Id { get; set; }

        public decimal Total { get; set; }
        public decimal OriginalTotal { get; set; }
        public int InsuranceId { get; set; } = 0;
        public string? DoctorsNotes { get; set; }
        public bool IsDoctorApproved { get; set; }
        // References
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }
    }
}
