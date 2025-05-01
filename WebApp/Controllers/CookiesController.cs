using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApp.Controllers;

//[Route("cookies")]
public class CookiesController : Controller
{
    [Route("cookies/setcookies")]
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

        //if (!consent.Functional)
        //{
        //    Response.Cookies.Delete("ThemeCookie");
        //}

        Response.Cookies.Append("consentCookie", JsonSerializer.Serialize(consent), new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(365),
            SameSite = SameSiteMode.Lax,
            Path = "/"
        });

        return Ok();
    }
}
