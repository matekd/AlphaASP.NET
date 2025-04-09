using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IJobTitleService
{
    Task<RegisterResult> CreateAsync(AddJobTitleModel model);
    Task<JobTitleResult> GetAllAsync();
}