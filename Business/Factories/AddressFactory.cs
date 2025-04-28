using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class AddressFactory
{
    public static MemberAddress? Create(MemberAddressEntity entity)
    {
        if (entity == null)
            return null;

        var address = new MemberAddress
        {
            StreetName = entity.StreetName,
            PostalCode = entity.PostalCode,
            City = entity.City,
        };
        return address;
    }

    public static MemberAddressEntity? Create(MemberAddressDto model)
    {
        if (model == null) return null;

        return new MemberAddressEntity
        {
            MemberId = model.MemberId,
            StreetName = model.StreetName,
            PostalCode = model.PostalCode,
            City = model.City,
        };
    }

    public static MemberAddressDto Create(MemberAddress address, string memberId) => new()
    {
        MemberId = memberId,
        StreetName = address.StreetName,
        PostalCode = address.PostalCode,
        City = address.City
    };
}
