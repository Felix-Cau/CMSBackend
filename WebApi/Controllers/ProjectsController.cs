//using Infrastructure.Interfaces;
//using Infrastructure.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace WebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProjectsController(IProjectService projectService) : ControllerBase
//    {
//        private readonly IProjectService _projectService = projectService;
//        [HttpPost]
//        public async Task<IActionResult> Create(AddProjectForm form)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(form);

//            var result = await
//        }
//    }
//}
