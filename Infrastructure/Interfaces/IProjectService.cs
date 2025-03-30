using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(AddProjectForm formData, string defaultStatus = "started");
        Task<bool> DeleteProjectAsync(string id);
        Task<ProjectDto> GetProjectByIdAsync(string id);
        Task<IEnumerable<ProjectDto>> GetProjectsAsync();
        Task<bool> UpdateProjectAsync(EditProjectForm formData);
    }
}
