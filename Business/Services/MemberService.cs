using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public async Task<RegisterResult> SignUpAsync(AddMemberModel model, string ImageUrl = "")
    {
        //var result = await _userManager.CreateAsync(MemberFactory.Create(model, ImageUrl), "BytMig123!");
        //return result.Succeeded;
        var entity = MemberFactory.Create(model, ImageUrl);
        var result = await _userManager.CreateAsync(entity, "BytMig123!");

        if (result.Succeeded) await _userManager.AddToRoleAsync(entity, "User");

        return result.Succeeded
            ? new RegisterResult { Success = result.Succeeded, StatusCode = 200, Entity = entity.Id }
            : new RegisterResult { Success = result.Succeeded, StatusCode = 500, Error = "Failed to register." };
    }

    public async Task<IEnumerable<Member>> GetAllUsersAsync()
    {
        var entities = await _userManager.Users.ToListAsync();
        if (entities.Count == 0)
            return [];

        var members = entities.Select(MemberFactory.Create);
        return members;
    }
}
