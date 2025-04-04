using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public async Task<bool> LoginAsync(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        return result.Succeeded;
    }

    public async Task<RegisterResult> SignUpAsync(RegisterModel model)
    {
        var entity = MemberFactory.Create(model);
        var result = await _userManager.CreateAsync(entity, model.Password);

        if(result.Succeeded) await _userManager.AddToRoleAsync(entity, "User");

        return result.Succeeded
            ? new RegisterResult { Success = result.Succeeded, StatusCode = 200, Entity = entity.Id }
            : new RegisterResult { Success = result.Succeeded, StatusCode = 500, Error = "Failed to register." };
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> UserExists(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);

        return result != null;
    }
}
