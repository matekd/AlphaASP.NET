using Business.Factories;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    // create

    public async Task<ClientResult> GetAllAsync()
    {
        var res = await _clientRepository.GetAllAsync(orderDescending: false, sortBy: null, where: null, includes: []);

        if (!res.Success)
            return new ClientResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var clients = res.Result!.Select(ClientFactory.Create);
        return new ClientResult { Success = res.Success, StatusCode = res.StatusCode, Results = clients };
    }

    // update

    // delete
}
