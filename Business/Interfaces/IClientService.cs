using Business.Models;

namespace Business.Services;

public interface IClientService
{
    Task<ClientResult> GetAllAsync();
}