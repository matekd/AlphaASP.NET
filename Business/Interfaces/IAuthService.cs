using Domain.Models;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> UserExists(string email);
    }
}