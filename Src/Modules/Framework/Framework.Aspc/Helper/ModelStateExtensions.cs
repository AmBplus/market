using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Aspc.Helper;

public static class ModelStateExtensions
{
    // اکستنشن برای استخراج تمامی پیام‌های خطای ModelState و قرار دادن هر خطا در یک خط جدید
    public static string GetAllErrorMessages(this ModelStateDictionary modelState)
    {
        var errorMessages = modelState.Values
                                      .SelectMany(v => v.Errors)
                                      .Select(e => e.ErrorMessage)
                                       .Distinct()
                                      .ToList();

        // اگر خطاها موجود نباشد، پیغام پیش‌فرض را باز می‌گرداند
        return errorMessages.Any() ? string.Join("\n", errorMessages) : "ارسال مقادیر نا معتبر";
    }
}