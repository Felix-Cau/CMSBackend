using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities
{
    public class ClientEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ClientName { get; set; } = null!;

        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool IsActive { get; set; }
        public virtual ClientContactInformationEntity ContactInformation { get; set; } = null!;
        public virtual ClientAddressEntity ClientAddress { get; set; } = null!;

        public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
    }
}
