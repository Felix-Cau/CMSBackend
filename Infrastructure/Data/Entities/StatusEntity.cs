using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Data.Entities
{
    public class StatusEntity
    {
        [Key]
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
    }
}
