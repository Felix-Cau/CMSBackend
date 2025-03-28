namespace Domain.Models
{
    public class AppUserDTO
    {
        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? JobTitle { get; set; }
        public string Role { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
    }
}
