using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Framework.Exceptions;

/// <summary>
/// میدلور مدیریت استثناهای سراسری
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "خطای کنترل نشده: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            StatusCode = context.Response.StatusCode,
            Message = "خطای داخلی سرور رخ داده است",
            Detail = exception.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

/// <summary>
/// اکستنشن برای ثبت میدلور مدیریت استثنا
/// </summary>
public static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) =>
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
