using Data.Entities;

namespace Business.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(string message, string notificationType, string targetGroup = "Member", string icon = null);
    Task DismissNotification(string memberId, int notificationId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string memberId, int take = 10);
}