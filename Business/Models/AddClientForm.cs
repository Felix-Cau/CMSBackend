using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Business.Models
{
    public class AddClientForm
    {
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string ClientName { get; set; } = null!;

        [Required]
        public string ClientEmail { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        [Required]
        public string City { get; set; } = null!;
        public string? Reference { get; set; }
    }
}
