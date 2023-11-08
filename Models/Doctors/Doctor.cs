using HospitalApp.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models.Doctors
{
    public class Doctor
    {
        // Properties

        [Key]
        public int ID { get; set; }
        public string DoctorRole { get; set; }

        // References
        [ForeignKey("AspNetUsers")]
        public int UserID { get; set; }
        public virtual AppUser User { get; set; }

    }

}
