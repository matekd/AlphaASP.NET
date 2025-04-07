using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Linq.Expressions;

namespace Business.Services;

public class StatusService(IStatusRepository statusRepository) : IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<StatusResult> CreateAsync(string status)
    {
        var result = await _statusRepository.AddAsync(new StatusEntity() { StatusName = status });
        return result.Success
            ? new StatusResult { Success = result.Success, StatusCode = result.StatusCode, Result = StatusFactory.Create(result.Result!) }
            : new StatusResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<StatusResult> GetAllAsync()
    {
        var result = await _statusRepository.GetAllAsync();
        return result.Success
            ? new StatusResult { Success = result.Success, StatusCode = result.StatusCode, Results = result.Result!.Select(StatusFactory.Create) }
            : new StatusResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error, Results = [] };
    }








    public async Task<StatusResult> ExistsAsync(string status)
    {
        var result = await _statusRepository.AnyAsync(x => x.StatusName == status);
        return result.Success
            ? new StatusResult { Success = result.Success, StatusCode = result.StatusCode }
            : new StatusResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error };
    }
}
