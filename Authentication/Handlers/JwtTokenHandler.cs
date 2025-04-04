using Authentication.Entities;
using Authentication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Handlers
{
    public class JwtTokenHandler(IConfiguration configuration) : IJwtTokenHandler
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(AppUserEntity appUser, string? role = null)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!);

                var issuer = _configuration["JWT:Issuer"]!;
                var audience = _configuration["JWT:Audience"]!;
                List<Claim> claims = new()
                {
                    new(ClaimTypes.NameIdentifier, appUser.Id),
                    new(ClaimTypes.Email, appUser.Email!),
                };

                if (role is not null)
                    claims.Add(new Claim(ClaimTypes.Role, role));

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Issuer = issuer,
                    Audience = audience,
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                JwtSecurityTokenHandler tokenHandler = new();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
    }
}
