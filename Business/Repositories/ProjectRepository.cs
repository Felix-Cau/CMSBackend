using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Infrastructure.Repositories
{
    public class ProjectRepository(AlphaDbContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
    {
    }
}
