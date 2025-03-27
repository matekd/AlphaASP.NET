using Data.Entities;
using Domain.Models;

namespace Business.Factories;

public static class AddressFactory
{
    public static UserAddress? Create(UserAddressEntity entity)
    {
        if (entity == null)
            return null;

        var address = new UserAddress
        {
            StreetName = entity.StreetName,
            PostalCode = entity.PostalCode,
            City = entity.City,
        };
        return address;
    }
}
