using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication.Models
{
    public class AppUserAddressEntity
    {
        [Key, ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        public virtual AppUserEntity User { get; set; } = null!;
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
