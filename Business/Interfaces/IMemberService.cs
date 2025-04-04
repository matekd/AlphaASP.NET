using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllUsersAsync();
    Task<RegisterResult> SignUpAsync(AddMemberModel model, string ImageUrl = "");
}