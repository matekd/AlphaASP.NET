using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    public IActionResult Login()
    {
        return View();
    }

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

            return BadRequest(new { success = false, errors });
        }

        var result = await _authService.LoginAsync(model);
        if (result)
            return Redirect(returnUrl);

        return BadRequest(new { success = false, submitError = "Email or password is incorrect" });
        //return View(model);
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        return View();
    }





    [Route("logout")]
    public IActionResult Logout()
    {
        return LocalRedirect("/projects");
        //return View();
    }
}
