using ErolFinal.BL.Exceptions;
using ErolFinal.BL.Services;
using ErolFinal.DAL.Models;
using ErolFinal.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ErolFinal.MVC.Controllers;

public class AccountController : Controller
{
    protected AppUserService _userService;

    public AccountController(AppUserService userService)
    {
        _userService = userService;
    }

    #region Login
    [HttpGet]
    [ActionName("Login")]
    public IActionResult GetLogin()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ActionName("Login")]
    public async Task<IActionResult> PostLogin(AccountLoginVM data)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please fill all of the required fields!");
            return View();
        }
        try
        {
            await _userService.LoginAsync(data.UsernameOrEmail, data.Password);
        }
        catch (LoginException exc)
        {
            foreach (var error in exc.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
        return RedirectToAction("Index", "Home");
    }
    #endregion

    #region Register
    [HttpGet]
    [ActionName("Register")]
    public IActionResult GetRegister()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ActionName("Register")]
    public async Task<IActionResult> PostRegister(AccountRegisterVM data)
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Please fill all of the required fields!");
            return View();
        }
        try
        {
            var user = new AppUser()
            {
               UserName = data.Username,
               Name = data.Name,
               Email = data.Email,
            };
            await _userService.RegisterAsync(user, data.Password);
        }
        catch (RegisterException exc)
        {
            foreach (var error in exc.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
        return RedirectToAction("Login", "Account");
    }
    #endregion

    #region Logout
    [HttpGet]
    [ActionName("Logout")]
    public async Task<IActionResult> GetLogout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await _userService.LogoutAsync();
        }

        return RedirectToAction("Index", "Home");
    }
    #endregion

    #region Hacker zone
    [HttpGet]
    [ActionName("BecomeAdmin")]
    [Authorize]
    public async Task<IActionResult> GetBecomeAdmin()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login");
        }

        var user = await _userService.GetByNameAsync(User.Identity!.Name!);
        if (user == null) // deleted account? not registered? idk
        {
            return RedirectToAction("Login");
        }

        await _userService.MakeAdminAsync(user);
        // wtf? a bug or smth, i cant assign user to an admin role immediately, he has to relogin to account. but why? still idk
        // HEY TEACHER THIS IS A HACK HACK HACK ¯\_(ツ)_/¯ relogin again pls
        await _userService.LogoutAsync();

        return RedirectToAction("Index", "Admin");
    }
    #endregion
}
