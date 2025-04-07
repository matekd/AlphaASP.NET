using Business.Models;
using Data.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<RegisterResult> CreateUserAsync(AddMemberModel model, string ImageUrl = "");
    Task<MemberResult> GetAllUsersAsync();
    Task<MemberResult> GetUserAsync(Expression<Func<MemberEntity, bool>> expression);
}