using AppCore;
using AppCore.Data;
using AppCore.SeedData;
using Framework.DatatableModels;
using Framework.Exceptions;
using Framework.Security;
using Scalar.AspNetCore;
using Web.Helper;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Wolverine
builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(ECommerceDbContext).Assembly);
});

// AppCore Services
services.RegisterAppCoreServices(config.GetConnectionString("ECommerceDb")!);

// Framework Services
services.AddScoped<IDatatableResponseHelper, DatatableResponseHelper>();
services.AddScoped<UserInfo>();
services.AddHttpContextAccessor();

// Controllers + OpenAPI
services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly);
services.AddOpenApi();

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    await ECommerceSeedData.SeedAsync(db);
}

// Middleware
if (app.Environment.IsDevelopment())
{
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
app.MapControllers();
app.Run();
