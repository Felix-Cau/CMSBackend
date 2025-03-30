﻿using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    public class AppUserEntity : IdentityUser
    {
        [ProtectedPersonalData] 
        public string FirstName { get; set; } = null!;

        [ProtectedPersonalData] 
        public string LastName { get; set; } = null!;
        public string? JobTitle { get; set; }
        public virtual AppUserAddressEntity? Address { get; set; }

    }
}
