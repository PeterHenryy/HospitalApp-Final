using HospitalApp.Models.Doctors;
using HospitalApp.Models.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApp.Models
{
    public class Appointment
    {
        // Properties
        [Key]
        public int ID { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal InitialTotal { get; set; }
        public bool IsBooked { get; set; }  
        public bool IsPaid { get; set; }  
        public bool IsRejected { get; set; }  
        public string Notes { get; set; }
        //References
        [ForeignKey("AspNetUsers")]
        [Display(Name = "PatientID")]
        public int? UserID { get; set; }
        public virtual AppUser User {get; set;}

        [ForeignKey("Doctors")] 
        public int DoctorID { get; set; }
        public virtual Doctor Doctor { get; set; }

        // This is for runtime
        public virtual Bill AttachedBill { get; set; }

    }
}
