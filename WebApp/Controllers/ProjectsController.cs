using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    //Default routing
    public IActionResult Index()
    {
        return RedirectToAction("Projects");
    }

    // "/projects?status=test"
    [Route("projects")]
    public IActionResult Projects(string? status)
    {
        if (status != null)
            Console.WriteLine(status);
        return View();
    }

    [Route("Add")]
    [HttpPost]
    public IActionResult Add(AddProjectModel project)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp =>  kvp.Key,
                    kvp =>  kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        // Send data to service

        return RedirectToAction("Projects", "Projects");
    }

    [Route("Edit")]
    [HttpPost]
    public IActionResult Edit(EditProjectModel project)
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

        // Send data to service

        return RedirectToAction("Projects", "Projects");
    }

    [Route("Delete")]
    [HttpDelete]
    public IActionResult Delete()
    {
        return RedirectToAction("Projects", "Projects");
    }
}
