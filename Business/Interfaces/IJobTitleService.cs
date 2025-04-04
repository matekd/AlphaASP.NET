using Domain.Models;

namespace Business.Interfaces;

public interface IJobTitleService
{
    Task<bool> Create(AddJobTitleModel model);
}