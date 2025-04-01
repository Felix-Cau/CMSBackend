using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController(IClientService clientService) : ControllerBase
    {
        private readonly IClientService _clientService = clientService;

        [HttpPost]
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

        [HttpGet("{clientName}")]
        public async Task<IActionResult> GetProjectById(string clientName)
        {
            var result = await _clientService.GetClientByClientNameAsync(clientName);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                500 => StatusCode(500, "Internal server error."),
                _ => BadRequest(clientName),
            };
        }

        [HttpPut]
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

        [HttpDelete]
        public async Task<IActionResult> DeleteClient(string id)
        {
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
