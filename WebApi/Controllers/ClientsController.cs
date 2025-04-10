using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Extensions.Attributes;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(IClientService clientService) : ControllerBase
    {
        private readonly IClientService _clientService = clientService;

        [HttpPost]
        [UseAdminApiKey]
        public async Task<IActionResult> Create(AddClientForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _clientService.CreateClientAsync(form);
            return result.StatusCode switch
            {
                201 => Created(),
                400 => BadRequest(),
                409 => Conflict(form),
                500 => StatusCode(500, "Internal server error"),
                _ => BadRequest(form)
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _clientService.GetAllClientsAsync();
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                500 => StatusCode(500, "Internal server error"),
                _ => BadRequest()
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _clientService.GetClientByClientIdAsync(id);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => NotFound(),
                500 => StatusCode(500, "Internal server error"),
                _ => BadRequest(id),
            };
        }

        [HttpPut]
        [UseAdminApiKey]
        public async Task<IActionResult> UpdateClient(EditClientForm form)
        {
            if (!ModelState.IsValid) 
                return BadRequest(form);

            var result = await _clientService.UpdateClientAsync(form);
            return result.StatusCode switch
            {
                200 => Ok(),
                400 => BadRequest(),
                404 => NotFound(form),
                500 => StatusCode(500, "Internal server error"),
                _ => BadRequest()
            };
        }

        [HttpDelete("{id}")]
        [UseAdminApiKey]
        public async Task<IActionResult> DeleteClient(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _clientService.DeleteClientAsync(id);
            return result.StatusCode switch
            {
                200 => Ok(),
                500 => StatusCode(500, "Internal server error."),
                _ => BadRequest(id)
            };
        }
    }
}
