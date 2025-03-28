using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities
{
    public class ClientAddressEntity
    {
        [Key]
        public string ClientId { get; set; }
        public virtual ClientEntity Client { get; set; }
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}
