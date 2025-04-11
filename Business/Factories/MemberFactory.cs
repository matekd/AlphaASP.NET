using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class MemberFactory
{
    public static MemberModel Create(Member member)
    {
        var model = new MemberModel { Id = member.Id, Email = member.Email! };
        if (member.FirstName != null) model.FirstName = member.FirstName;
        if (member.LastName != null) model.LastName = member.LastName;
        if (member.PhoneNumber != null) model.PhoneNumber = member.PhoneNumber;
        if (member.BirthDate != null) model.BirthDate = member.BirthDate;
        //if (member.ImageUrl != null) model.ImageUrl = member.ImageUrl;
        if (member.JobTitle != null) model.JobTitleId = member.JobTitle.Id;
        if (member.Address != null) model.Address = member.Address;
        //if (!string.IsNullOrEmpty(member.ImageUrl)) model.ImageUrl = member.ImageUrl;
        model.ImageUrl = !string.IsNullOrEmpty(member.ImageUrl) ? member.ImageUrl : "";

        return model;
    }

    public static Member Create(MemberEntity entity)
    {
        var member = new Member { Id = entity.Id, Email = entity.Email! };
        if (entity.FirstName != null) member.FirstName = entity.FirstName;
        if (entity.LastName != null) member.LastName = entity.LastName;
        if (entity.PhoneNumber != null) member.PhoneNumber = entity.PhoneNumber;
        if (entity.BirthDate != null) member.BirthDate = entity.BirthDate;
        if (entity.ImageUrl != null) member.ImageUrl = entity.ImageUrl;
        if (entity.JobTitle != null) member.JobTitle = JobTitleFactory.Create(entity.JobTitle);
        if (entity.Address != null) member.Address = AddressFactory.Create(entity.Address);

        return member;
    }

    public static MemberEntity Create(MemberModel model)
    {
        var entity = new MemberEntity
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email,
            JobTitleId = model.JobTitleId,
        };
        if (model.Id != null) entity.Id = model.Id;
        if (model.PhoneNumber != null) entity.PhoneNumber = model.PhoneNumber;
        if (model.BirthDate != null) entity.BirthDate = model.BirthDate;
        //if (!string.IsNullOrEmpty(model.ImageUrl)) entity.ImageUrl = model.ImageUrl;
        entity.ImageUrl = !string.IsNullOrEmpty(model.ImageUrl) ? entity.ImageUrl : "";


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
}
