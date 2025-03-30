using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class ProjectRepository(AlphaDbContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
    {
    }
}
