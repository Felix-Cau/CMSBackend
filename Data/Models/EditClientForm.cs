using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class EditClientForm
    {
        [Required]
        public string Id { get; set; } = null!;
        public string? ImageUrl { get; set; }

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
