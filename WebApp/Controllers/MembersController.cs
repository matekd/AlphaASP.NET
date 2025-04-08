﻿using Business.Factories;
using Business.Interfaces;
using Business.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("members")]
[Authorize(Roles = "Administrator")]
public class MembersController(IMemberService memberService, IAddressService addressService, IWebHostEnvironment env) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IAddressService _addressService = addressService;
    private readonly IWebHostEnvironment _env = env;
    public IEnumerable<Member> MemberList { get; set; } = [];

    [Route("")]
    public async Task<IActionResult> Members()
    {
        var res = await _memberService.GetAllUsersAsync();
        if (res.Success && res.Results != null)
            MemberList = res.Results;
        return View(MemberList);
    }

    [ValidateAntiForgeryToken]
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
        
        var result = await _memberService.CreateUserAsync(model, filePath);
        if (!result.Success)
        {
            ViewBag.ErrorMessage = result.Error;
            return BadRequest(new { success = false });
        }
        if (model.Address != null)
        {
            var dto = AddressFactory.Create(model.Address.StreetName!, model.Address.PostalCode!, model.Address.City!, result.Result!);
            if (dto != null)
                await _addressService.CreateAsync(dto);
        }
        return Ok(new { success = true });
    }

    [ValidateAntiForgeryToken]
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
