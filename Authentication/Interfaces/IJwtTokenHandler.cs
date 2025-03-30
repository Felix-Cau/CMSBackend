using Authentication.Models;

namespace Authentication.Interfaces
{
    public interface IJwtTokenHandler
    {
        string GenerateToken(AppUserEntity appUser, string? role = null);
    }
}