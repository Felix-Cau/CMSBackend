using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class StatusRepository(AlphaDbContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
    {
    }
}
