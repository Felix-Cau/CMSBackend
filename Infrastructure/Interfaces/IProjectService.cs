namespace Infrastructure.Interfaces
{
    public interface IProjectService
    {
        Task<bool> CreateProjectAsync(AddProjectFormData formData, string defaultStatus = "started");
        Task<bool> DeleteProjectAsync(string id);
        Task<Project> GetProjectByIdAsync(string id);
        Task<IEnumerable<Project>> GetProjectsAsync();
        Task<bool> UpdateProjectAsync(EditProjectFormData formData);
    }
}
