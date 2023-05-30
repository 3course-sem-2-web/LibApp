using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;
using sso_server.Models;
using sso_server.ViewModels;
using System.Security.Claims;

namespace sso_server.Controllers;

public class AccountController : Controller
{
    //private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View(new SignUpViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = new ApplicationUser
        {
            UserName = viewModel.Username,
        };

        var result = await _userManager.CreateAsync(user, viewModel.Password);
        await _userManager.AddToRoleAsync(user, LibraryAppRoles.User);

        if (result.Succeeded)
        {
            //await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Login");
        }

        ModelState.AddModelError(string.Empty, "Error occurred");
        foreach (var item in result.Errors)
        {
            ModelState.AddModelError(item.Code, item.Description);
        }

        return View(viewModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        TempData["ReturnUrl"] = returnUrl ?? TempData["ReturnUrl"];
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        //ViewData["ReturnUrl"] = model.ReturnUrl;

        var foundUser = await _userManager.FindByNameAsync(model.Username);

        if (foundUser == null)
        {
            ModelState.AddModelError(string.Empty, "No such user");
        }

        if (ModelState.IsValid)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, foundUser!.UserName!),
                new Claim(ClaimTypes.Role, string.Join(",", await _userManager.GetRolesAsync(foundUser)))
            };

            //await _signInManager.SignInAsync(new ApplicationUser
            //{
            //    UserName = foundUser.UserName
            //}, false);

            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);

            await _signInManager.SignInWithClaimsAsync(new ApplicationUser
            {
                UserName = foundUser.UserName,
            }, false, claims);
            //await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            string returnUri = TempData["ReturnUrl"]!.ToString()!;

            if (Url.IsLocalUrl(returnUri))
            {
                return Redirect(returnUri);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}
