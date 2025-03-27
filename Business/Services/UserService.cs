using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Business.Services;

public class UserService(UserManager<UserEntity> userManager) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<bool> SignUpAsync(AddMemberModel model)
    {
        UserEntity entity = new()
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
        var result = await _userManager.CreateAsync(entity, model.Password);

        return result.Succeeded;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var entities = await _userManager.Users.ToListAsync();
        if (entities.Count == 0)
            return [];

        var users = entities.Select(UserFactory.Create);
        return users;
    }
}
