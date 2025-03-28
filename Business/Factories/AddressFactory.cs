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
}
