﻿using System.Security.Cryptography.X509Certificates;
using Authentication.Factories;
using Authentication.Handlers;
using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Authentication.Services
{
    public class UserService(UserManager<AppUserEntity> userManager, SignInManager<AppUserEntity> signInManager, RoleHandler roleHander, IJwtTokenHandler jwtTokenHandler, IConfiguration configuration, IUsersRepository userRepository) : IUserService
    {
        private readonly UserManager<AppUserEntity> _userManager = userManager;
        private readonly SignInManager<AppUserEntity> _signInManager = signInManager;
        private readonly RoleHandler _roleHandler = roleHander;
        private readonly IJwtTokenHandler _tokenHandler = jwtTokenHandler;
        private readonly IConfiguration _configuration = configuration;
        private readonly IUsersRepository _userRepository = userRepository;


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
            var userRole = await _roleHandler.GetRoleAsync(user!);
            if (userRole is null)
                return ServiceResult.Failed();

            var token = _tokenHandler.GenerateToken(user!, userRole);

            return ServiceResult.TokenOk(token);
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

                return ServiceResult.Created();
            }
            return ServiceResult.Failed();
        }

        public async Task<ServiceResult<IEnumerable<AppUserDto>>> GetAllUsersAsync()
        {
            var result = await _userRepository.GetAllAsync(includes: user => user.Address);
            return !result.Succeeded 
                ? ServiceResult<IEnumerable<AppUserDto>>.Failed([], "Could not fetch users.") 
                : result;
        }
        
        public async Task<ServiceResult<AppUserDto>> GetUserByIdAsync(string id)
        {
            var result = await _userRepository.GetUserAsync(findByExpression: x => x.Id == id, includes: user => user.Address);
            return !result.Succeeded
                ? ServiceResult<AppUserDto>.NotFound(result.Result, "Could not fetch user.")
                : result;
        }

        public async Task<ServiceResult<IdentityResult>> UpdateUserAsync(EditAppUserForm formData)
        {
            var appUser = await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == formData.Id);
            if (appUser == null)
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

            return ServiceResult<IdentityResult>.Ok(deleteUserResult, "User removed successfully.");
        }
    }
}

