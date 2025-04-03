using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Infrastructure.Repositories
{
    public class ClientRepository(AlphaDbContext context) : BaseRepository<ClientEntity>(context), IClientRepository
    {
    }
}
