﻿using Data.Entities;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;

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

    public static MemberAddressDto? Create(string streetName, string postalCode, string city, string memberId)
    {
        if (string.IsNullOrEmpty(streetName) || string.IsNullOrEmpty(postalCode) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(memberId))
            return null;

        return new MemberAddressDto
        {
            MemberId = memberId,
            StreetName = streetName,
            PostalCode = postalCode,
            City = city,
        };
    }
}
