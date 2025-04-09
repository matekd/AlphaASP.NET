using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<RegisterResult> CreateAsync(AddProjectModel model, string ImageUrl = "")
    {
        var existsResult = await _projectRepository.AnyAsync(x => x.ProjectName == model.ProjectName);
        if (existsResult.Success)
            return new RegisterResult { Success = false, StatusCode = 409, Error = "Project already exists." };

        var entity = ProjectFactory.Create(model, ImageUrl);
        if (entity == null)
            return new RegisterResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var result = await _projectRepository.AddAsync(entity);

        return result.Success
            ? new RegisterResult { Success = result.Success, StatusCode = result.StatusCode }
            : new RegisterResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error};
    }

    public async Task<ProjectResult> GetAllAsync()
    {
        var res = await _projectRepository.GetAllAsync
            (
                orderDescending: false,
                sortBy: null,
                where: null,
                x => x.Status,
                x => x.Client,
                x => x.Members!
            );

        if (!res.Success)
            return new ProjectResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var projects = res.Result!.Select(ProjectFactory.Create);
        return projects != null
            ? new ProjectResult { Success = res.Success, StatusCode = res.StatusCode, Results = projects! }
            : new ProjectResult { Success = false, StatusCode = 404, Error = "No projects found." };
    }
}
