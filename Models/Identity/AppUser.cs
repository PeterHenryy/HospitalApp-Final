using Microsoft.AspNetCore.Identity;

namespace HospitalApp.Models.Identity
{
    public class AppUser : IdentityUser<int>
    {
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
    }
}
