using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Infrastructure.Repositories
{
    public class StatusRepository(AlphaDbContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
    {
    }
}
