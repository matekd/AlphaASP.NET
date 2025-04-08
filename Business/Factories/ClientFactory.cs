using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class ClientFactory
{
    public static Client Create(ClientEntity entity)
    {
        return new Client
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }

    public static ClientEntity Create(AddClientModel model)
    {
        return new ClientEntity { Name = model.Name, };
    }

    public static ClientEntity Create(EditClientModel model)
    {
        return new ClientEntity
        {
            Id = model.Id,
            Name = model.Name,
        };
    }
}
