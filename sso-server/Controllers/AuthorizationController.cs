using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Shared;
using sso_server.Models;
using System.Security.Claims;
using webapi.DTOs;

namespace sso_server.Controllers;

public class AuthorizationController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _usersManager;

    public AuthorizationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> usersManager)
    {
        _signInManager = signInManager;
        _usersManager = usersManager;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> GetAccessTokenAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        ClaimsPrincipal claimsPrincipal;


        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        
        // Subject (sub) is a required field, we use the client id as the subject identifier here.
        identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());


        if (request.IsClientCredentialsGrantType())
        {
            // Note: the client credentials are automatically validated by OpenIddict:
            // if client_id or client_secret are invalid, this action won't be invoked.


            // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
            //identity
            //    .AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken)
            //    .SetDestinations(claim=> new [] { OpenIddictConstants.Destinations.AccessToken });

            claimsPrincipal = new ClaimsPrincipal(identity);

            claimsPrincipal.SetScopes(request.GetScopes());
        }

        else if (request.IsAuthorizationCodeGrantType())
        {
            // Retrieve the claims principal stored in the authorization code
            claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal!;
        }

        else if (request.IsRefreshTokenGrantType())
        {
            identity.AddClaim(ClaimTypes.Name, request.ClientId ?? throw new InvalidOperationException());

            // Retrieve the claims principal stored in the refresh token.
            claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal!;
        }
        else
        {
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        claimsPrincipal.SetResources(MyConstants.LibraryApiResource); // obligatory to set audience

        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpGet("~/connect/authorize")]
    //[HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> GetAuthorizaitonCode()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        // Retrieve the user principal stored in the authentication cookie.
        //var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        // If the user principal can't be extracted, redirect the user to the login page.
        if (!result.Succeeded)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                });
        }

        // Create a new claims principal
        var claims = new List<Claim>
        {
            // 'subject' claim which is required
            new Claim(OpenIddictConstants.Claims.Subject, result.Principal.Identity!.Name!).SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            new Claim(OpenIddictConstants.Claims.Role, User.GetClaim(ClaimTypes.Role)!).SetDestinations(OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken)
            //new Claim("some claim", "some value").SetDestinations(OpenIddictConstants.Destinations.AccessToken),
            //new Claim(OpenIddictConstants.Claims.Email, "some@email").SetDestinations(OpenIddictConstants.Destinations.IdentityToken)
        };

        var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Set requested scopes (this is not done automatically)
        claimsPrincipal.SetScopes(request.GetScopes());

        // Signing in with the OpenIddict authentiction scheme trigger OpenIddict to issue a code (which can be exchanged for an access token)
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo")]
    public async Task<IActionResult> Userinfo()
    {
        var claimsPrincipal = (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;

        var role = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Role);

        return Ok(new
        {
            sub = claimsPrincipal!.GetClaim(OpenIddictConstants.Claims.Subject), // required
            role = role
        });
    }

    #region Users managment


    [HttpGet("~/users")]
    public async Task<ActionResult<UserDTO[]>> GetUsers()
    {
        var response = new List<UserDTO>();


        var users = await _usersManager.Users.ToListAsync();

        foreach (var item in users)
        {
            var role = await _usersManager.GetRolesAsync(item);
            response.Add(new UserDTO(item.UserName!, role.First()));
        }

        return Ok(response.ToArray());
    }

    [HttpPost("~/user/{username}")]
    public async Task<ActionResult<UserDTO>> CreateUser(string username, [FromBody] UserDTO request)
    {
        const string newUserDefaultPassword = "SomeStrong@Password123";

        if (request.Role is not (LibraryAppRoles.Admin or LibraryAppRoles.Manager or LibraryAppRoles.User)) return BadRequest();

        var newUser = new ApplicationUser
        {
            UserName = request.Username
        };

        await _usersManager.CreateAsync(newUser, newUserDefaultPassword);
        await _usersManager.AddToRoleAsync(newUser, request.Role);

        return Ok(new UserDTO(request.Username, request.Role));
    }

    [HttpPut("~/user/{username}")]
    public async Task<ActionResult> UpdateUser([FromRoute] string username, [FromBody] UserDTO request)
    {
        if (request.Role is not (LibraryAppRoles.Admin or LibraryAppRoles.Manager or LibraryAppRoles.User)) return BadRequest();

        var foundUser = await _usersManager.FindByNameAsync(request.Username);

        if (foundUser is null) return NoContent();

        // username
        await _usersManager.UpdateAsync(foundUser);

        // role
        var roles = await _usersManager.GetRolesAsync(foundUser);
        await _usersManager.RemoveFromRolesAsync(foundUser, roles);
        await _usersManager.AddToRoleAsync(foundUser, request.Role);

        return Ok();
    }

    [HttpDelete("~/user/{username}")]
    public async Task<ActionResult> RemoveUser(string username)
    {
        var foundUser = await _usersManager.FindByNameAsync(username);

        if (foundUser is null) return NoContent();

        await _usersManager.DeleteAsync(foundUser);

        return Ok();
    }

    #endregion

    [HttpGet("~/connect/logout")]
    public IActionResult Logout() => View();

    [ActionName(nameof(Logout)), HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> LogoutPost()
    {
        await HttpContext.SignOutAsync();
        await _signInManager.SignOutAsync();
        return SignOut(
              authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
              properties: new AuthenticationProperties
              {
                  RedirectUri = "https://localhost:4200/"
              });
    }
}
