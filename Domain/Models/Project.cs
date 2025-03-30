using Microsoft.AspNetCore.Http;

namespace Domain.Models;

public class Project
{
    public IFormFile? ProjectImage { get; set; }

    public string ProjectName { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<string>? Status { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public ICollection<string>? Members { get; set; }

    public int Budget { get; set; }
}
