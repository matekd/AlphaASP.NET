using Business.Interfaces;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(SignInManager<UserEntity> signInManager) : IAuthService
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

    public async Task<bool> LoginAsync(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        return result.Succeeded;
    }

    public async Task<bool> UserExists(string email)
    {
        return false;
    }
}
