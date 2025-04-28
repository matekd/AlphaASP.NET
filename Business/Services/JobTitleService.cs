using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Models;

namespace Business.Services;

public class JobTitleService(IJobTitleRepository jobTitleRepository) : IJobTitleService
{
    private readonly IJobTitleRepository _jobTitleRepository = jobTitleRepository;

    public async Task<RegisterResult> CreateAsync(JobTitleModel model)
    {
        if (model == null)
            return new RegisterResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var existsResult = await _jobTitleRepository.AnyAsync(x => x.Title == model.Title);
        if (existsResult.Success)
            return new RegisterResult { Success = false, StatusCode = 409, Error = "Job Title already exists." };

        var entity = new JobTitleEntity { Title = model.Title };

        var result = await _jobTitleRepository.AddAsync(entity);
        return result.Success
            ? new RegisterResult { Success = result.Success, StatusCode = 200, Result = result.Result!.Title }
            : new RegisterResult { Success = result.Success, StatusCode = 500, Error = "Failed to create Job Title." };
    }

    public async Task<JobTitleResult> GetAllAsync()
    {
        var res = await _jobTitleRepository.GetAllAsync(orderDescending: false, sortBy: null, where: null, includes: []);

        if (!res.Success)
            return new JobTitleResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var titles = res.Result!.Select(JobTitleFactory.Create);
        return new JobTitleResult { Success = res.Success, StatusCode = res.StatusCode, Results = titles };
    }

    public async Task<BoolResult> UpdateAsync(JobTitleModel model)
    {
        if (model == null)
            return new BoolResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var existsResult = await _jobTitleRepository.AnyAsync(x => x.Title == model.Title);
        if (existsResult.Success)
            return new BoolResult { Success = false, StatusCode = 409, Error = "Job Title already exists." };

        var result = await _jobTitleRepository.UpdateAsync(JobTitleFactory.Create(model));
        return result.Success
            ? new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Result = result.Result }
            : new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error };
    }
    public async Task<BoolResult> DeleteAsync(int id)
    {
        var entityRes = await _jobTitleRepository.GetAsync(x => x.Id == id);
        if (!entityRes.Success)
            return new BoolResult { Success = false, StatusCode = 404, Error = "Job Title not found." };

        var result = await _jobTitleRepository.DeleteAsync(entityRes.Result!);
        return result.Success
            ? new BoolResult { Success = true, StatusCode = result.StatusCode }
            : new BoolResult { Success = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}
