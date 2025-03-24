using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    // Add, Edit, Delete
    [Route("members")]
    public IActionResult Members()
    {
        return View();
    }

    [Route("clients")]
    public IActionResult Clients()
    {
        return View();
    }

    [Route("status")]
    public IActionResult Status()
    {
        return View();
    }

    [Route("title")]
    public IActionResult Title()
    {
        return View();
    }
}
