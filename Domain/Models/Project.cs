﻿using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class Project
{
    public string? Id { get; set; }
    public IFormFile? ProjectImage { get; set; }

    public string ProjectName { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string? Description { get; set; }


    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }
    public Status Status { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public ICollection<Member>? Members { get; set; }

    public int Budget { get; set; }
}
