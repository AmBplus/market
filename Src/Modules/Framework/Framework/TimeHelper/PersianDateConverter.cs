using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Framework;
public static class PersianDateConverter
{
    public static string ToPersianDateString(this DateTime gregorianDateTime)
    {
        try
        {
            // ایجاد نمونه از PersianCalendar
            PersianCalendar persianCalendar = new PersianCalendar();

            // استخراج سال، ماه و روز شمسی از تاریخ میلادی
            int year = persianCalendar.GetYear(gregorianDateTime);
            int month = persianCalendar.GetMonth(gregorianDateTime);
            int day = persianCalendar.GetDayOfMonth(gregorianDateTime);

            // قالب‌بندی به صورت YYYY/MM/DD
            return $"{year:0000}/{month:00}/{day:00}";
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"خطا در تبدیل تاریخ میلادی به شمسی: {ex.Message}", ex);
        }
    }
    public static string ToPersianDateString(this DateOnly gregorianDate)
    {
        try
        {
            // ایجاد نمونه از PersianCalendar
            PersianCalendar persianCalendar = new PersianCalendar();

            // استخراج سال، ماه و روز شمسی از تاریخ میلادی
            int year = persianCalendar.GetYear(gregorianDate.ToDateTime(TimeOnly.MinValue));
            int month = persianCalendar.GetMonth(gregorianDate.ToDateTime(TimeOnly.MinValue));
            int day = persianCalendar.GetDayOfMonth(gregorianDate.ToDateTime(TimeOnly.MinValue));

            // قالب‌بندی به صورت YYYY/MM/DD
            return $"{year:0000}/{month:00}/{day:00}";
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"خطا در تبدیل تاریخ میلادی به شمسی: {ex.Message}", ex);
        }
    }

    public static DateOnly ToDateOnlyFromPersian(this string persianDateString)
    {

        if (!persianDateString.IsValidPersianDate())
            throw new InvalidTimeZoneException("داده نامعتبر برای تاریخ ارسال شده است");
        try
        {
            // جداسازی سال، ماه و روز
            var parts = persianDateString.Split('/');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            // تبدیل به DateTime با استفاده از PersianCalendar
            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime gregorianDate = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);

            // تبدیل به DateOnly
            return DateOnly.FromDateTime(gregorianDate);
        }
        
        catch (Exception ex)
        {
            throw new ArgumentException($"تاریخ شمسی وارد شده نامعتبر است: {persianDateString}. جزئیات: {ex.Message}", ex);
        }
    }
}
