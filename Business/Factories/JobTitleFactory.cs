using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class JobTitleFactory
{
    public static JobTitle Create(JobTitleEntity entity)
    {
        return new JobTitle
        {
            Id = entity.Id,
            Title = entity.Title,
        };
    }

    public static JobTitleEntity Create(AddJobTitleModel model)
    {
        return new JobTitleEntity
        {
            Title = model.Title,
        };
    }

    public static JobTitleEntity Create(EditJobTitleModel model)
    {
        return new JobTitleEntity
        {
            Id = model.Id,
            Title = model.Title,
        };
    }
}
