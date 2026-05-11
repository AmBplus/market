using Framework.ResultHelper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceHost.ExceptionHandler;

public class GlobalExceptionHandlerMiddleware : object
{
    public GlobalExceptionHandlerMiddleware
        (RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger) : base()
    {
        Next = next;
        Logger = logger;
    }
    string errorHtml = @"
                                        <div>
                                            <section>
                                                <title>خطای سرور</title>
                                                <style>
                                                    .error-page {
                                                        font-family: Arial, sans-serif;
                                                        display: flex;
                                                        flex-direction: column;
                                                        align-items: center;
                                                        justify-content: center;
                                                        height: 100vh;
                                                        background-color: #f2f2f2;
                                                        margin: 0;
                                                    }
                                                    .error-page h1 {
                                                        color: #e74c3c;
                                                        font-size: 48px;
                                                        margin-bottom: 20px;
                                                    }
                                                    .error-page p {
                                                        color: #555;
                                                        font-size: 18px;
                                                        margin-bottom: 30px;
                                                    }
                                                    .error-page .back-button {
                                                        display: inline-block;
                                                        padding: 10px 20px;
                                                        font-size: 16px;
                                                        color: #fff;
                                                        background-color: #3498db;
                                                        border: none;
                                                        border-radius: 5px;
                                                        text-decoration: none;
                                                        cursor: pointer;
                                                        transition: background-color 0.3s ease;
                                                    }
                                                    .error-page .back-button:hover {
                                                        background-color: #2980b9;
                                                    }
                                                    /* اضافه کردن فونت بولد به بقیه تکست‌ها */
                                                    .error-page p, .error-page a {
                                                        font-weight: bold;
                                                    }
                                                </style>
                                            </section>
                                            <section class=""error-page"">
                                                <h1>خطای ۵۰۰</h1>
                                                <p>متاسفانه مشکلی در سرور رخ داده است. لطفاً بعداً دوباره تلاش کنید.</p>
                                                <a href=""/"" class=""back-button"">بازگشت به صفحه اصلی</a>
                                            </section>
                                        </div>



";
    private RequestDelegate Next { get; }
    public ILogger<GlobalExceptionHandlerMiddleware> Logger { get; }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }

        catch (Exception ex)
        {
            var path = httpContext.Request.Path.ToString();
            
            
            var errmsg = await GetRequestDetails(httpContext);
            
            // Log the error
            Logger.LogError(ex, $"{ex.Message} \n\n\n error msg :   {errmsg}");

            if (path.ToLower().StartsWith("/api/"))
            {
                // Handle API errors
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(ResultOperation.ToFailedResult());
            }
            else
            {
                // Handle HTML responses
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "text/html; charset=utf-8";
                await httpContext.Response.WriteAsync(errorHtml);

            }
        }
    }
    public async Task<string> GetRequestDetails(HttpContext httpContext)
    {
        var request = httpContext.Request;

        // جمع‌آوری اطلاعات مختلف ریکوئست
        try
        {
            var requestDetails = new List<string>
    {
        $"Method: {request.Method}",
        $"Path: {request.Path}",
        $"Query String: {request.QueryString}",
        $"Headers: {string.Join(", ", request.Headers.Select(h => $"{h.Key}: {h.Value}"))}"
    };

            // بررسی بدنه ریکوئست
            if (request.ContentType == "application/x-www-form-urlencoded")
            {
                // اگر بدنه به صورت فرم ارسال شده باشد
                request.EnableBuffering(); // برای خواندن چندباره بدنه
                var formData = await request.ReadFormAsync();
                foreach (var key in formData.Keys)
                {
                    requestDetails.Add($"Form Data - {key}: {formData[key]}");
                }
            }
            else if (request.ContentType == "application/json")
            {
                // اگر بدنه به صورت JSON ارسال شده باشد
                request.EnableBuffering(); // برای خواندن چندباره بدنه
                using (var reader = new StreamReader(request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    requestDetails.Add($"Body (JSON): {body}");
                }
            }
            else
            {
                // اگر بدنه به صورت دیگری ارسال شده باشد
                request.EnableBuffering(); // برای خواندن چندباره بدنه
                using (var reader = new StreamReader(request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    requestDetails.Add($"Body: {body}");
                }
            }

            // مرتب‌سازی و تبدیل به استرینگ
            return string.Join(Environment.NewLine, requestDetails);
        }
        catch (Exception e)
        {
            return $"unable to resolve httpcontext request : {e.Message}";
            
        }
    }

}
// Extension method used to add the middleware to the HTTP request pipeline.
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}