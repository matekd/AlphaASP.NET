using Business.Models;
using Data.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<RegisterResult> CreateUserAsync(MemberModel model);
    Task<MemberResult> GetAllUsersAsync();
    Task<MemberResult> GetUserAsync(Expression<Func<MemberEntity, bool>> expression);
    Task<BoolResult> UpdateAsync(MemberModel model);
}