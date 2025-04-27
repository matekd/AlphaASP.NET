using Business.Factories;
using Business.Interfaces;
using Business.Services;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApp.Hubs;

namespace WebApp.Controllers;

[Route("members")]
[Authorize(Roles = "Administrator")]
public class MembersController(IMemberService memberService, IAddressService addressService, IWebHostEnvironment env, IHubContext<NotificationHub> notificationHub, INotificationService notificationService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IAddressService _addressService = addressService;
    private readonly IWebHostEnvironment _env = env;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
    private readonly INotificationService _notificationService = notificationService;

    public IEnumerable<Member> MemberList { get; set; } = [];

    [Route("")]
    public async Task<IActionResult> Members()
    {
        var res = await _memberService.GetAllUsersAsync();
        if (res.Success && res.Results != null)
            MemberList = res.Results;
        return View(MemberList);
    }

    [ValidateAntiForgeryToken]
    [Route("Add")]
    [HttpPost]
    public async Task<IActionResult> Add(MemberModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        var filePath = "";
        if (model.MemberImage != null && model.MemberImage.Length != 0)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + '_' + Path.GetFileName(model.MemberImage.FileName);
            filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.MemberImage.CopyToAsync(stream);
            }

            filePath = "uploads/" + fileName;
        }

        model.ImageUrl = filePath;
        var result = await _memberService.CreateUserAsync(model);
        if (!result.Success)
            return BadRequest(new { success = false, submitError = result.Error, result.StatusCode });

        var notification = new NotificationEntity
        {
            Message = $"{model.FirstName} {model.LastName} was created.",
            NotificationType = "Member",
            TargetGroup = "Admin",
            Icon = model.ImageUrl
        };
        await _notificationService.AddNotificationAsync(notification);
        var notifications = await _notificationService.GetNotificationsAsync("");
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
            await _notificationHub.Clients.Group("Admin").SendAsync("ReceiveNotification", newNotification);

        if (model.Address != null)
        {
            var dto = AddressFactory.Create(model.Address.StreetName!, model.Address.PostalCode!, model.Address.City!, result.Result!);
            if (dto != null)
                await _addressService.CreateAsync(dto);
        }
        return Ok(new { success = true });
    }

    [ValidateAntiForgeryToken]
    [Route("Edit")]
    [HttpPut]
    public async Task<IActionResult> EditAsync(MemberModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        var filePath = "";
        if ((model.MemberImage == null || model.MemberImage.Length == 0) && !string.IsNullOrEmpty(model.ImageUrl))
        {
            filePath = model.ImageUrl;
        }
        else if (model.MemberImage != null && model.MemberImage.Length != 0)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + '_' + Path.GetFileName(model.MemberImage.FileName);
            filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.MemberImage.CopyToAsync(stream);
            }

            filePath = "uploads/" + fileName;
        }

        model.ImageUrl = filePath;
        var result = await _memberService.UpdateAsync(model);
        if (!result.Success)
        {
            return BadRequest(new { success = false, submitError = result.Error });
        }

        var notification = new NotificationEntity
        {
            Message = $"{model.FirstName} {model.LastName} was updated.",
            NotificationType = "Member",
            TargetGroup = "Admin",
            Icon = model.ImageUrl
        };
        await _notificationService.AddNotificationAsync(notification);
        var notifications = await _notificationService.GetNotificationsAsync("");
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
            await _notificationHub.Clients.Group("Admin").SendAsync("ReceiveNotification", newNotification);

        return Ok(new { success = true });
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _memberService.DeleteAsync(id);

        return RedirectToAction("Members");
    }
}
