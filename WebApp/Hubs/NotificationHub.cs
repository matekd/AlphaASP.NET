using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
namespace WebApp.Hubs;

[Authorize]
public class NotificationHub(UserManager<MemberEntity> userManager) : Hub
{
    private readonly UserManager<MemberEntity> _userManager = userManager;

    public override async Task OnConnectedAsync()
    {
        var member = await _userManager.FindByIdAsync(Context.UserIdentifier!);
        if (await _userManager.IsInRoleAsync(member!, "Administrator"))
        {
            await AddToGroup("Admin");
        }
        //await AddToGroup("All");
    }

    public async Task SendNotificationToAll(object notification)
    {
        await Clients.All.SendAsync("ReceiveNotification", notification);
    }

    public async Task SendNotificationToAdmins(object notification)
    {
        await Clients.Group("Admin").SendAsync("ReceiveNotification", notification);
    }

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
