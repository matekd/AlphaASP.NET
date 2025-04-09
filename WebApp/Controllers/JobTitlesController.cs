using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("titles")]
[Authorize(Roles = "Administrator")]
public class JobTitlesController(IJobTitleService jobTitleService) : Controller
{
    private readonly IJobTitleService _jobTitleService = jobTitleService;

    [Route("")]
    public IActionResult JobTitles()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    [Route("Add")]
    [HttpPost]
    public async Task<IActionResult> Add(AddJobTitleModel model)
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

        var result = await _jobTitleService.CreateAsync(model);

        if (!result.Success)
        {
            ViewBag.ErrorMessage = result.Error;
            return BadRequest(new { success = false });
        }
        return Ok(new { success = true });
    }
}
