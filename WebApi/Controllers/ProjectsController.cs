﻿using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [HttpPost]
        public async Task<IActionResult> Create(AddProjectForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _projectService.CreateProjectAsync(form);
            return result.StatusCode switch
            {
                201 => Created(),
                400 => BadRequest(form),
                500 => StatusCode(500, "Internal server error."),
                _ => BadRequest(form)
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projectService.GetProjectsAsync();
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => Ok(result.Result),
                _ => BadRequest()
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _projectService.GetProjectByIdAsync(id);
            return result.StatusCode switch
            {
                200 => Ok(result.Result),
                404 => NotFound(id),
                500 => StatusCode(500, "Internal server error."),
                _ => BadRequest(id),
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(EditProjectForm form)
        {
            if (!ModelState.IsValid)
                return BadRequest(form);

            var result = await _projectService.UpdateProjectAsync(form);
            return result.StatusCode switch
            {
                200 => Ok(),
                400 => BadRequest(),
                500 => StatusCode(500, "Internal server error."),
                _ => BadRequest(form)
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var result = await _projectService.DeleteProjectAsync(id);
            return result.StatusCode switch
            {
                200 => Ok(),
                400 => BadRequest(),
                500 => StatusCode(500, "Internal server error."),
                _ => BadRequest(id)
            };
        }
    }
}
