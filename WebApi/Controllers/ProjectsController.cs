using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(I) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(AddProjectForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await
        }
    }
}
