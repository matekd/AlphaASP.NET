using Business.Models;
using Data.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<RegisterResult> CreateAsync(ProjectModel model);
    Task<BoolResult> UpdateAsync(ProjectModel model);
    Task<BoolResult> AddMemberAsync(int projectId, string[] memberIds);
    Task<ProjectResult> GetAllAsync();
}