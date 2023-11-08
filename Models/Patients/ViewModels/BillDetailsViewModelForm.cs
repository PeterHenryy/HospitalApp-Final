using System.Collections.Generic;

namespace HospitalApp.Models.Patients.ViewModels
{
    public class BillDetailsViewModelForm
    {
        public Bill BillData { get; set; }
        public List<BillItem> BillItems { get; set; }
    }
}
