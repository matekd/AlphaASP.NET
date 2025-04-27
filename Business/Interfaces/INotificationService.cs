using Data.Entities;

namespace Business.Interfaces;

public interface INotificationService
{
    Task AddNotificationAsync(NotificationEntity entity);
    Task DismissNotification(string memberId, int notificationId);
    Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string memberId, int take = 10);
}