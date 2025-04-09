using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;
public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    public virtual async Task<RepositoryResult<bool>> AddMemberAsync(int projectId, string memberId)
    {
        var project = await _dbSet.FirstOrDefaultAsync(x => x.Id == projectId);
        if (project == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 404, Error = "Project not found." };

        var member = await _context.Users.FirstOrDefaultAsync(x => x.Id == memberId);
        if (member == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 404, Error = "Member not found." };

        try
        {
            project!.Members!.Add(member);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Success = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = ex.Message };
        }
        //if (project == null)
        //    return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Project can't be null." };

        //if (member == null)
        //    return new RepositoryResult<bool> { Success = false, StatusCode = 400, Error = "Member can't be null." };

        //try
        //{
        //    project!.Members!.Add(member);
        //    _dbSet.Update(project);
        //    await _context.SaveChangesAsync();
        //    return new RepositoryResult<bool> { Success = true, StatusCode = 201 };
        //}
        //catch (Exception ex)
        //{
        //    Debug.WriteLine(ex.Message);
        //    return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = ex.Message };
        //}
    }

    public virtual async Task<RepositoryResult<bool>> RemoveMemberAsync(int projectId, string memberId)
    {
        var project = await _dbSet.FirstOrDefaultAsync(x => x.Id == projectId);
        if (project == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 404, Error = "Project not found." };

        var member = await _context.Users.FirstOrDefaultAsync(x => x.Id == memberId);
        if (member == null)
            return new RepositoryResult<bool> { Success = false, StatusCode = 404, Error = "Member not found." };

        try
        {
            project!.Members!.Remove(member);
            await _context.SaveChangesAsync();
            return new RepositoryResult<bool> { Success = true, StatusCode = 201 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new RepositoryResult<bool> { Success = false, StatusCode = 500, Error = ex.Message };
        }
    }
}