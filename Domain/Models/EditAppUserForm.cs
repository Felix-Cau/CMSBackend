using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class EditAppUserForm
    {
        [Required]
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? JobTitle { get; set; }
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = null!;
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
