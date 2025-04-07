using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class AddProjectForm
    {
        public IFormFile? ImageFile { get; set; }

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
    }
}
