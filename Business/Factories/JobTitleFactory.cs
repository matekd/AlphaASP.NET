using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class JobTitleFactory
{
    public static JobTitle Create(JobTitleEntity entity) => new() { Id = entity.Id, Title = entity.Title };

    public static JobTitleEntity Create(JobTitleModel model) => new() { Id = model.Id, Title = model.Title };

    public static JobTitleModel Create(JobTitle model) => new() { Id = model.Id, Title = model.Title, };
}
