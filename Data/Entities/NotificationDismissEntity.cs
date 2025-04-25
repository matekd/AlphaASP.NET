using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class NotificationDismissEntity
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(NotificationEntity))]
    public int NotificationId { get; set; }
    public NotificationEntity Notification { get; set; } = null!;

    [ForeignKey(nameof(MemberEntity))]
    public string MemberId { get; set; } = null!;
    public MemberEntity Member { get; set; } = null!;
}
