using Authentication.Models;

namespace Authentication.Interfaces
{
    public interface IJwtTokenHandler
    {
        string GenerateToken(AppUser appUser, string? role = null);
    }
}