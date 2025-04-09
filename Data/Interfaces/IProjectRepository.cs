using Data.Entities;
using Data.Models;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<RepositoryResult<bool>> AddMemberAsync(int projectId, string memberId);
    Task<RepositoryResult<bool>> RemoveMemberAsync(int projectId, string memberId);
}
