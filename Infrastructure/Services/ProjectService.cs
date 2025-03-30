using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services
{
    public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IStatusService _statusService = statusService;
        public Task<bool> CreateProjectAsync(AddProjectForm formData, string defaultStatus = "STARTED")
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProjectAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectDto> GetProjectByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectDto>> GetProjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProjectAsync(EditProjectForm formData)
        {
            throw new NotImplementedException();
        }
    }
}
