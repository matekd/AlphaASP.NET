using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public async Task<bool> SignUpAsync(AddMemberModel model)
    {
        MemberEntity entity = new()
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
        var result = await _userManager.CreateAsync(entity, "BytMig123!");

        return result.Succeeded;
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
