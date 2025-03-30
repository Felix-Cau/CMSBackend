using Domain.Models;

namespace Infrastructure.Models
{
    public class ProjectDto
    {
        public string Id { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string ProjectName { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public AppUserDto ProjectOwner { get; set; } = null!;
        public decimal? Budget { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
    }
}