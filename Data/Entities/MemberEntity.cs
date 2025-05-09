﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class MemberEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? ImageUrl { get; set; }

    [ForeignKey(nameof(JobTitle))]
    public int? JobTitleId { get; set; }
    public virtual JobTitleEntity? JobTitle { get; set; }

    public virtual MemberAddressEntity? Address { get; set; }

    public virtual ICollection<ProjectEntity>? Projects { get; set; }

    public ICollection<NotificationDismissEntity> DismissedNotifications { get; set; } = [];
}
