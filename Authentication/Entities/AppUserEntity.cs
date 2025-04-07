using Microsoft.AspNetCore.Identity;

namespace Authentication.Entities
{
    public class AppUserEntity : IdentityUser
    {
        [ProtectedPersonalData] 
        public string FirstName { get; set; } = null!;

        [ProtectedPersonalData] 
        public string LastName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? JobTitle { get; set; }
        public virtual AppUserAddressEntity? Address { get; set; }

    }
}
