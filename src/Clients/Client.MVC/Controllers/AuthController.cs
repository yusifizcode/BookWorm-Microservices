using Client.MVC.Models;
using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Client.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IIdentityService _identityService;
    public AuthController(IIdentityService identityService)
        => _identityService = identityService;

    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpInput signUpInput)
    {
        if (!ModelState.IsValid)
            return View();

        var response = await _identityService.SignUp(signUpInput);

        if (!response.IsSuccessful)
        {
            response.Errors.ForEach(e =>
            {
                ModelState.AddModelError("SignUp", e);
            });

            return View();
        }
        return RedirectToAction(nameof(SignIn));
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInInput signInInput)
    {
        if (!ModelState.IsValid)
            return View();

        var response = await _identityService.SignIn(signInInput);

        if (!response.IsSuccessful)
        {
            response.Errors.ForEach(x =>
            {
                ModelState.AddModelError(string.Empty, x);
            });
            return View();
        }


        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await _identityService.RevokeRefreshToken();
        return RedirectToAction("Index", "Home");
    }
}
