﻿using Domain.Models;
using Infrastructure.Data.Entities;
using Infrastructure.Models;

namespace Infrastructure.Factories
{
    public class ProjectFactory
    {
        public static ProjectEntity? ToEntity(AddProjectForm form)
        {
            return (form is null)
                ? null
                : new ProjectEntity
                {
                    ImageUrl = form.Image,
                    ProjectName = form.ProjectName,
                    ClientId = form.ClientId,
                    Description = form.Description,
                    StartDate = form.StartDate,
                    EndDate = form.EndDate,
                    ProjectOwnerId = form.ProjectOwnerId,
                    Budget = form.Budget
                };
        }

        public static ProjectEntity? UpdateEntity(EditProjectForm form)
        {
            return (form is null)
                ? null
                : new ProjectEntity
                {
                    Id = form.Id,
                    ImageUrl = form.ImageUrl,
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
                    ImageUrl = entity.ImageUrl,
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