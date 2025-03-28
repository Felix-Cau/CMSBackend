using Authentication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Contexts
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<AppUserAddress> UserAddresses { get; set; }
    }
}
