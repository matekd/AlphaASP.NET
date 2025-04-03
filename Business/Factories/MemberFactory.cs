using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class MemberFactory
{
    public static Member Create(MemberEntity entity)
    {
        var member = new Member
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            //JobTitle = entity.JobTitle!.Title,
            BirthDate = entity.BirthDate,
            // create in service, send ready address
            //Address = AddressFactory.Create(entity.Address!),
        };
        return member;
    }

    public static EditMemberModel Create(Member member)
    {
        var model = new EditMemberModel
        {

        };
        return model;
    }
}
