using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace HospitalApp.Models
{
    public class CreditCard
    {
        [Key]
        public int ID { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public int CVV { get; set; }
        public DateTime Expiry { get; set; }
        public decimal Balance { get; set; }

        [ForeignKey("AspNetUsers")]
        public int? UserID { get; set; }
    }
}
