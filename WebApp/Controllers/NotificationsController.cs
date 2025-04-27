using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApp.Hubs;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(IHubContext<NotificationHub> notificationHub, INotificationService notificationService) : ControllerBase
{
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
    private readonly INotificationService _notificationService = notificationService;

    [HttpPost]
    public async Task<IActionResult> CreateNotification(NotificationEntity entity)
    {
        await _notificationService.AddNotificationAsync(entity);
        var notifications = await _notificationService.GetNotificationsAsync("");
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
            await _notificationHub.Clients.All.SendAsync("ReceiveNotification", newNotification);

        return Ok(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications()
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (string.IsNullOrEmpty(memberId))
            return Unauthorized();

        var notifications = await _notificationService.GetNotificationsAsync(memberId);
        return Ok(notifications);
    }

    [HttpPost("dismiss/{id}")]
    public async Task<IActionResult> DismissNotification(int id)
    {
        var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (string.IsNullOrEmpty(memberId))
            return Unauthorized();

        await _notificationService.DismissNotification(memberId, id);
        await _notificationHub.Clients.User(memberId).SendAsync("NotificationDismissed", id);
        return Ok(new { success = true });
    }
}
