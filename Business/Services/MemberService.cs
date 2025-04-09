using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, IMemberRepository memberRepository) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberRepository _memberRepository = memberRepository;

    public async Task<RegisterResult> CreateUserAsync(AddMemberModel model, string ImageUrl = "")
    {
        var existsResult = await _memberRepository.AnyAsync(x => x.Email == model.Email);
        if (existsResult.Success)
            return new RegisterResult { Success = false, StatusCode = 409, Error = "Member already exists." };

        var entity = MemberFactory.Create(model, ImageUrl);
        if (entity == null)
            return new RegisterResult { Success = false, StatusCode = 400, Error = "Invalid request." };

        var result = await _userManager.CreateAsync(entity, "BytMig123!");

        if (result.Succeeded) await _userManager.AddToRoleAsync(entity, "User");

        return result.Succeeded
            ? new RegisterResult { Success = result.Succeeded, StatusCode = 200, Result = entity.Id }
            : new RegisterResult { Success = result.Succeeded, StatusCode = 500, Error = "Failed to register." };
    }

    public async Task<MemberResult> GetAllUsersAsync()
    {
        var res = await _memberRepository.GetAllAsync
            (
                orderDescending: false,
                sortBy: null,
                where: null,
                x => x.Address!,
                x => x.JobTitle!
            );

        if (!res.Success)
            return new MemberResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var members = res.Result!.Select(MemberFactory.Create);
        return new MemberResult { Success = res.Success, StatusCode = res.StatusCode, Results = members};
    }

    public async Task<MemberResult> GetUserAsync(Expression<Func<MemberEntity, bool>> expression)
    {
        var res = await _memberRepository.GetAsync
            (
                where: expression,
                x => x.Address!,
                x => x.JobTitle!
            );

        if (!res.Success)
            return new MemberResult { Success = res.Success, StatusCode = res.StatusCode, Error = res.Error };

        var member = MemberFactory.Create(res.Result!);
        return new MemberResult { Success = res.Success, StatusCode = res.StatusCode, Result = member };
    }
}
