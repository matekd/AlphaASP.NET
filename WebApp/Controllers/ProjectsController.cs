using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("projects")]
public class ProjectsController : Controller
{
    [Route("")]
    public IActionResult Projects()
    {
        return View();
    }

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

        return Ok(new { success = true });
    }
}
