using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<RegisterResult> CreateAsync(ProjectModel model);
    Task<ProjectResult> GetAllAsync();
    Task<BoolResult> UpdateAsync(ProjectModel model);
}