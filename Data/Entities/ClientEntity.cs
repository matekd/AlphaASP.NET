using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(Name), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    // expand on

    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
