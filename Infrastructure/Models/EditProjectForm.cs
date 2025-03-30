using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class EditProjectForm
    {
        [Required]
        public string Id { get; set; } = null!;
        public string? ImageUrl { get; set; }

        [Required]
        public string ProjectName { get; set; } = null!;

        [Required]
        public string ClientId { get; set; } = null!;
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required]
        public string ProjectOwnerId { get; set; } = null!;
        public decimal? Budget { get; set; }

        [Required]
        public int StatusId { get; set; }
    }
}
