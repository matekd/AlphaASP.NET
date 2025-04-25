using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class NotificationEntity
{
    [Key]
    public int Id { get; set; }
    public string TargetGroup { get; set; } = null!;
    public string? Icon { get; set; }
    public string Message { get; set; } = null!;
    public string NotificationType { get; set; } = null!; // member / project. Determines default image
    public DateTime Created { get; set; } = DateTime.Now;
    public ICollection<NotificationDismissEntity> Dismissed { get; set; } = [];
}
