using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using Shared;
using System.Reflection;
using task2.BLL;
using task2.Context;
using task2.Controllers.Middlewares;
using webapi.Context;

var builder = WebApplication.CreateBuilder(args);

// Register the OpenIddict validation components.
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        // Note: the validation handler uses OpenID Connect discovery
        // to retrieve the address of the introspection endpoint.
        options.SetIssuer("https://localhost:7040/");
        options.AddAudiences(MyConstants.LibraryApiResource);

        // Configure the validation handler to use introspection and register the client
        // credentials used when communicating with the remote introspection endpoint.
        options.UseIntrospection()
               .SetClientId(MyConstants.LibraryApiResource)
               .SetClientSecret("CF8B4485-AA73-46EC-8D68-CE8059DE4E13");
        //options.AddEncryptionKey(new SymmetricSecurityKey(
        //Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

builder.Services.AddScoped<BooksRepository>();
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("Default")!);
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IValidatorInterceptor, UseCustomErrorModelInterceptor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(conf =>
{
    conf.AllowAnyHeader();
    conf.AllowAnyMethod();
    conf.AllowAnyOrigin();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorMiddleware>();

app.MapControllers();

using var scope = app.Services.CreateScope();
var appContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

Initializer.InitDatabase(appContext);
Initializer.Seed(appContext);

app.Run();
