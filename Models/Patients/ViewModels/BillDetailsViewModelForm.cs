using System.Collections.Generic;

namespace HospitalApp.Models.Patients.ViewModels
{
    public class BillDetailsViewModelForm
    {
        public string InsuranceName { get; set; }
        public Bill BillData { get; set; }
        public List<BillItem> BillItems { get; set; }
    }
}
