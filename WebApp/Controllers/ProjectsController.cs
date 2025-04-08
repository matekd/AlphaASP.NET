using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController : Controller
{
    public IEnumerable<Project> ProjectList { get; set; } = [];

    //Default routing
    public IActionResult Index()
    {
        return RedirectToAction("Projects");
    }

    // "/projects?status=test"
    [Route("projects")]
    public IActionResult Projects()
    {
        
        return View(ProjectList);
    }

    [ValidateAntiForgeryToken]
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

    [ValidateAntiForgeryToken]
    [Route("Edit")]
    [HttpPatch]
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

        // check if ImageUrl has value but not imageFile. if so, use ImageUrl

        // Send data to service

        return RedirectToAction("Projects", "Projects");
    }

    [ValidateAntiForgeryToken]
    [Route("Delete")]
    [HttpDelete]
    public IActionResult Delete()
    {
        return RedirectToAction("Projects", "Projects");
    }
}
