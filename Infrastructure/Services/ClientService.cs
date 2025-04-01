﻿using Domain.Models;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Services
{
    public class ClientService(IClientRepository clientRepository, IProjectRepository projectRepository) : IClientService
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IProjectRepository _projectRepository = projectRepository;

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
            return createResult
                ? ServiceResult.Created()
                : ServiceResult.Failed();
        }

        public async Task<ServiceResult<IEnumerable<ClientDto>>> GetAllClientsAsync()
        {
            var clientList = await _clientRepository.GetAllAsync(false, null, null, x => x.ClientAddress, x => x.ContactInformation);
            List<ClientDto> tempClientList = [];

            foreach (var client in clientList)
            {
                var clientDto = ClientFactory.ToModel(client);
                if (clientDto is null)
                    return ServiceResult<IEnumerable<ClientDto>>.Failed([], "Internal server error");
                tempClientList.Add(clientDto);
            }

            IEnumerable<ClientDto> returnList = tempClientList.AsEnumerable();
            return (returnList is not null)
                ? ServiceResult<IEnumerable<ClientDto>>.Ok(returnList, "Ok")
                : ServiceResult<IEnumerable<ClientDto>>.Failed([], "Failed");
            
        }

        public async Task<ServiceResult<ClientDto>> GetClientByClientNameAsync(string clientName)
        {
            var clientEntity = await _clientRepository.GetAsync(x => x.ClientName == clientName, x => x.ClientAddress, x => x.ContactInformation);

            var returnClientEntity = ClientFactory.ToModel(clientEntity);
            if (returnClientEntity is null)
                return ServiceResult<ClientDto>.Failed(new ClientDto(), "Internal server error");

            return ServiceResult<ClientDto>.Ok(returnClientEntity, "Ok");
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
            return updatedResult
                ? ServiceResult.Ok("Client updated successfully.")
                : ServiceResult.Failed();
        }

        public async Task<ServiceResult> DeleteClientAsync(string id)
        {
            var deleteClientResult = await _clientRepository.DeleteAsync(x => x.Id == id);

            //Ska jag implementera detta? Eller "silent delete" och sätta isActive till false? Mer logik dock.
            var deleteProjectsResult = await _projectRepository.DeleteManyAsync(x => x.ClientId == id);

            return (deleteClientResult && deleteProjectsResult)
                ? ServiceResult.Ok("Client successfully deleted.")
                : ServiceResult.Failed();
        }
    }
}
