using IranKish.Interface;
using IranKish.Utility;
using IranKish;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var services = builder.Services;
    services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
services.IranKishServiceInjector( builder.Configuration);

    services.AddScoped<IIRanKishVerification, IRanKishVerification>();

    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // این خط برای فعال‌سازی کنترلرها کافی است
app.Run();
