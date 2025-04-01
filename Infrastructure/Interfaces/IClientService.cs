using Domain.Models;
using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IClientService
{
    Task<ServiceResult> CreateClientAsync(AddClientForm form);
    Task<ServiceResult<IEnumerable<ClientDto>>> GetAllClientsAsync();
    Task<ServiceResult<ClientDto>> GetClientByClientNameAsync(string clientName);
    Task<ServiceResult> UpdateClientAsync(EditClientForm form);
    Task<ServiceResult> DeleteClientAsync(string id);
}