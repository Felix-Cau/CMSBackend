using Authentication.Models;
using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Factories
{
    public class UserFactory
    {
        public static AppUserEntity? ToEntity(SignUpForm formData)
        {
            if (formData is null)
                return null;

            AppUserEntity appUser = new()
            {
                UserName = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email,
            };

            appUser.Address = new AppUserAddressEntity()
            {
                UserId = appUser.Id
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
                JobTitle = appUser.JobTitle,
                Role = role,
                Address = appUser.Address!.Address,
                PostalCode = appUser.Address.PostalCode,
                City = appUser.Address.City
            };
            return appUserDto;
        }

        public static AppUserEntity? ToEntity(NewAppUserForm formData)
        {
            if (formData is null)
                return null;

            var appUser = new AppUserEntity
            {
                UserName = formData.Email,
                Email = formData.Email,
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

        public static AppUserEntity? UpdateEntity(EditAppUserForm formData, AppUserEntity oldUser)
        {
            if (formData is null)
                return null;

            oldUser.FirstName = formData.FirstName;
            oldUser.LastName = formData.LastName;
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
