using AppCore;
using AppCore.Data;
using Feature.Client.Common;
using Framework.Aspc.Security;
using Framework.ConfigurationHelper;
using Framework.Helpers;
using Framework.Services.Orm;
using Framework.Settings.AppSettings;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using ServiceHost.ExceptionHandler;
using ServiceHost.ServiceInjector;
using Services.CachingProvider;
using SixLabors.ImageSharp;
using Web.Helper;
using Web.Pages.AdminPanel.ID.Users;
using Wolverine;
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(AppDbContext).Assembly);
});

MappingConfigLoader.Load(typeof(UserUpsertModel).Assembly); 
// Program.cs
services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly);
// Add services to the container.
services.AddRazorPages();
services.AddHttpContextAccessor();
services.AddScoped<UserInfo>();
// Authentication (Cookie)
services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.SlidingExpiration = true;
    });
services.AddRazorPages(options =>
{
    options.Conventions.Add(new CustomRouteConvention());
});
services.ServiceHostInjector(config, builder.Environment);
services.AddEndpointsApiExplorer();
services.AddOpenApi();
services.RegisterCommonServices(config);
services.RegisterAppCoreServics(config);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseGlobalExceptionHandler();
}
app.MapOpenApi();
app.MapScalarApiReference();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.Run();

