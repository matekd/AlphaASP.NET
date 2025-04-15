using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class ClientsController(IClientService clientService) : Controller
{
    private readonly IClientService _clientService = clientService;

    public IActionResult Index()
    {
        return LocalRedirect("~/");
    }

    //public IActionResult Clients()
    //{

    //}

    //[HttpPost]
    //public IActionResult Add()
    //{

    //}

    //[HttpPut]
    //public IActionResult Update()
    //{

    //}

    //[HttpDelete]
    //public IActionResult Delete()
    //{

    //}
}
