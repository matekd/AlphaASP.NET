using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginModel model);
    Task<RegisterResult> SignUpAsync(RegisterModel model);
    Task LogoutAsync();
    Task<bool> UserExists(string email);
}