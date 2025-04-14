using Authentication.Entities;
using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using WebApi.Extensions.Attributes;
using WebApi.Extensions.UserEndPoint;

namespace WebApi.Controllers
{
    [Authorize]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, SignInManager<AppUserEntity> signInManager) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly SignInManager<AppUserEntity> _signInManager = signInManager;


        //Fixa authentication/authorization.
        [HttpPost("signup")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Creates a new User upon sign up.")]
        [SwaggerRequestExample(typeof(SignUpForm), typeof(SignUpDataExample))]
        [SwaggerResponseExample(400, typeof(SignUpDataExample))]
        [SwaggerResponseExample(500, typeof(ServiceResultErrorExample))]
        [SwaggerResponse(201, "User created successfully")]
        [SwaggerResponse(400, "Invalid field(s)", typeof(SignUpForm))]
        [SwaggerResponse(409, "User already exists")]
        [SwaggerResponse(500, "Internal server error", typeof(ServiceResult))]
        public async Task<IActionResult> SignUp(SignUpForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _userService.SignUpAsync(form);
            return result.StatusCode switch
            {
                201 => Created(),
                409 => Conflict(),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "User login if credentials are valid.")]
        [SwaggerRequestExample(typeof(SignInForm), typeof(SignInDataExample))]
        [SwaggerResponseExample(400, typeof(SignInDataExample))]
        [SwaggerResponse(200, "User signed in successfully", typeof(ServiceResult))]
        [SwaggerResponse(400, "Invalid field(s)", typeof(SignInForm))]
        [SwaggerResponse(401, "Invalid email or password")]
        public async Task<IActionResult> SignIn(SignInForm form)
        {
            if (!ModelState.IsValid) 
                return BadRequest();

            var signInResult = await _userService.SignInAsync(form);
            return signInResult.StatusCode switch
            {
                200 => Ok(signInResult),
                401 => Unauthorized("Invalid email or password."),
                404 => NotFound("User does not exist."),
                _ => BadRequest(form),
            };
        }

        
        [HttpPost("createuser")]
        [UseAdminApiKey]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Creates a new user when logged in as Admin.", Description = "Requries a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerRequestExample(typeof(NewAppUserForm), typeof(NewAppUserFormDataExample))]
        [SwaggerResponseExample(400, typeof(NewAppUserFormDataExample))]
        [SwaggerResponseExample(500, typeof(ServiceResultErrorExample))]
        [SwaggerResponse(201, "User created successfully")]
        [SwaggerResponse(400, "Invalid field(s)", typeof(NewAppUserForm))]
        [SwaggerResponse(409, "User already exists")]
        [SwaggerResponse(500, "Internal server error", typeof(ServiceResult))]
        public async Task<IActionResult> CreateUser(NewAppUserForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _userService.CreateUserAsAdminAsync(form);
            return result.StatusCode switch
            {
                201 => Created(),
                409 => Conflict(),
                _ => StatusCode(500, result.Message)
            };
        }

        [HttpGet("getusers")]
        [SwaggerOperation(Summary = "Gets all users.")]
        [SwaggerResponseExample(200, typeof(UserLIstDataExample))]
        [SwaggerResponseExample(500, typeof(ServiceResultErrorExample))]
        [SwaggerResponse(200, "Returns all users", typeof(IEnumerable<AppUserDto>))]
        [SwaggerResponse(400, "Something went wrong with the request")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUsersAsync();
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                500 => Problem(),
                _ => BadRequest()
            };
        }

        [HttpGet("getuser/{id}")]
        [SwaggerOperation(Summary = "Gets a user based on the Id.")]
        [SwaggerResponseExample(200, typeof(UserDataExample))]
        [SwaggerResponse(200, "Returns user by Id", typeof(AppUserDto))]
        [SwaggerResponse(400, "Something went wrong with the request")]
        [SwaggerResponse(404, "User not found")]
        [SwaggerResponse(500, "Internal server error", typeof(ServiceResult<AppUserDto>))]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _userService.GetUserByIdAsync(id);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => NotFound(),
                500 => Problem(result.Message),
                _ => BadRequest()
            };
        }

        [HttpPut("updateuser")]
        [UseAdminApiKey]
        [Consumes("multipart/form-data")]
        [SwaggerOperation(Summary = "Updates a user when logged in as Admin", Description = "Requries a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerRequestExample(typeof(EditAppUserForm), typeof(EditAppUserFormDataExample))]
        [SwaggerResponse(200, "User updates successfully")]
        [SwaggerResponse(400, "Something went wrong with the request", typeof(EditAppUserForm))]
        [SwaggerResponse(500, "Internal server error", typeof(ServiceResult<IdentityResult>))]
        public async Task<IActionResult> UpdateUser(EditAppUserForm formData)
        {
            if (!ModelState.IsValid)
                return BadRequest(formData);

            var result = await _userService.UpdateUserAsync(formData);
            return result.StatusCode switch
            {
                200 => Ok(),
                500 => Problem(result.Message),
                _ => BadRequest()
            };
        }

        [HttpDelete("deleteuser/{id}")]
        [UseAdminApiKey]
        [SwaggerOperation(Summary = "Deletes a user when logged in as Admin", Description = "Requries a API-key 'X-ADM-API-KEY' in the header request.")]
        [SwaggerResponse(200, "User deleted successfully")]
        [SwaggerResponse(400, "Something went wrong with the request")]
        [SwaggerResponse(500, "Internal server error", typeof(ServiceResult<IdentityResult>))]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _userService.DeleteUserAsync(id);
            return result.StatusCode switch
            {
                200 => Ok(),
                500 => Problem(result.Message),
                _ => BadRequest()
            };
        }
    }
}
