using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using ViewModels;
// مطمئن شوید namespace مربوط به ApplicationConstant ایمپورت شده باشد

namespace ServiceHost.ExceptionHandler.ServiceHost.Middlewares
{
    public class MaxSizeMiddleware
    {
        private readonly RequestDelegate _next;

        public MaxSizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // 1. بررسی سریع بر اساس هدر (مثل کد اول شما که درست کار می‌کرد)
            // این کار باعث می‌شود قبل از درگیر شدن Model Binding و خطای 400، ما جلوی درخواست را بگیریم
            if (context.Request.ContentLength.HasValue &&
                context.Request.ContentLength.Value > ApplicationConstant.ProjectFileSize.MaxAllowedSizeFileByProject)
            {
                await HandleSizeErrorAsync(context);
                return; // پایان درخواست، دیگر ادامه نده
            }

            try
            {
                await _next(context);
            }
            catch (BadHttpRequestException ex) when (ex.StatusCode == StatusCodes.Status413PayloadTooLarge)
            {
                // جهت اطمینان: اگر به هر دلیلی هدر درست نبود ولی موقع خواندن خطا داد
                await HandleSizeErrorAsync(context);
            }
            catch (Exception ex)
            {
                // مدیریت سایر خطاهای احتمالی حجم
                if (ex.Message.Contains("Request body too large", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleSizeErrorAsync(context);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task HandleSizeErrorAsync(HttpContext context)
        {
            // جلوگیری از ارسال هدرهای اضافی یا ریدارکت
            context.Response.Clear();

            // ست کردن کد وضعیت به 413 (بسیار مهم برای جاوااسکریپت)
            context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
            context.Response.ContentType = "application/json";

            // متن جیسون برای نمایش در SweetAlert
            var responseModel = new
            {
                isSuccess = false,
                message = $"حجم فایل ارسالی بیش از حد مجاز است. (حداکثر  مگابایت)",
                messageSingle = "فایل انتخاب شده بسیار حجیم است."
            };

            var jsonResponse = JsonSerializer.Serialize(responseModel);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public static class MaxSizeMiddlewareExtensions
    {
        public static IApplicationBuilder UseMaxSizeMiddlware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaxSizeMiddleware>();
        }
    }
}
