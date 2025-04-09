using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class EditAppUserForm
    {
        [Required]
        public string Id { get; set; } = null!;

        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
        public string? JobTitle { get; set; }
        public string? PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; } = null!;
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
