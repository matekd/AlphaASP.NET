﻿using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    public string? LastName { get; set; }

    [ProtectedPersonalData]
    public string? JobTitle { get; set; }

    public DateOnly? BirthDate { get; set; }

    public virtual UserAddressEntity? Address { get; set; }
}
