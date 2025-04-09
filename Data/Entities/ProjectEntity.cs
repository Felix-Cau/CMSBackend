using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ProjectEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? ImageName { get; set; }

        public string ProjectName { get; set; } = null!;

        [ForeignKey(nameof(Client))]
        public string ClientId { get; set; } = null!;
        public virtual ClientEntity Client { get; set; } = null!;
        public string? Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public string ProjectOwnerId { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal? Budget { get; set; }

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }
        public virtual StatusEntity Status { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
