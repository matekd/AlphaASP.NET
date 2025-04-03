using Business.Models;

namespace Business.Interfaces;

public interface IStatusService
{
    Task<StatusResult> CreateAsync(string status);
    Task<StatusResult> ExistsAsync(string status);
    Task<StatusResult> GetAllAsync();
}