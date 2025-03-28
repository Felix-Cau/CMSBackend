using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
    {
    }
}
