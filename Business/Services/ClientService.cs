using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services
{
    public class ClientService(IClientRepository clientRepository, IMemoryCache cache) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "Client_All";

        public async Task<ServiceResult> CreateClientAsync(AddClientForm form)
        {
            if(form is null)
                return ServiceResult.BadRequest();

            var doesClientExist = await _clientRepository.ExistsAsync(x => x.ClientName == form.ClientName);
            if (doesClientExist)
                return ServiceResult.AlreadyExists();

            var clientEntity = ClientFactory.ToEntity(form);
            if (clientEntity is null)
                return ServiceResult.Failed();

            var createResult = await _clientRepository.AddAsync(clientEntity);
            if (!createResult)
                return ServiceResult.Failed();

            _cache.Remove(_cacheKey_All);
            return ServiceResult.Created();
        }

        public async Task<ServiceResult<IEnumerable<ClientDto>>> GetAllClientsAsync()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<ClientDto>? cachedItems))
                return ServiceResult<IEnumerable<ClientDto>>.Ok(cachedItems!, "Ok");

            var clientDtoList = await SetCache();
            return clientDtoList is not null && clientDtoList.Any() 
                ? ServiceResult<IEnumerable<ClientDto>>.Ok(clientDtoList, "ok")
                : ServiceResult<IEnumerable<ClientDto>>.Failed([], "An unexpected error occured");
        }

        public async Task<ServiceResult<ClientDto>> GetClientByClientIdAsync(string clientId)
        {
            ClientDto clientDto = new();

            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<ClientDto>? cachedItems))
            {
                clientDto = cachedItems.FirstOrDefault(c => c.Id == clientId);
                if (clientDto is not null)
                    return ServiceResult<ClientDto>.Ok(clientDto, "Ok");
            }

            var result = await _clientRepository.GetAsync(x => x.Id == clientId, x => x.ClientAddress, x => x.ContactInformation);
            if (result is null)
                return ServiceResult<ClientDto>.Failed(new ClientDto(), "An unexpected error occured");
            var clientEntity = ClientFactory.ToModel(result);
            if (clientEntity is null)
                return ServiceResult<ClientDto>.NotFound(new ClientDto(), "Not found");

            await SetCache();
            return ServiceResult<ClientDto>.Ok(clientEntity, "Ok");
        }

        public async Task<ServiceResult> UpdateClientAsync(EditClientForm form)
        {
            if (form is null)
                return ServiceResult.BadRequest();

            var oldClient = await _clientRepository.GetAsync(x => x.Id == form.Id, x => x.ClientAddress!, x => x.ContactInformation!);
            if (oldClient is null)
                return ServiceResult.NotFound();

            var updatedClient = ClientFactory.UpdateEntity(form, oldClient);
            if (updatedClient is null)
                return ServiceResult.Failed();

            var updatedResult = await _clientRepository.UpdateAsync(updatedClient);
            if (!updatedResult)
                return ServiceResult.Failed();

            _cache.Remove(_cacheKey_All);
            return ServiceResult.Ok("Client updated successfully.");
        }

        public async Task<ServiceResult> DeleteClientAsync(string id)
        {
            var deleteClientResult = await _clientRepository.DeleteAsync(x => x.Id == id);
            if (!deleteClientResult)
                return ServiceResult.Failed();

            _cache.Remove(_cacheKey_All);
            return ServiceResult.Ok("Client successfully deleted.");
        }

        public async Task<IEnumerable<ClientDto>> SetCache()
        {
            _cache.Remove(_cacheKey_All);
            var clientEntityList = await _clientRepository.GetAllAsync(false, c => c.ClientName, null, c => c.ClientAddress!, c => c.ContactInformation!);

            List<ClientDto> returnList = [];
            foreach (var entity in clientEntityList)
            {
                var returnDto = ClientFactory.ToModel(entity);
                if (returnDto is null)
                {
                    returnList = [];
                    return returnList.AsEnumerable();
                }
                returnList.Add(returnDto);
            }

            _cache.Set(_cacheKey_All, returnList.AsEnumerable(), TimeSpan.FromHours(1));
            return returnList.AsEnumerable();
        }
    }
}
