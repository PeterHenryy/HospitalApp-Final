using Microsoft.AspNetCore.Identity;

namespace HospitalApp.Models.Identity
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
