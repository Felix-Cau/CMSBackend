using Authentication.Entities;
using Authentication.Models;
using Domain.Interfaces;
using Domain.Models;

namespace Authentication.Factories
{
    public class UserFactory
    {
        public static AppUserEntity? ToEntity(SignUpForm formData, string? newImageFileUri = null)
        {
            if (formData is null)
                return null;

            AppUserEntity appUser = new()
            {
                UserName = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email,
                ImageName = newImageFileUri
            };

            appUser.Address = new AppUserAddressEntity()
            {
                UserId = appUser.Id
            };
            return appUser;
        }

        public static AppUserEntity? ToEntity(NewAppUserForm formData, string? newImageFileUri = null)
        {
            if (formData is null)
                return null;

            var appUser = new AppUserEntity
            {
                UserName = formData.Email,
                Email = formData.Email,
                ImageName = newImageFileUri, 
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                JobTitle = formData.JobTitle,
                PhoneNumber = formData.PhoneNumber
            };

            appUser.Address = new AppUserAddressEntity
            {
                UserId = appUser.Id,
                Address = formData.Address,
                PostalCode = formData.PostalCode,
                City = formData.City
            };
            return appUser;
        }

        public static AppUserDto? ToModel(AppUserEntity appUser, string role)
        {
            if (appUser is null)
                return null;

            AppUserDto appUserDto = new()
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                ImageName = appUser.ImageName,
                JobTitle = appUser.JobTitle,
                PhoneNumber = appUser.PhoneNumber,
                Role = role,
                Address = appUser.Address.Address,
                PostalCode = appUser.Address.PostalCode,
                City = appUser.Address.City
            };
            return appUserDto;
        }

        public static AppUserEntity? UpdateEntity(EditAppUserForm formData, AppUserEntity oldUser, string? newImageFileUri = null)
        {
            if (formData is null)
                return null;

            oldUser.FirstName = formData.FirstName;
            oldUser.LastName = formData.LastName;
            oldUser.ImageName = newImageFileUri ?? formData.ImageName;
            oldUser.JobTitle = formData.JobTitle;
            oldUser.PhoneNumber = formData.PhoneNumber;

            if (oldUser.Address is null)
                return null;

            oldUser.Address.Address = formData.Address;
            oldUser.Address.PostalCode = formData.PostalCode;
            oldUser.Address.City = formData.City;

            return oldUser;
        }
    }
}
