
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fintech.Library.Business.Abstract;
using Fintech.Library.Entities.Dto;
using System.Security.Claims;
using Fintech.Library.Entities.Concrete;
using Fintech.Library.Entities.Models;
using System.Globalization;

namespace Fintech.Presentation.WEB.MerchantPanel.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly IFixerService _fixerService;


    public AuthController(IUserService userService, IFixerService fixerService)
    {
        _userService = userService;
        _fixerService = fixerService;
    }

    public async Task<IActionResult> Login()
    {
      
        
            return View(viewName: "Login");
    }
    public async Task<IActionResult> Register()
    {

        return View(viewName: "Register");
    }


    [HttpPost]
    public async Task<IActionResult> Register(User User,string Password)
    {

        var Result = await _userService.Register(User, Password);
        if (Result.Success)
        {
            return RedirectToAction("Login");
        }
        return View(viewName: "Login");
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel Model)
    {
        var Result = await _userService.Login(Model);
        if (Result.Success)
        {
            await SignIn(Result.Data);
            return Redirect("/Home/index");
        }
        return View(viewName: "Login");
    }

    private async Task SignIn(User model)
    {
        var claims = new List<Claim>
        {
            new Claim("Email",model.Email ),
            new Claim("UserId",model.Id.ToString() ),
            new Claim("FirstName",model.FirstName),
            new Claim("LastName",model.LastName),
            new Claim("UserPrice",string.Format(new CultureInfo("fr-FR"), "{0:C}", model.UserPrice))
        };


        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            RedirectUri = "/Home",
            ExpiresUtc = DateTimeOffset.Now.AddHours(24),

        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Auth");
    }
}
