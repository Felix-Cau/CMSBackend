using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IClientService
{
    Task<ServiceResult> CreateClientAsync(AddClientForm form);
    Task<ServiceResult<IEnumerable<ClientDto>>> GetAllClientsAsync();
    Task<ServiceResult<ClientDto>> GetClientByClientIdAsync(string id);
    Task<ServiceResult> UpdateClientAsync(EditClientForm form);
    Task<ServiceResult> DeleteClientAsync(string id);
}