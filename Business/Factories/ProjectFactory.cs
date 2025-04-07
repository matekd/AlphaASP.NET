using Domain.Models;

namespace Business.Factories;

public static class ProjectFactory
{
    public static EditProjectModel? Create(Project project)
    {
        if (project == null) return null;
        var projectModel = new EditProjectModel
        {
            Id = project.Id,
            ProjectName = project.ProjectName,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Budget = project.Budget,
            ClientId = project.Client.Id,
            StatusId = project.Status.Id,
        };
            //Members = project.Members,
        if (project.Description != null) projectModel.Description = project.Description;
        if (project.ImageUrl != null) projectModel.ImageUrl = project.ImageUrl;
        return projectModel;
    }
}
