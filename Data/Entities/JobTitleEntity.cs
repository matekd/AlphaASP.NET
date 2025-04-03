using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(Title), IsUnique = true)]
public class JobTitleEntity
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public ICollection<MemberEntity> Members { get; set; } = [];
}