using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class ClientRepository(DataContext context) : BaseRepository<ClientEntity>(context), IClientRepository
    {
    }
}
