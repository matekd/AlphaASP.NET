using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("admin")]
[Authorize(Roles = "Administrator")]
public class AdminController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult Index()
    {
        //return View();
        return LocalRedirect("/login");
    }

    [AllowAnonymous]
    [Route("login")]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [ValidateAntiForgeryToken]
    [AllowAnonymous]
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
}
