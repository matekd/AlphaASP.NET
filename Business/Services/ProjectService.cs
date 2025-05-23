﻿using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<RegisterResult> CreateAsync(ProjectModel model)
    {
        var existsResult = await _projectRepository.AnyAsync(x => x.ProjectName == model.ProjectName);
        if (existsResult.Success)
            return new RegisterResult { Success = false, StatusCode = 409, Error = "Project already exists." };

        var entity = ProjectFactory.Create(model);
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

    public async Task<BoolResult> UpdateAsync(ProjectModel model)
    {
        if (model == null)
            return new BoolResult { Success = false, StatusCode = 400, Error = "Model can't be null."};
        var result = await _projectRepository.UpdateAsync(ProjectFactory.Create(model)!);
        return result.Success
            ? new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Result = result.Result }
            : new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<BoolResult> DeleteAsync(int id)
    {
        var entityRes = await _projectRepository.GetAsync(x => x.Id == id);
        if (!entityRes.Success)
            return new BoolResult { Success = false, StatusCode = 404, Error = "Project not found." };

        var result = await _projectRepository.DeleteAsync(entityRes.Result!);
        return result.Success
            ? new BoolResult { Success = true, StatusCode = result.StatusCode }
            : new BoolResult { Success = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<BoolResult> AddMemberAsync(int projectId, string[] memberIds)
    {
        var projectExists = await _projectRepository.AnyAsync(p => p.Id == projectId);
        if (!projectExists.Success)
            return new BoolResult { Success = false, StatusCode = 404, Error = "Project not found." };
        if (memberIds.IsNullOrEmpty())
            return new BoolResult { Success = false, StatusCode = 400, Error = "Member list can't be empty." };

        foreach (var memberId in memberIds)
        {
            await _projectRepository.AddMemberAsync(projectId, memberId);
        }

        return new BoolResult { Success = true, StatusCode = 200 };
    }

    public async Task<BoolResult> RemoveMemberAsync(int projectId, string[] memberIds)
    {
        var projectExists = await _projectRepository.AnyAsync(p => p.Id == projectId);
        if (!projectExists.Success)
            return new BoolResult { Success = false, StatusCode = 404, Error = "Project not found." };
        if (memberIds.IsNullOrEmpty())
            return new BoolResult { Success = false, StatusCode = 400, Error = "Member list can't be empty." };

        foreach (var memberId in memberIds)
        {
            await _projectRepository.RemoveMemberAsync(projectId, memberId);
        }

        return new BoolResult { Success = true, StatusCode = 200 };
    }
}
