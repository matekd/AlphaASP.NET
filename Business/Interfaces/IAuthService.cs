using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(LoginModel model);
    Task<RegisterResult> SignUpAsync(RegisterModel model);
    Task LogoutAsync();
    Task<BoolResult> UserExists(string email);
    Task<BoolResult> UserHasRoleAsync(string email, string role);
}