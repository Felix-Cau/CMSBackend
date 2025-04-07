using Authentication.Entities;
using Authentication.Models;
using Domain.Models;

namespace Authentication.Factories
{
    public class UserFactory
    {
        public static AppUserEntity? ToEntity(SignUpForm formData, string? newImageFileName = null)
        {
            if (formData is null)
                return null;

            AppUserEntity appUser = new()
            {
                UserName = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email,
                ImageUrl = newImageFileName
            };

            appUser.Address = new AppUserAddressEntity()
            {
                UserId = appUser.Id
            };
            return appUser;
        }

        public static AppUserEntity? ToEntity(NewAppUserForm formData, string? newImageFileName = null)
        {
            if (formData is null)
                return null;

            var appUser = new AppUserEntity
            {
                UserName = formData.Email,
                Email = formData.Email,
                ImageUrl = newImageFileName, 
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
                ImageUrl = appUser.ImageUrl,
                JobTitle = appUser.JobTitle,
                Role = role,
                Address = appUser.Address.Address,
                PostalCode = appUser.Address.PostalCode,
                City = appUser.Address.City
            };
            return appUserDto;
        }

        public static AppUserEntity? UpdateEntity(EditAppUserForm formData, AppUserEntity oldUser, string? newImageFileName = null)
        {
            if (formData is null)
                return null;

            oldUser.FirstName = formData.FirstName;
            oldUser.LastName = formData.LastName;
            oldUser.ImageUrl = newImageFileName ?? formData.ImageUrl;
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
