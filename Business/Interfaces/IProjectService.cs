using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<RegisterResult> CreateAsync(AddProjectModel model, string ImageUrl = "");
    Task<ProjectResult> GetAllAsync();
}