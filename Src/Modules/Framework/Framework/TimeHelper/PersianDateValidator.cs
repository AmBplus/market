using System;
using System.Globalization;
using System.Text.RegularExpressions;
public static class PersianDateValidator
{
    public static bool IsValidPersianDate(this string dateString)
    {
        // بررسی فرمت کلی با regex
        if (!Regex.IsMatch(dateString, @"^\d{4}/\d{2}/\d{2}$"))
            return false;

        try
        {
            // جداسازی سال، ماه و روز
            var parts = dateString.Split('/');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            // بررسی محدوده معتبر
            if (year < 1300 || year > 1499) // محدوده سال‌های شمسی معقول
                return false;
            if (month < 1 || month > 12)
                return false;
            if (day < 1 || day > 31)
                return false;

            // بررسی تعداد روزهای ماه‌های شمسی
            if (month <= 6 && day > 31) // ماه‌های اول تا ششم 31 روز دارند
                return false;
            if (month >= 7 && month <= 11 && day > 30) // ماه‌های هفتم تا یازدهم 30 روز دارند
                return false;
            if (month == 12) // ماه آخر (اسفند)
            {
                bool isLeapYear = IsPersianLeapYear(year);
                if (day > (isLeapYear ? 30 : 29)) // اسفند در سال کبیسه 30 روز، غیرکبیسه 29 روز
                    return false;
            }

            // استفاده از PersianCalendar برای اعتبارسنجی نهایی
            PersianCalendar pc = new PersianCalendar();
            DateTime date = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static DateTime? ToDateTimeFromPersian(this string persianDateString)
    {

        if (string.IsNullOrEmpty(persianDateString))
            return null;
        // بررسی فرمت کلی با regex
        if (!Regex.IsMatch(persianDateString, @"^\d{4}/\d{2}/\d{2}$"))
            throw new InvalidTimeZoneException("تاریخ با صحیح نیست {yyyy/mm/dd}");
        try
        {
            // جداسازی سال، ماه و روز
            var parts = persianDateString.Split('/');
            int year = int.Parse(parts[0]);
            int month = int.Parse(parts[1]);
            int day = int.Parse(parts[2]);

            // بررسی محدوده معتبر
            if (year < 1300 || year > 1499) // محدوده سال‌های شمسی معقول
                throw new InvalidTimeZoneException("تاریخ با صحیح نیست {yyyy/mm/dd}");
            if (month < 1 || month > 12)
                throw new InvalidTimeZoneException("تاریخ با صحیح نیست {yyyy/mm/dd}");
            if (day < 1 || day > 31)
                throw new InvalidTimeZoneException("تاریخ با صحیح نیست {yyyy/mm/dd}");

            // بررسی تعداد روزهای ماه‌های شمسی
            if (month <= 6 && day > 31) // ماه‌های اول تا ششم 31 روز دارند
                throw new InvalidTimeZoneException("تاریخ با صحیح نیست {yyyy/mm/dd}");
            if (month >= 7 && month <= 11 && day > 30) // ماه‌های هفتم تا یازدهم 30 روز دارند
                throw new InvalidTimeZoneException("تاریخ با صحیح نیست {yyyy/mm/dd}");
            if (month == 12) // ماه آخر (اسفند)
            {
                bool isLeapYear = IsPersianLeapYear(year);
                if (day > (isLeapYear ? 30 : 29)) // اسفند در سال کبیسه 30 روز، غیرکبیسه 29 روز
                    throw new InvalidTimeZoneException("تاریخ با فرمت صحیحی نیست  کبیسه رعایت نشده {yyyy/mm/dd}");
            }

            // استفاده از PersianCalendar برای اعتبارسنجی نهایی
            PersianCalendar pc = new PersianCalendar();
            DateTime date = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            return date;
        }
        catch
        {
            throw new InvalidTimeZoneException("تاریخ با فرمت صحیحی نیست {yyyy/mm/dd}");
        }
    }
    

    // تابع کمکی برای بررسی سال کبیسه شمسی
    private static bool IsPersianLeapYear(int year)
    {
        PersianCalendar pc = new PersianCalendar();
        return pc.IsLeapYear(year);
    }
}