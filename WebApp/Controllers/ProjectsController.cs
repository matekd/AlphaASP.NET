using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

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
            return BadRequest(new { success = false, submitError = result.Error });
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
            return BadRequest(new { success = false, submitError = result.Error });
        }

        if (!model.MemberIds.IsNullOrEmpty())
        {
            await _projectService.RemoveMemberAsync(model.Id, [.. model.MemberIds!]);
        }

        return Ok(new { success = true });
    }

    [Route("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(id);

        return RedirectToAction("Projects");
    }

    [ValidateAntiForgeryToken]
    [Route("AddMember")]
    [HttpPatch]
    public async Task<IActionResult> AddMember(int Id, string[] MemberIds)
    {
        if (Id <= 0 || MemberIds.IsNullOrEmpty())
        {
            return BadRequest(new { success = false });
        }

        var result = await _projectService.AddMemberAsync(Id, MemberIds);

        return result.Success
            ? Ok(new { success = true })
            : BadRequest(new { success = false, submitError = result.Error });
    }
}
