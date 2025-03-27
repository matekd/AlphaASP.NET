using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class UserFactory
{
    public static User Create(UserEntity entity)
    {
        var user = new User
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            JobTitle = entity.JobTitle,
            BirthDate = entity.BirthDate,
            Address = AddressFactory.Create(entity.Address!),
        };
        return user;
    }

    public static EditMemberModel Create(User user)
    {
        var model = new EditMemberModel
        {

        };
        return model;
    }
}
