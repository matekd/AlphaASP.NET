using Business.Interfaces;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using WebApp.Hubs;

namespace WebApp.Controllers;

[Route("auth")]
public class AuthController(IAuthService authService, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IHubContext<NotificationHub> notificationHub, INotificationService notificationService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IHubContext<NotificationHub> _notificationHub = notificationHub;
    private readonly INotificationService _notificationService = notificationService;

    [Route("login")]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [ValidateAntiForgeryToken]
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model, string returnUrl = "~/")
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return View(model);
        }

        var result = await _authService.LoginAsync(model);
        if (result.Success)
        {
            var member = await _userManager.FindByEmailAsync(model.Email);
            if (member != null)
            {
                var notification = new NotificationEntity
                {
                    Message = $"{member.FirstName} {member.LastName} logged in.",
                    NotificationType = "Member",
                    TargetGroup = "Admin",
                    Icon = member.ImageUrl
                };
                await _notificationService.AddNotificationAsync(notification);
                var notifications = await _notificationService.GetNotificationsAsync("");
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

                if (newNotification != null)
                    await _notificationHub.Clients.Group("Admin").SendAsync("ReceiveNotification", newNotification);
            }

            ViewBag.ReturnUrl = returnUrl;
            return LocalRedirect(returnUrl);
        }
        ViewBag.ErrorMessage = "Email or password is incorrect";
        return View(model);
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return View(model);
        }

        var register = await _authService.SignUpAsync(model);
        if (!register.Success)
        {
            ViewBag.ErrorMessage = "Failed to register";
            return View(model);
        }

        var notification = new NotificationEntity
        {
            Message = $"{model.FirstName} {model.LastName} was registered.",
            NotificationType = "Member",
            TargetGroup = "Admin",
        };
        await _notificationService.AddNotificationAsync(notification);
        var notifications = await _notificationService.GetNotificationsAsync("");
        var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

        if (newNotification != null)
            await _notificationHub.Clients.Group("Admin").SendAsync("ReceiveNotification", newNotification);

        var login = await _authService.LoginAsync(new LoginModel { Email = model.Email, Password = model.Password });
        if (login.Success)
            return LocalRedirect("~/");

        return LocalRedirect("/auth/login");
    }

    [Route("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return LocalRedirect("~/");
    }

    [AllowAnonymous]
    [Route("admin/login")]
    public IActionResult AdminLogin(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [Route("admin/login")]
    [HttpPost]
    public async Task<IActionResult> AdminLogin(LoginModel model, string returnUrl = "~/")
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return View(model);
        }

        var userIsAdmin = await _authService.UserHasRoleAsync(model.Email, "Administrator");
        if (!userIsAdmin.Success)
        {
            ViewBag.ErrorMessage = "Not an Administrator";
            return View(model);
        }

        var result = await _authService.LoginAsync(model);
        if (result.Success)
        {
            ViewBag.ReturnUrl = returnUrl;
            return LocalRedirect(returnUrl);
        }
        ViewBag.ErrorMessage = "Email or password is incorrect";
        return View(model);
    }

    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("Login");
        }

        var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("Login");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("Login");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            string firstName = string.Empty;
            string lastName = string.Empty;
            try
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }
            catch { }
            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string username = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var member = new MemberEntity { Email = email, UserName = username, FirstName = firstName, LastName = lastName };

            var identityResult = await _userManager.CreateAsync(member);
            if (identityResult.Succeeded)
            {
                var notification = new NotificationEntity
                {
                    Message = $"External member {firstName} {lastName} was registered.",
                    NotificationType = "Member",
                    TargetGroup = "Admin",
                };
                await _notificationService.AddNotificationAsync(notification);
                var notifications = await _notificationService.GetNotificationsAsync("");
                var newNotification = notifications.OrderByDescending(x => x.Created).FirstOrDefault();

                if (newNotification != null)
                    await _notificationHub.Clients.Group("Admin").SendAsync("ReceiveNotification", newNotification);

                await _userManager.AddLoginAsync(member, info);
                await _userManager.AddToRoleAsync(member, "User");
                await _signInManager.SignInAsync(member, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Login");
        }
    }
}
