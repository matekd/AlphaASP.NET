using Business.Services;
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

    //[HttpPost]
    //public IActionResult Add()
    //{

    //}

    //[HttpPut]
    //public IActionResult Update()
    //{

    //}

    //public IActionResult Delete()
    //{

    //}
}
