using Business.Factories;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<RegisterResult> CreateAsync(ClientModel model)
    {
        if (model == null)
            return new RegisterResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var existsResult = await _clientRepository.AnyAsync(x => x.Name == model.Name);
        if (existsResult.Success)
            return new RegisterResult { Success = false, StatusCode = 409, Error = "Client already exists." };

        var entity = new ClientEntity { Name = model.Name };

        var result = await _clientRepository.AddAsync(entity);
        return result.Success
            ? new RegisterResult { Success = result.Success, StatusCode = 200, Result = result.Result!.Name }
            : new RegisterResult { Success = result.Success, StatusCode = 500, Error = "Failed to create client." };
    }

    public async Task<ClientResult> GetAllAsync()
    {
        var res = await _clientRepository.GetAllAsync(orderDescending: false, sortBy: null, where: null, includes: []);

        if (!res.Success)
            return new ClientResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var clients = res.Result!.Select(ClientFactory.Create);
        return new ClientResult { Success = res.Success, StatusCode = res.StatusCode, Results = clients };
    }

    public async Task<BoolResult> UpdateAsync(ClientModel model)
    {
        if (model == null)
            return new BoolResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var existsResult = await _clientRepository.AnyAsync(x => x.Name == model.Name);
        if (existsResult.Success)
            return new BoolResult { Success = false, StatusCode = 409, Error = "Client already exists." };

        var result = await _clientRepository.UpdateAsync(ClientFactory.Create(model));
        return result.Success
            ? new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Result = result.Result }
            : new BoolResult { Success = result.Success, StatusCode = result.StatusCode, Error = result.Error };
    }
    public async Task<BoolResult> DeleteAsync(int id)
    {
        var entityRes = await _clientRepository.GetAsync(x => x.Id == id);
        if (!entityRes.Success)
            return new BoolResult { Success = false, StatusCode = 404, Error = "Client not found." };

        var result = await _clientRepository.DeleteAsync(entityRes.Result!);
        return result.Success
            ? new BoolResult { Success = true, StatusCode = result.StatusCode }
            : new BoolResult { Success = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}
