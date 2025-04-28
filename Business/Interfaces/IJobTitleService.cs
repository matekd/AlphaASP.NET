using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IJobTitleService
{
    Task<RegisterResult> CreateAsync(JobTitleModel model);
    Task<BoolResult> DeleteAsync(int id);
    Task<JobTitleResult> GetAllAsync();
    Task<BoolResult> UpdateAsync(JobTitleModel model);
}