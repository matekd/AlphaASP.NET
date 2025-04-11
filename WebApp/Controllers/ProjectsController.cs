using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Authorize]
public class ProjectsController(IProjectService projectService, IWebHostEnvironment env) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IWebHostEnvironment _env = env;
    public IEnumerable<Project> ProjectList { get; set; } = [];

    //Default routing
    public IActionResult Index()
    {
        return RedirectToAction("Projects");
    }

    [Route("projects")]
    public async Task<IActionResult> ProjectsAsync()
    {
        var res = await _projectService.GetAllAsync();
        if (res.Success && res.Results != null)
            ProjectList = res.Results;
        return View(ProjectList);
    }

    [ValidateAntiForgeryToken]
    [Route("Add")]
    [HttpPost]
    public async Task<IActionResult> Add(ProjectModel model)
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

        var filePath = "";
        if (model.ProjectImage != null && model.ProjectImage.Length != 0)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + '_' + Path.GetFileName(model.ProjectImage.FileName);

            filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProjectImage.CopyToAsync(stream);
            }

            filePath = "uploads/" + fileName;
        }

        model.ImageUrl = filePath;
        var result = await _projectService.CreateAsync(model);
        if (!result.Success)
        {
            ViewBag.ErrorMessage = result.Error;
            return BadRequest(new { success = false });
        }
        
        return Ok(new { success = true });
    }

    [ValidateAntiForgeryToken]
    [Route("Edit")]
    [HttpPut]
    public async Task<IActionResult> Edit(ProjectModel model)
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

        var filePath = "";
        // check if a new image wasn't selected, if so use previous ImageUrl
        if ((model.ProjectImage == null || model.ProjectImage.Length == 0) && !string.IsNullOrEmpty(model.ImageUrl))
        {
            filePath = model.ImageUrl;
        }
        else if (model.ProjectImage != null && model.ProjectImage.Length != 0)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + '_' + Path.GetFileName(model.ProjectImage.FileName);

            filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProjectImage.CopyToAsync(stream);
            }

            filePath = "uploads/" + fileName;
        }
        
        model.ImageUrl = filePath;
        var result = await _projectService.UpdateAsync(model);
        if (!result.Success)
        {
            ViewBag.ErrorMessage = result.Error;
            return BadRequest(new { success = false });
        }

        return Ok(new { success = true });
    }

    [ValidateAntiForgeryToken]
    [Route("Delete")]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        return RedirectToAction("Projects", "Projects");
    }
}
