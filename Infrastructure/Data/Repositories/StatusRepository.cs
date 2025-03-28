using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class StatusRepository(DataContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
    {
    }
}
