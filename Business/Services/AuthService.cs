using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IMemberRepository memberRepository, RoleManager<IdentityRole> roleManager) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<BoolResult> LoginAsync(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        return result.Succeeded
            ? new BoolResult { Success = result.Succeeded, StatusCode = 200, Result = true }
            : new BoolResult { Success = result.Succeeded, StatusCode = 401, Error = "Failed to login." };
    }

    public async Task<BoolResult> SignUpAsync(RegisterModel model)
    {
        var entity = MemberFactory.Create(model);
        var result = await _userManager.CreateAsync(entity, model.Password);

        if(result.Succeeded) await _userManager.AddToRoleAsync(entity, "User");

        return result.Succeeded
            ? new BoolResult { Success = result.Succeeded, StatusCode = 200, Result = true }
            : new BoolResult { Success = result.Succeeded, StatusCode = 500, Error = "Failed to register." };
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<BoolResult> UserExists(string email)
    {
        var result = await _memberRepository.AnyAsync(x => x.Email == email);

        return result.Success
            ? new BoolResult { Success = result.Success, StatusCode = 200, Result = true }
            : new BoolResult { Success = result.Success, StatusCode = 404, Error = "Member not found." };
    }

    public async Task<BoolResult> UserHasRoleAsync(string email, string role)
    {
        var member = await _memberRepository.GetAsync(x => x.Email == email);
        if (!member.Success)
            return new BoolResult { Success = member.Success, StatusCode = 404, Error = "Member not found." };

        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
            return new BoolResult { Success = roleExists, StatusCode = 404, Error = "Role not found." };

        var result = await _userManager.IsInRoleAsync(member.Result!, role);
        return result
            ? new BoolResult { Success = true, StatusCode = 200, Result = true }
            : new BoolResult { Success = false, StatusCode = 500, Error = $"Member does not have role: {role}." };
    }
}
