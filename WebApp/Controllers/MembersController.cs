﻿using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("members")]
[Authorize(Roles = "Administrator")]
public class MembersController(IMemberService memberService, IWebHostEnvironment env) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IWebHostEnvironment _env = env;

    [Route("")]
    public async Task<IActionResult> Members()
    {
        var members = await _memberService.GetAllUsersAsync();
        return View(members);
    }

    [Route("Add")]
    [HttpPost]
    public async Task<IActionResult> Add(AddMemberModel model)
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

        if (model.MemberImage != null && model.MemberImage.Length != 0)
        {
            var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + '_' + Path.GetFileName(model.MemberImage.FileName);

            // full path
            filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.MemberImage.CopyToAsync(stream);
            }

            // relative path
            filePath = "uploads/" + fileName;
        }
        
        // Send data to service
        var result = await _memberService.SignUpAsync(model, filePath);

        // create address (with member id)

        return Ok(new { success = true });
    }

    [Route("Edit")]
    [HttpPost]
    public IActionResult Edit(EditMemberModel model)
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

        return Ok(new { success = true });
    }
}
