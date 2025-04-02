using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities
{
    public class ClientEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ImageUrl { get; set; }
        public string ClientName { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool IsActive { get; set; }
        public virtual ClientContactInformationEntity? ContactInformation { get; set; }
        public virtual ClientAddressEntity? ClientAddress { get; set; }

        public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
    }
}
