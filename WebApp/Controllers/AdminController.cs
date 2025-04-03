using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        //return View();
        return LocalRedirect("/titles");
    }

    [Route("/titles")]
    public IActionResult JobTitles()
    {
        return View();
    }
}
