using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models.Patients
{
    public class PROMIS10
    {
        // Properties
        [Key]
        public int Id { get; set; }
        public int Answer1 { get; set; } // 1-5
        public int Answer2 { get; set; } // 1-5
        public int Answer3 { get; set; } // 1-5
        public int Answer4 { get; set; } // 1-5
        public int Answer5 { get; set; } // 1-5
        public int Answer6 { get; set; } // 1-5
        public int Answer7 { get; set; } // 1-5

        // References

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public virtual Appointment Appointment { get; set; }

    }
}
