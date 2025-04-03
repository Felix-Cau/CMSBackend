using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class AlphaDbContext(DbContextOptions<AlphaDbContext> context) : DbContext(context)
    {
        public virtual DbSet<ClientEntity> Clients { get; set; }
        public virtual DbSet<ClientContactInformationEntity> ClientContacts { get; set; }
        public virtual DbSet<ClientAddressEntity> ClientAddresses { get; set; }
        public virtual DbSet<ProjectEntity> Projects { get; set; }
        public virtual DbSet<StatusEntity> Statuses { get; set; }
    }
}
