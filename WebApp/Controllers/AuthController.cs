using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("auth")]
public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    [Route("login")]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

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
            //return BadRequest(new { success = false, errors });
            return View(model);
        }

        var result = await _authService.LoginAsync(model);
        if (result)
        {
            ViewBag.ReturnUrl = returnUrl;
            return LocalRedirect(returnUrl);
        }
        //return BadRequest(new { success = false, submitError = "Email or password is incorrect" });
        ViewBag.ErrorMessage = "Email or password is incorrect";
        return View(model);
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }

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
        
        var result = await _authService.SignUpAsync(model);
        if (result.Success)
            return LocalRedirect("~/");

        ViewBag.ErrorMessage = "Failed to register";
        return View(model);
    }

    [Route("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return LocalRedirect("~/");
        //return RedirectToAction("Login", "Auth");
    }
}
