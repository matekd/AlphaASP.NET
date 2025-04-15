using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<RegisterResult> CreateAsync(ProjectModel model);
    Task<BoolResult> UpdateAsync(ProjectModel model);
    Task<ProjectResult> GetAllAsync();
    Task<BoolResult> AddMemberAsync(int projectId, string[] memberIds);
    Task<BoolResult> RemoveMemberAsync(int projectId, string[] memberIds);
}