using Business.Models;
using Data.Entities;

namespace Business.Factories
{
    public class ClientFactory
    {
        public static ClientEntity? ToEntity(AddClientForm form, string? newImageFileUri = null)
        {
            if (form is null)
                return null;

            ClientEntity client = new()
            {
                ImageName = newImageFileUri,
                ClientName = form.ClientName,
                Created = DateTime.Now,
                Modified = DateTime.Now,
                IsActive = true,
            };

            client.ContactInformation = new ClientContactInformationEntity()
            {
                ClientId = client.Id,
                Email = form.ClientEmail,
                Phone = form.PhoneNumber,
                Reference = form.Reference
            };

            client.ClientAddress = new ClientAddressEntity()
            {
                ClientId = client.Id,
                Address = form.Address,
                PostalCode = form.PostalCode,
                City = form.City
            };
            return client;
        }

        public static ClientDto? ToModel(ClientEntity entity)
        {
            if (entity is null)
                return null;

            ClientDto clientDto = new()
            {
                Id = entity.Id,
                ImageName = entity.ImageName,
                ClientName = entity.ClientName,
                Email = entity.ContactInformation.Email,
                Phone = entity.ContactInformation.Phone,
                Reference = entity.ContactInformation.Reference,
                Address = entity.ClientAddress.Address,
                PostalCode = entity.ClientAddress.PostalCode,
                City = entity.ClientAddress.City,
                Created = entity.Created,
                Modified = entity.Modified,
                IsActive = entity.IsActive,
            };
            return clientDto;
        }

        public static ClientEntity? UpdateEntity(EditClientForm form, ClientEntity oldEntity, string? newImageFileUri = null)
        {
            if (form is null || oldEntity is null)
                return null;

            oldEntity.ClientName = form.ClientName;
            oldEntity.ImageName = newImageFileUri ?? form.ImageName;
            oldEntity.ClientName = form.ClientName;
            oldEntity.Modified = DateTime.Now;
            oldEntity.ContactInformation.Email = form.ClientEmail;
            oldEntity.ContactInformation.Phone = form.PhoneNumber;
            oldEntity.ContactInformation.Reference = form.Reference;
            oldEntity.ClientAddress.Address = form.Address;
            oldEntity.ClientAddress.PostalCode = form.PostalCode;
            oldEntity.ClientAddress.City = form.City;

            return oldEntity;
        }
    }
}