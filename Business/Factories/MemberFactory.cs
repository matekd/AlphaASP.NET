using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class MemberFactory
{
    public static Member Create(MemberEntity entity)
    {
        var member = new Member { Id = entity.Id, Email = entity.Email! };
        if (entity.FirstName != null) member.FirstName = entity.FirstName;
        if (entity.LastName != null) member.LastName = entity.LastName;
        if (entity.PhoneNumber != null) member.PhoneNumber = entity.PhoneNumber;
        if (entity.BirthDate != null) member.BirthDate = entity.BirthDate;
        if (entity.ImageUrl != null) member.ImageUrl = entity.ImageUrl;
        if (entity.JobTitle != null) member.JobTitle = entity.JobTitle.Title;
        if (entity.Address != null) member.Address = AddressFactory.Create(entity.Address);

        return member;
    }

    public static MemberEntity Create(AddMemberModel model, string ImageUrl = "")
    {
        var entity = new MemberEntity
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email,
            JobTitleId = model.JobTitleId,
        };
        if (model.PhoneNumber != null) entity.PhoneNumber = model.PhoneNumber;
        if (model.BirthDate != null) entity.BirthDate = model.BirthDate;
        if (!string.IsNullOrEmpty(ImageUrl)) entity.ImageUrl = ImageUrl;
        
        return entity;
    }

    public static MemberEntity Create(RegisterModel model)
    {
        var entity = new MemberEntity
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };
        return entity;
    }

    public static EditMemberModel Create(Member member)
    {
        var model = new EditMemberModel
        {

        };
        return model;
    }
}
