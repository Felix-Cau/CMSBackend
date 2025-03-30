using Infrastructure.Data.Contexts;
using Infrastructure.Data.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Data.Repositories
{
    public class ClientRepository(AlphaDbContext context) : BaseRepository<ClientEntity>(context), IClientRepository
    {
    }
}
