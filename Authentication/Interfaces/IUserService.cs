using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult> SignUpAsync(SignUpForm form);

        Task<ServiceResult> SignInAsync(SignInForm form);

        Task<ServiceResult> CreateUserAsAdminAsync(NewAppUserForm form);

        Task<ServiceResult<IEnumerable<AppUserDto>>> GetAllUsersAsync();

        Task<ServiceResult<AppUserDto>> GetUserByIdAsync(string id);

        Task<ServiceResult<IdentityResult>> UpdateUserAsync(EditAppUserForm formData);

        Task<ServiceResult<IdentityResult>> DeleteUserAsync(string id);
    }
}
