using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class ClientFactory
{
    public static Client Create(ClientEntity entity) => new() { Id = entity.Id, Name = entity.Name };

    public static ClientEntity Create(ClientModel model) => new() { Id = model.Id,  Name = model.Name };

    public static ClientModel Create(Client model) => new() { Id = model.Id,  Name = model.Name };
}
