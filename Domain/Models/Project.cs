namespace Domain.Models;

public class Project
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public Status Status { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public IEnumerable<Member>? Members { get; set; }
    public int Budget { get; set; }
}
