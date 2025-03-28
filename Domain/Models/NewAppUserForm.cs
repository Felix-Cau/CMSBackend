using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class NewAppUserForm
    {
        [Required] 
        public string Email { get; set; } = null!;
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public string Role { get; set; } = null!;
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
