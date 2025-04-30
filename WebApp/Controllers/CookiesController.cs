using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApp.Controllers;

public class CookiesController : Controller
{
    [HttpPost]
    public IActionResult SetCookies([FromBody] CookieConsent consent)
    {
        if (consent == null)
            return BadRequest();

        //if (consent.Functional)
        //{
        //    Response.Cookies.Append("DarkmodeCookie", "light", new CookieOptions
        //    {
        //        IsEssential = false,
        //        Expires = DateTimeOffset.UtcNow.AddDays(365),
        //        SameSite = SameSiteMode.Lax,
        //        Path = "/"
        //    });
        //}
        //else
        //{
        //    Response.Cookies.Delete("DarkmodeCookie");
        //}

        Response.Cookies.Append("cookieConsent", JsonSerializer.Serialize(consent), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(365),
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });

        return Ok();
    }
}
