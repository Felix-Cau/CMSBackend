﻿using Authentication.Interfaces;
using Domain.Models;
using Infrastructure.Factories;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, IUsersRepository userRepository) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IStatusService _statusService = statusService;
        private readonly IUsersRepository _userRepository = userRepository;

        public async Task<ServiceResult> CreateProjectAsync(AddProjectForm formData, string defaultStatus = "STARTED")
        {
            if (formData is null)
                return ServiceResult.BadRequest();

            var projectEntity = ProjectFactory.ToEntity(formData);
            if(projectEntity is null)
                return ServiceResult.Failed();

            var status = await _statusService.GetStatusByStatusNameAsync(defaultStatus);
            if (status is not null && status.Id is not 0)
            {
                projectEntity.StatusId = status.Id;
            }
            else
            {
                return ServiceResult.Failed();
            }

            var createResult = await _projectRepository.AddAsync(projectEntity);
            return createResult 
                ? ServiceResult.Created() 
                : ServiceResult.Failed();
        }

        public async Task<ServiceResult<ProjectDto>> GetProjectByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return ServiceResult<ProjectDto>.BadRequest(new ProjectDto(), "Invalid field(s).");

            var projectEntity = await _projectRepository.GetAsync(findByExpression: x => x.Id == id, p => p.Client, p => p.Status);
            var loadUserResult =
                await _userRepository.GetUserAsync(findByExpression: u => u.Id == projectEntity.ProjectOwnerId);
            var projectOwner = loadUserResult.Result;

            if (projectOwner is null)
                return ServiceResult<ProjectDto>.Failed(new ProjectDto(), "An unexpected error occured.");

            var projectDto = ProjectFactory.ToModel(projectEntity, projectOwner);
            if (projectDto is null)
                return ServiceResult<ProjectDto>.Failed(new ProjectDto(), "An unexpected error occured.");

            return ServiceResult<ProjectDto>.Ok(projectDto, "Ok");
        }

        public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetProjectsAsync()
        {
            var projectList = await _projectRepository.GetAllAsync(orderByDescending: true, sortByExpression: p => p.Created, 
                filterByExpression: null, p => p.Client, p => p.Status);
            List<ProjectDto> tempProjectList = [];

            try
            {
                foreach (var project in projectList)
                {
                    var loadUserResult =
                        await _userRepository.GetUserAsync(findByExpression: u => u.Id == project.ProjectOwnerId);
                    var projectOwner = loadUserResult.Result;

                    var projectDto = ProjectFactory.ToModel(project, projectOwner);
                    tempProjectList.Add(projectDto);
                }

                IEnumerable<ProjectDto> returnList = tempProjectList.AsEnumerable();
                return ServiceResult<IEnumerable<ProjectDto>>.Ok(returnList, "Ok");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return ServiceResult<IEnumerable<ProjectDto>>.Failed([], "Failed");
            }
        }

        public async Task<ServiceResult> UpdateProjectAsync(EditProjectForm formData)
        {
            if (formData is null)
                return ServiceResult.BadRequest();

            var projectEntity = ProjectFactory.UpdateEntity(formData);
            if (projectEntity is null)
                return ServiceResult.Failed();

            var updateResult = await _projectRepository.UpdateAsync(projectEntity);
            return updateResult
                ? ServiceResult.Ok()
                : ServiceResult.Failed("Could not update user in database.");
        }

        public async Task<ServiceResult> DeleteProjectAsync(string id)
        {
            var result = await _projectRepository.DeleteAsync(x => x.Id == id);
            return result
                ? ServiceResult.Ok()
                : ServiceResult.Failed();
        }
    }
}
