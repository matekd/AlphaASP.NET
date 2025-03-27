using Domain.Models;

namespace Business.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
}