using System.Collections.Generic;

namespace HospitalApp.Models.Patients.ViewModels
{
    public class BillAndTransactionDetailsViewModelForm
    {
        public string InsuranceName { get; set; }
        public Bill BillData { get; set; }
        public Transaction TransactionForm { get; set; }
        public int AppointmentId { get; set; }

        public List<BillItem> BillItems { get; set; }
    }
}
