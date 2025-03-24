using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    public IActionResult Login()
    {
        return LocalRedirect("/projects");
        //return View();
    }

    public IActionResult Register()
    {
        return LocalRedirect("/projects");
        //return View();
    }

    public IActionResult Logout()
    {
        return LocalRedirect("/projects");
        //return View();
    }

    public IActionResult LoginAdmin()
    {
        return LocalRedirect("/projects");
        //return View();
    }

    public IActionResult LogoutAdmin()
    {
        return LocalRedirect("/projects");
        //return View();
    }
}
