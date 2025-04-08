using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IAuthService
{
    Task<BoolResult> LoginAsync(LoginModel model);
    Task<BoolResult> SignUpAsync(RegisterModel model);
    Task LogoutAsync();
    Task<BoolResult> UserExists(string email);
    Task<BoolResult> UserHasRoleAsync(string email, string role);
}