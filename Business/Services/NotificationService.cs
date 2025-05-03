using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class NotificationService(DataContext context, UserManager<MemberEntity> userManager) : INotificationService
{
    private readonly DataContext _context = context;
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public async Task AddNotificationAsync(NotificationEntity entity)
    {
        if (string.IsNullOrEmpty(entity.Icon))
        {
            switch (entity.NotificationType)
            {
                case "Member":
                    entity.Icon = "/images/DefaultProfile.png";
                    break;
                case "Project":
                    entity.Icon = "";
                    break;
                default:
                    entity.Icon = "";
                    break;
            }
        }
        _context.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<NotificationEntity>> GetNotificationsAsync(string memberId, int take = 5)
    {
        var member = await _userManager.FindByIdAsync(memberId);
        if (member == null)
            return [];

        var allNotifications = await _context.Notifications
            .OrderByDescending(x => x.Created)
            .ToListAsync();
            
        if (!await _userManager.IsInRoleAsync(member!, "Administrator"))
            allNotifications = allNotifications.Where(x => x.TargetGroup == "Member").ToList();

        var dismissed = await _context.DismissedNotifications
            .Where(x => x.MemberId == memberId)
            .Select(x => x.NotificationId)
            .ToListAsync();

        var notifications = allNotifications
            .Where(x => !dismissed.Contains(x.Id))
            .OrderByDescending(x => x.Created)
            .Take(take);

        return notifications;
    }

    public async Task DismissNotification(string memberId, int notificationId)
    {
        var alreadyDismissed = await _context.DismissedNotifications.AnyAsync(x => x.Id == notificationId && x.MemberId == memberId);
        if (!alreadyDismissed)
        {
            var dismissed = new NotificationDismissEntity
            {
                NotificationId = notificationId,
                MemberId = memberId
            };

            _context.Add(dismissed);
            await _context.SaveChangesAsync();
        }
    }
}
