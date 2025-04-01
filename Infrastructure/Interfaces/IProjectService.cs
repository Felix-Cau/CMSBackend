using Domain.Models;
using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
    public interface IProjectService
    {
        Task<ServiceResult> CreateProjectAsync(AddProjectForm formData, string defaultStatus = "STARTED");
        Task<ServiceResult> DeleteProjectAsync(string id);
        Task<ServiceResult<ProjectDto>> GetProjectByIdAsync(string id);
        Task<ServiceResult<IEnumerable<ProjectDto>>> GetProjectsAsync();
        Task<ServiceResult> UpdateProjectAsync(EditProjectForm formData);
    }
}
