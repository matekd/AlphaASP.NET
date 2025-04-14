using Data.Entities;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectModel? Create(Project project)
    {
        if (project == null) return null;
        var projectModel = new ProjectModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            ClientId = project.Client.Id,
            StatusId = project.Status.Id
        };
        if (!project.Members.IsNullOrEmpty()) projectModel.Members = [.. project.Members!];
        if (project.Description != null) projectModel.Description = project.Description;
        if (project.ImageUrl != null) projectModel.ImageUrl = project.ImageUrl;
        return projectModel;
    }

    public static Project? Create(ProjectEntity entity)
    {
        if (entity == null) return null!;

        var project = new Project
        {
            Id = entity.Id,
            ProjectName = entity.ProjectName,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            Status = StatusFactory.Create(entity.Status),
            Client = ClientFactory.Create(entity.Client),
        };
        if (entity.Description != null) project.Description = entity.Description;
        if (entity.ImageUrl != null) project.ImageUrl = entity.ImageUrl;
        if (entity.Members != null && entity.Members.Any()) project.Members = [.. entity.Members!.Select(MemberFactory.Create)];

        return project;
    }

    public static ProjectEntity? Create(ProjectModel model)
    {
        if (model == null) return null!;

        var entity = new ProjectEntity
        {
            Id = model.Id,
            ProjectName = model.ProjectName,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Budget = model.Budget,
            StatusId = model.StatusId,
            ClientId = model.ClientId,
        };
        if (model.Description != null) entity.Description = model.Description;
        if (!string.IsNullOrEmpty(model.ImageUrl)) entity.ImageUrl = model.ImageUrl;

        return entity;
    }
}
