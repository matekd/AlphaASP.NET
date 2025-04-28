using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IClientService
{
    Task<RegisterResult> CreateAsync(ClientModel model);
    Task<ClientResult> GetAllAsync();
    Task<BoolResult> UpdateAsync(ClientModel model);
    Task<BoolResult> DeleteAsync(int id);
}