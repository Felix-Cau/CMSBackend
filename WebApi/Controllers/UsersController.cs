using Authentication.Interfaces;
using Authentication.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;


        //Fixa authentication/authorization.
        [HttpPost]
        [Route("signup")]
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
        public async Task<IActionResult> SignIn(SignInForm form)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _userService.SignInAsync(form);

                switch (signInResult.StatusCode)
                {
                    case 200:
                        return Ok(signInResult.Token);
                    case 401:
                        return Unauthorized("Invalid email or password.");
                    default:
                        return BadRequest(form);
                }
            }
            return BadRequest();
        }

        [HttpPost("createuser")]
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

        [HttpPut("updateuser/{id}")]
        public async Task<IActionResult> UpdateUser(EditAppUserForm formData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.UpdateUserAsync(formData);
            return result.StatusCode switch
            {
                200 => Ok(),
                500 => Problem(result.Message),
                _ => BadRequest()
            };
        }

        [HttpDelete("deleteuser/{id}")]
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
