using Authentication.Models;
using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Factories
{
    public class UserFactory
    {
        public static AppUser? ToEntity(SignUpForm formData)
        {
            if (formData is null)
                return null;

            AppUser appUser = new()
            {
                UserName = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email,
            };

            appUser.Address = new AppUserAddress()
            {
                UserId = appUser.Id
            };
            return appUser;
        }

        public static AppUserDTO? ToModel(AppUser appUser, string role)
        {
            if (appUser is null)
                return null;

            AppUserDTO appUserDto = new()
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

        public static AppUser? ToEntity(NewAppUserForm formData)
        {
            if (formData is null)
                return null;

            var appUser = new AppUser
            {
                UserName = formData.Email,
                Email = formData.Email,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                JobTitle = formData.JobTitle,
                PhoneNumber = formData.PhoneNumber
            };

            appUser.Address = new AppUserAddress
            {
                UserId = appUser.Id,
                Address = formData.Address,
                PostalCode = formData.PostalCode,
                City = formData.City
            };
            return appUser;
        }

        public static AppUser? UpdateEntity(EditAppUserForm formData, AppUser oldUser)
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

//public class EditAppUserForm
//{
//    [Required]
//    public string Id { get; set; } = null!;
//    public string? FirstName { get; set; }
//    public string? LastName { get; set; }
//    public string? JobTitle { get; set; }
//    public string? Email { get; set; }
//    public string Role { get; set; } = null!;
//    public string? Address { get; set; }
//    public string? PostalCode { get; set; }
//    public string? City { get; set; }
//}
