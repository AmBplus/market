using PersianTools.Core;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Framework.TimeHelper
{
    public static class TimeUtilHelper
    {
        public static DateTime NowDateTime(bool useCultrure = false)
        {
            if(useCultrure == true) return DateTime.Now;

                var currentCulture =
                    System.Threading.Thread.CurrentThread.CurrentCulture;

                var currentUICulture =
                    System.Threading.Thread.CurrentThread.CurrentUICulture;

                var englishCulture =
                    new System.Globalization.CultureInfo(name: "en-US");

                System.Threading.Thread.CurrentThread.CurrentCulture = englishCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = englishCulture;

                var result =
                    System.DateTime.Now;

                //var result =
                //	System.DateTime.Now.AddMinutes(value: 210);

                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = currentUICulture;

                return result;
        }
        public static DateTime ToDateTime(this DateOnly dateOnly)
        {
            return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
        }
        public static string GetPersianDate()
        {
            var persianDateTime = new PersianDateTime(NowDateTime());
            return persianDateTime.ToString();
        }

        public static string GetPersianDate(DateOnly? dateTime = null)
        {
            if (dateTime == null)
                return "";

            var currentCulture =
                   System.Threading.Thread.CurrentThread.CurrentCulture;

            var currentUICulture =
                System.Threading.Thread.CurrentThread.CurrentUICulture;

            var englishCulture =
                new System.Globalization.CultureInfo(name: "en-US");

            System.Threading.Thread.CurrentThread.CurrentCulture = englishCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = englishCulture;

            // Convert the DateOnly to a DateTime in UTC
            var gregorianDate = ((DateOnly)dateTime).ToDateTime(TimeOnly.MinValue);

            string persianDateString = gregorianDate.ToString("yyyy/MM/dd", new CultureInfo("fa-IR"));

            //var result =
            //	System.DateTime.Now.AddMinutes(value: 210);

            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = currentUICulture;

            return persianDateString;
        }
        public static string GetPersianDate(this DateOnly dateTime)
        {

            PersianCalendar pc = new PersianCalendar();
            DateTime thisDate = DateTime.Now;


            var Year = pc.GetYear(thisDate);
            var Month = pc.GetMonth(thisDate);
            var DayOfMonth = pc.GetDayOfMonth(thisDate);

            return $"{Year}/{Month}/{DayOfMonth}";
        }
        public static string GetPersianDate(this DateTime dateTime)
        {
            var persianDateTime = new PersianDateTime(dateTime);
            return persianDateTime.ToString();
        } 
        public static string GetPersianDate(this DateTime? dateTime)
        {
            var persianDateTime = new PersianDateTime();
            return persianDateTime.ToString();
        }

        public static string GetDateFromPersianDate(this string persianDate)
        {
             var persianDateTime = new PersianDateTime(persianDate);
            string dateTimeString = persianDate;
            string formattedDateTime = persianDateTime.DateTime.ToString("yyyy-MM-dd");

            return formattedDateTime;

        }
        
        public static bool IsPersianValid(this string persianDate)
        {
            
            string pattern = @"^[0-9][0-9][0-9]{2}\/((0[1-6]\/(0[1-9]|[1-2][0-9]|3[0-1]))|(0[7-9]\/(0[1-9]|[1-2][0-9]|30))|(1[0-1]\/(0[1-9]|[1-2][0-9]|30))|(12\/(0[1-9]|[1-2][0-9])))";
            // Check if the string matches the pattern
            bool isValid = Regex.IsMatch(persianDate, pattern);

            return isValid;
        }

    }
}
