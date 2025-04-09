using Authentication.Entities;
using Authentication.Factories;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Authentication.Services
{
    public class UserService(UserManager<AppUserEntity> userManager, SignInManager<AppUserEntity> signInManager, RoleHandler roleHander, IJwtTokenHandler jwtTokenHandler, 
        IConfiguration configuration, IUsersRepository userRepository, IMemoryCache cache) : IUserService
    {
        private readonly UserManager<AppUserEntity> _userManager = userManager;
        private readonly SignInManager<AppUserEntity> _signInManager = signInManager;
        private readonly RoleHandler _roleHandler = roleHander;
        private readonly IJwtTokenHandler _tokenHandler = jwtTokenHandler;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUsersRepository _userRepository = userRepository;
        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "User_All";



        public async Task<ServiceResult> SignUpAsync(SignUpForm form)
        {
            var appUser = UserFactory.ToEntity(form);

            if (appUser is not null)
            {
                var exists = await _userRepository.ExistsAsync(form);
                if (exists)
                    return ServiceResult.AlreadyExists();

                form.Role = "Admin";

                var result = await _userManager.CreateAsync(appUser, form.Password);
                if (!result.Succeeded)
                    return ServiceResult.Failed();

                var roleResult = await _roleHandler.AddToRoleAsync(appUser, form.Role);
                if (!roleResult.Succeeded)
                    return ServiceResult.Failed();

                _cache.Remove(_cacheKey_All);
                return ServiceResult.Created() ;
            }
            return ServiceResult.Failed();
        }

        public async Task<ServiceResult> SignInAsync(SignInForm form)
        {
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
            if (!result.Succeeded)
                return ServiceResult.UnAuthorized();

            var user = await _userManager.FindByEmailAsync(form.Email);
            if (user is null)
                return ServiceResult.NotFound();

            var userRole = await _roleHandler.GetRoleAsync(user!);
            if (userRole is null)
                return ServiceResult.Failed();

            string adminApiKey = string.Empty;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                adminApiKey = _configuration["SecretKeys:Admin"]!;
            }

            var token = _tokenHandler.GenerateToken(user!, userRole);

            return ServiceResult.AuthOk(token, isAdmin, adminApiKey);
        }

        public async Task<ServiceResult> CreateUserAsAdminAsync(NewAppUserForm form)
        {
            var exists = await _userRepository.ExistsAsync(form);
            if (exists)
                return ServiceResult.AlreadyExists();

            var appUser = UserFactory.ToEntity(form);

            if (appUser is not null)
            {
                form.Role = "User";
                var standardPassword = _configuration["PW:Standard"]!;

                var result = await _userManager.CreateAsync(appUser, standardPassword);
                if(!result.Succeeded)
                    return ServiceResult.Failed();

                var roleResult = await _roleHandler.AddToRoleAsync(appUser, form.Role);
                if (!roleResult.Succeeded)
                    return ServiceResult.Failed();

                _cache.Remove(_cacheKey_All);
                return ServiceResult.Created();
            }
            return ServiceResult.Failed();
        }

        public async Task<ServiceResult<IEnumerable<AppUserDto>>> GetAllUsersAsync()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<AppUserDto>? cachedItems))
                return ServiceResult<IEnumerable<AppUserDto>>.Ok(cachedItems!, "Ok");

            var appUserDtoList = await UpdateCacheAsync();
            return appUserDtoList is not null && appUserDtoList.Any() 
                ? ServiceResult<IEnumerable<AppUserDto>>.Ok(appUserDtoList!, "Ok")
                : ServiceResult<IEnumerable<AppUserDto>>.Failed([], "An unexpected error occured");
        }
        
        public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(string id)
        {
            AppUserDto appUserDto = new();

            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<AppUserDto>? cachedItems))
            {
                appUserDto = cachedItems.FirstOrDefault(x => x.Id == id);
                if (appUserDto is not null)
                    return ServiceResult<AppUserDto>.Ok(appUserDto, "Ok");
            }

            var result = await _userRepository.GetUserAsync(findByExpression: x => x.Id == id, includes: user => user.Address);
            if (result is null)
                return ServiceResult<AppUserDto>.NotFound(new AppUserDto(), "Not found");

            await UpdateCacheAsync();
            return result;
        }

        public async Task<ServiceResult<IdentityResult>> UpdateUserAsync(EditAppUserForm formData)
        {
            var appUser = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == formData.Id);
            if (appUser is null)
                return ServiceResult<IdentityResult>.NotFound(new IdentityResult(),"User not found.");

            var updatedUser = UserFactory.UpdateEntity(formData, appUser);
            if (updatedUser is null)
                return ServiceResult<IdentityResult>.Failed(new IdentityResult(), "Internal Server Error");

            var roleResult = await _roleHandler.UpdateRoleAsync(appUser, formData.Role);
            if (roleResult is null)
                return ServiceResult<IdentityResult>.Failed(new IdentityResult(), "Failed to remove old Role from user and aborted.");
            if (!roleResult.Succeeded)
                return ServiceResult<IdentityResult>.Failed(roleResult, "Could not add user to new Role and aborted.");

            var updateUserInDbResult = await _userManager.UpdateAsync(updatedUser);
            if (!updateUserInDbResult.Succeeded)
                return ServiceResult<IdentityResult>.Failed(updateUserInDbResult, "Could not update user in database.");

            _cache.Remove(_cacheKey_All);
            return ServiceResult<IdentityResult>.Ok(roleResult, "Updated user Role successfully.");
        }

        public async Task<ServiceResult<IdentityResult>> DeleteUserAsync(string id)
        {
            var appUser = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);
            if (appUser is null)
                return ServiceResult<IdentityResult>.Failed(new IdentityResult(), "Could not get user by Id");

            var removeFromRoleResult = await _roleHandler.RemoveFromRoleAsync(appUser);
            if (!removeFromRoleResult!.Succeeded)
                return ServiceResult<IdentityResult>.Failed(removeFromRoleResult ?? new IdentityResult(), "Could not remove user role and aborted");

            var deleteUserResult = await _userManager.DeleteAsync(appUser);
            if (!deleteUserResult.Succeeded)
                return ServiceResult<IdentityResult>.Failed(deleteUserResult, "Could not delete user after removing it from the role list.");

            _cache.Remove(_cacheKey_All);
            return ServiceResult<IdentityResult>.Ok(deleteUserResult, "User removed successfully.");
        }

        public async Task<IEnumerable<AppUserDto>?> UpdateCacheAsync()
        {
            _cache.Remove(_cacheKey_All);
            var result = await _userRepository.GetAllAsync(false, sortByExpression: u => u.LastName, null, user => user.Address!);
            if (!result.Succeeded)
                return [];

            _cache.Set(_cacheKey_All, result.Result, TimeSpan.FromHours(1));
            return result.Result;
        }
    }
}


