using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    public class AppUser : IdentityUser
    {
        [ProtectedPersonalData]
        public string? FirstName { get; set; }
        [ProtectedPersonalData]
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public virtual AppUserAddress? Address { get; set; }

    }
}
