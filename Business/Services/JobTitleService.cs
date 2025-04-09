using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class JobTitleService(IJobTitleRepository jobTitleRepository) : IJobTitleService
{
    private readonly IJobTitleRepository _jobTitleRepository = jobTitleRepository;

    public async Task<RegisterResult> CreateAsync(AddJobTitleModel model)
    {
        if (model == null)
            return new RegisterResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var existsResult = await _jobTitleRepository.AnyAsync(x => x.Title == model.Title);
        if (existsResult.Success)
            return new RegisterResult { Success = false, StatusCode = 409, Error = "Project already exists." };

        var entity = new JobTitleEntity
        {
            Title = model.Title,
        };

        var result = await _jobTitleRepository.AddAsync(entity);

        return result.Success
            ? new RegisterResult { Success = result.Success, StatusCode = 200, Result = result.Result!.Title }
            : new RegisterResult { Success = result.Success, StatusCode = 500, Error = "Failed to register." };
    }

    public async Task<JobTitleResult> GetAllAsync()
    {
        var res = await _jobTitleRepository.GetAllAsync(orderDescending: false, sortBy: null, where: null, includes: []);

        if (!res.Success)
            return new JobTitleResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var titles = res.Result!.Select(JobTitleFactory.Create);
        return new JobTitleResult { Success = res.Success, StatusCode = res.StatusCode, Results = titles };
    }
}
