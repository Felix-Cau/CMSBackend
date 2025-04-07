namespace Business.Models
{
    public class ClientDto
    {
        public string Id { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string ClientName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Reference { get; set; }
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool IsActive { get; set; }
    }
}
