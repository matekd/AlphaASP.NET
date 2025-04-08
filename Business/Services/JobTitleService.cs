using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class JobTitleService(IJobTitleRepository jobTitleRepository) : IJobTitleService
{
    private readonly IJobTitleRepository _jobTitleRepository = jobTitleRepository;

    public async Task<bool> Create(AddJobTitleModel model)
    {

        // to repo
        //RepositoryResult result = 
        return true;
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
