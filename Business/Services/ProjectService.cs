using Authentication.Interfaces;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services
{
    public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, IUsersRepository userRepository, IMemoryCache cache, IFileHandler fileHandler) : IProjectService
    {
        private readonly IProjectRepository _projectRepository = projectRepository;
        private readonly IStatusService _statusService = statusService;
        private readonly IUsersRepository _userRepository = userRepository;
        private readonly IMemoryCache _cache = cache;
        private const string _cacheKey_All = "Project_All";
        private readonly IFileHandler _fileHandler = fileHandler;


        public async Task<ServiceResult> CreateProjectAsync(AddProjectForm formData, string defaultStatus = "STARTED")
        {
            if (formData is null)
                return ServiceResult.BadRequest();

            var imageFileUri = await _fileHandler.UploadFileAsync(formData.ImageFile!);

            var projectEntity = imageFileUri is null
                ? ProjectFactory.ToEntity(formData)
                : ProjectFactory.ToEntity(formData, imageFileUri);
                
            if(projectEntity is null)
                return ServiceResult.Failed();

            var status = await _statusService.GetStatusByStatusNameAsync(defaultStatus);
            if (status is not null && status.Result.Id is not 0)
            {
                projectEntity.StatusId = status.Result.Id;
            }
            else
            {
                return ServiceResult.Failed();
            }

            var createResult = await _projectRepository.AddAsync(projectEntity);
            if (!createResult)
                return ServiceResult.Failed();

            _cache.Remove(_cacheKey_All);
            return ServiceResult.Created();
        }

        public async Task<ServiceResult<ProjectDto>> GetProjectByIdAsync(string id)
        {
            ProjectDto projectDto = new();

            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<ProjectDto>? cachedItems))
            {
                projectDto = cachedItems.FirstOrDefault(p => p.Id == id);
                if (projectDto is not null)
                    return ServiceResult<ProjectDto>.Ok(projectDto, "Ok");
            }

            var tempProjectEntity = await _projectRepository.GetAsync(p => p.Id == id, p => p.Client, p => p.Status);
            if (tempProjectEntity is null)
                return ServiceResult<ProjectDto>.NotFound(new ProjectDto(), "Unexpected error occured");

            var tempUserEntity =
                await _userRepository.GetUserAsync(u => u.Id == tempProjectEntity.ProjectOwnerId, u => u.Address!);
            var projectOwner = tempUserEntity.Result;
            if (projectOwner is null)
                return ServiceResult<ProjectDto>.Failed(new ProjectDto(), "An unexpected error occured.");

            projectDto = ProjectFactory.ToModel(tempProjectEntity, projectOwner);
            if (projectDto is null)
                return ServiceResult<ProjectDto>.Failed(new ProjectDto(), "An unexpected error occured.");

            await UpdateCacheAsync();
            return ServiceResult<ProjectDto>.Ok(projectDto, "Ok");
        }

        public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetProjectsAsync()
        {
            if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<ProjectDto>? cachedItems))
                return ServiceResult<IEnumerable<ProjectDto>>.Ok(cachedItems, "Ok");

            var projectDtoList = await UpdateCacheAsync();
            return projectDtoList is not null && projectDtoList.Any()
                ? ServiceResult<IEnumerable<ProjectDto>>.Ok(projectDtoList, "Ok")
                : ServiceResult<IEnumerable<ProjectDto>>.NotFound([], "Not found");
        }

        public async Task<ServiceResult> UpdateProjectAsync(EditProjectForm formData)
        {
            if (formData is null)
                return ServiceResult.BadRequest();

            var imageFileUri = await _fileHandler.UploadFileAsync(formData.NewImageFile!);

            var projectEntity = imageFileUri is null
                ? ProjectFactory.UpdateEntity(formData)
                : ProjectFactory.UpdateEntity(formData, imageFileUri);

            if (projectEntity is null)
                return ServiceResult.Failed();

            var updateResult = await _projectRepository.UpdateAsync(projectEntity);
            if (!updateResult)
                return ServiceResult.Failed("Could not update user in database.");

            _cache.Remove(_cacheKey_All);
            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> DeleteProjectAsync(string id)
        {
            var result = await _projectRepository.DeleteAsync(x => x.Id == id);
            if (!result)
                return ServiceResult.Failed();
           
            _cache.Remove(_cacheKey_All);
            return ServiceResult.Ok();
        }

        public async Task<IEnumerable<ProjectDto>> UpdateCacheAsync()
        {
            _cache.Remove(_cacheKey_All);
            var projectEntityList = await _projectRepository.GetAllAsync(true, p => p.Created, null, c => c.Client, c => c.Status!);

            List<ProjectDto> returnList = [];
            foreach (var entity in projectEntityList)
            {
                var loadUserResult =
                    await _userRepository.GetUserAsync(findByExpression: u => u.Id == entity.ProjectOwnerId, u => u.Address);
                var projectOwner = loadUserResult.Result;
                if (projectOwner is null)
                {
                    returnList = [];
                    return returnList.AsEnumerable();
                }

                var projectDto = ProjectFactory.ToModel(entity, projectOwner);
                returnList.Add(projectDto);
            }

            _cache.Set(_cacheKey_All, returnList.AsEnumerable(), TimeSpan.FromHours(1));
            return returnList.AsEnumerable();
        }
    }
}
