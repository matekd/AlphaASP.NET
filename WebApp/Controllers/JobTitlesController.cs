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
    public IEnumerable<JobTitle> JobTitleList { get; set; } = [];

    [Route("")]
    public async Task<IActionResult> JobTitles()
    {
        var result = await _jobTitleService.GetAllAsync();
        if (result.Success)
            JobTitleList = result.Results!;

        return View(JobTitleList);
    }

    [ValidateAntiForgeryToken]
    [Route("add")]
    [HttpPost]
    public async Task<IActionResult> Add(JobTitleModel model)
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
            return BadRequest(new { success = false, submitError = result.Error });

        return Ok(new { success = true });
    }

    [ValidateAntiForgeryToken]
    [Route("edit")]
    [HttpPut]
    public async Task<IActionResult> Edit(JobTitleModel model)
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

        var result = await _jobTitleService.UpdateAsync(model);
        if (!result.Success)
            return BadRequest(new { success = false, submitError = result.Error });

        return Ok(new { success = true });
    }

    [Route("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _jobTitleService.DeleteAsync(id);

        return RedirectToAction("JobTitles");
    }
}
