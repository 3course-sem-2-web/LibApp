using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using Shared;
using sso_server.Data;
using sso_server.Models;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace sso_server;

public class TestData : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public TestData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await AddClients();
        await AddScopes();
        await AddAspNetIdentityRoles();
        await AddUsers();
    }

    public async Task AddClients()
    {

        await using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        
        var api_client = await manager.FindByClientIdAsync(MyConstants.LibraryApiResource);
        var postman_client = await manager.FindByClientIdAsync(MyConstants.LibraryPostmanClient);
        var angular_client = await manager.FindByClientIdAsync(MyConstants.LibraryAngularApp);

        if (api_client != null)
        {
            await manager.DeleteAsync(api_client);
        }

        if (postman_client != null)
        {
            await manager.DeleteAsync(postman_client);
        }

        if (angular_client != null)
        {
            await manager.DeleteAsync(angular_client);
        }

        // API
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = MyConstants.LibraryApiResource,
            ClientSecret = "CF8B4485-AA73-46EC-8D68-CE8059DE4E13",
            DisplayName = "Library.API",
            Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Introspection,
                    },
        });
        
        // Postman
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = MyConstants.LibraryPostmanClient,
            ClientSecret = "CF8B4485-AA73-46EC-8D68-CE8059DE4E13",
            DisplayName = "Postman",
            RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
            Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.Endpoints.Introspection,

                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.Prefixes.Scope + MyConstants.LibraryApiScope,
                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    },
            //PostLogoutRedirectUris = { new Uri("https://localhost:7278") } 
        });

        // angular web ui
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = MyConstants.LibraryAngularApp,
            Type = "public", // !!!
            //ClientSecret = "CF8B4485-AA73-46EC-8D68-CE8059DE4E13",
            DisplayName = "Library.Web.Angular",
            RedirectUris = { new Uri("https://localhost:4200") },
            PostLogoutRedirectUris = { new Uri("https://localhost:4200") },
            Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        //OpenIddictConstants.Permissions.Endpoints.Introspection,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Revocation,


                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode, // enabling flows ("authorization" or "client creds") and refresh token
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                        OpenIddictConstants.Permissions.Prefixes.Scope + MyConstants.LibraryApiScope,
                        //OpenIddictConstants.Permissions.Scopes.Profile,

                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        });
    }

    public async Task AddScopes()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var apiScope = await manager.FindByNameAsync(MyConstants.LibraryApiScope);

        if (apiScope != null)
        {
            await manager.DeleteAsync(apiScope);
        }

        await manager.CreateAsync(new OpenIddictScopeDescriptor
        {
            DisplayName = "Api scope",
            Name = MyConstants.LibraryApiScope,
            Resources =
            {
               MyConstants.LibraryApiResource
            }
        });
    }

    public async Task AddAspNetIdentityRoles()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var RolesManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roles = new List<IdentityRole>();

        if (!await RolesManager.RoleExistsAsync(LibraryAppRoles.Admin))
        {
            roles.Add(new IdentityRole(roleName: LibraryAppRoles.Admin));
        }

        if (!await RolesManager.RoleExistsAsync(LibraryAppRoles.Manager))
        {
            roles.Add(new IdentityRole(roleName: LibraryAppRoles.Manager));
        }

        if (!await RolesManager.RoleExistsAsync(LibraryAppRoles.User))
        {
            roles.Add(new IdentityRole(roleName: LibraryAppRoles.User));
        }

        roles.ForEach(async x =>
        {
            await RolesManager.CreateAsync(x);
            await RolesManager.AddClaimAsync(x, new Claim(Claims.Role, x.Name!));
        });
    }

    public async Task AddUsers()
    {
        const string someStrongPassword = "Some_very_str0ng_Password!"; //TestAdmin1 Some_very_str0ng_Password!

        await using var scope = _serviceProvider.CreateAsyncScope();

        //var RolesManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var UsersManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var test1Admin = new ApplicationUser { UserName = "TestAdmin1" };
        await UsersManager.CreateAsync(test1Admin, someStrongPassword);
        await UsersManager.AddToRoleAsync(test1Admin, LibraryAppRoles.Admin);

        var test1Manager = new ApplicationUser { UserName = "TestManager1" };
        await UsersManager.CreateAsync(test1Manager, someStrongPassword);
        await UsersManager.AddToRoleAsync(test1Manager, LibraryAppRoles.Manager);

        var test1User = new ApplicationUser { UserName = "TestUser1" };
        await UsersManager.CreateAsync(test1User, someStrongPassword);
        await UsersManager.AddToRoleAsync(test1User, LibraryAppRoles.User);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
