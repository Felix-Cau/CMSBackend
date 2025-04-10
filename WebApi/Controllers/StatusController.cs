using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(IStatusService statusService) : ControllerBase
    {
        private readonly IStatusService _statusService = statusService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _statusService.GetStatusesAsync();
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                500 => StatusCode(500, result.Message),
                _ => BadRequest()
            };
        }

        [HttpGet("{statusName}")]
        public async Task<IActionResult> GetStatusByName(string statusName)
        {
            if (string.IsNullOrEmpty(statusName))
                return BadRequest();

            var result = await _statusService.GetStatusByStatusNameAsync(statusName);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => NotFound(),
                500 => StatusCode(500, result.Message),
                _ => BadRequest()
            };
        }
    }
}
