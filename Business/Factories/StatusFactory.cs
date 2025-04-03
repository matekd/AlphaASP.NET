using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class StatusFactory
{
    public static Status Create(StatusEntity entity)
    {
        return new Status
        {
            Id = entity.Id,
            StatusName = entity.StatusName,
        };
    }
}
