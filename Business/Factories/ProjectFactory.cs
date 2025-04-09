using Business.Models;
using Data.Entities;
using Domain.Models;

namespace Business.Factories
{
    public class ProjectFactory
    {
        public static ProjectEntity? ToEntity(AddProjectForm form, string? newImageFileName = null)
        {
            return (form is null)
                ? null
                : new ProjectEntity
                {
                    ImageName = newImageFileName,
                    ProjectName = form.ProjectName,
                    ClientId = form.ClientId,
                    Description = form.Description,
                    StartDate = form.StartDate,
                    EndDate = form.EndDate,
                    ProjectOwnerId = form.ProjectOwnerId,
                    Budget = form.Budget
                };
        }

        public static ProjectEntity? UpdateEntity(EditProjectForm form, string? newImageFileName = null)
        {
            return (form is null)
                ? null
                : new ProjectEntity
                {
                    Id = form.Id,
                    ImageName = newImageFileName ?? form.ImageName,
                    ProjectName = form.ProjectName,
                    ClientId = form.ClientId,
                    Description = form.Description,
                    StartDate = form.StartDate,
                    EndDate = form.EndDate,
                    Budget = form.Budget,
                    ProjectOwnerId = form.ProjectOwnerId,
                    StatusId = form.StatusId
                };
        }

        public static ProjectDto? ToModel(ProjectEntity entity, AppUserDto appUser)
        {
            return (entity is null)
                ? null
                : new ProjectDto
                {
                    Id = entity.Id,
                    ImageName = entity.ImageName,
                    ProjectName = entity.ProjectName,
                    ClientId = entity.ClientId,
                    ClientName = entity.Client.ClientName,
                    Description = entity.Description,
                    StartDate = entity.StartDate,
                    EndDate = entity.EndDate,
                    ProjectOwnerId = appUser.Id,
                    ProjectOwnerName = appUser.Name,
                    Budget = entity.Budget,
                    StatusId = entity.StatusId,
                    StatusName = entity.Status.StatusName
                };
        }
    }
}