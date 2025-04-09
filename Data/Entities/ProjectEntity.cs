using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(ProjectName), IsUnique = true)]
public class ProjectEntity
{
    [Key]
    public int Id { get; set; }

    public string? ImageUrl { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly EndDate { get; set; }

    [Required]
    public int Budget { get; set; }

    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;

    public ICollection<MemberEntity>? Members { get; set; }
}