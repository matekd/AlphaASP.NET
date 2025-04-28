using Business.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("clients")]
public class ClientsController(IClientService clientService) : Controller
{
    private readonly IClientService _clientService = clientService;
    public IEnumerable<Client> ClientList { get; set; } = [];

    [Route("")]
    public async Task<IActionResult> Clients()
    {
        var result = await _clientService.GetAllAsync();
        if (result.Success)
            ClientList = result.Results!;

        return View(ClientList);
    }

    [ValidateAntiForgeryToken]
    [Route("add")]
    [HttpPost]
    public async Task<IActionResult> Add(ClientModel model)
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

        var result = await _clientService.CreateAsync(model);
        if (!result.Success)
            return BadRequest(new { success = false, submitError = result.Error });

        return Ok(new { success = true });
    }

    [ValidateAntiForgeryToken]
    [Route("edit")]
    [HttpPut]
    public async Task<IActionResult> Edit(ClientModel model)
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

        var result = await _clientService.UpdateAsync(model);
        if (!result.Success)
            return BadRequest(new { success = false, submitError = result.Error });

        return Ok(new { success = true });
    }

    [Route("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clientService.DeleteAsync(id);

        return RedirectToAction("Clients");
    }
}
