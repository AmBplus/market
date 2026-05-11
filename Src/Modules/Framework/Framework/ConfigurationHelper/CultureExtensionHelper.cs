using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.ConfigurationHelper
{
    public static class CultureExtensionHelper
    {
        private static CultureInfo _culturePersian;
        private static CultureInfo _cultureArabic;
        private static CultureInfo _cultureEn;

        public const string Fa = "fa";
        public const string En = "en-US";
        public const string Ar = "ar";
        

        public static bool IsPersian()
        {
            return IsThisCulture(Fa);
        }
        public static bool IsArabic()
        {
            return IsThisCulture(Ar);
        }
        public static bool IsEnglish()
        {
            return IsThisCulture(En);
        }
        private static bool IsThisCulture(string cultureName)
        {
            if (CultureInfo.CurrentCulture.Name == Fa)
            {
                return true;
            }
            return false;
        }
        
        public static CultureInfo GetPersianCulture()
        {
            const string cultureName = "fa";

            if (_culturePersian == null)
            {
                _culturePersian = new CultureInfo(cultureName);

                DateTimeFormatInfo formatInfo = _culturePersian.DateTimeFormat;

                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                "اسفند",
                ""
            };
                formatInfo.AbbreviatedMonthNames = formatInfo.MonthNames = formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;

                _culturePersian.DateTimeFormat = new CultureInfo("en-US").DateTimeFormat;

                Calendar persianCalendar = new PersianCalendar();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FieldInfo fieldInfo = _culturePersian.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (fieldInfo != null)
                    fieldInfo.SetValue(_culturePersian, persianCalendar);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (info != null)
                    info.SetValue(formatInfo, persianCalendar);

                _culturePersian.NumberFormat.NumberDecimalSeparator = "/";
                _culturePersian.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _culturePersian.NumberFormat.NumberNegativePattern = 0;
            }
            return _culturePersian;
        }

        public static CultureInfo GetArabicCulture()
        {
            const string cultureName = "ar";

            if (_cultureArabic == null)
            {
                _cultureArabic = new CultureInfo(cultureName);

                DateTimeFormatInfo formatInfo = _cultureArabic.DateTimeFormat;

                formatInfo.AbbreviatedDayNames = new[] { "أحد", "اثنين", "ثلاثاء", "أربعاء", "خميس", "جمعة", "سبت" };
                formatInfo.DayNames = new[] { "الأحد", "الاثنين", "الثلاثاء", "الأربعاء", "الخميس", "الجمعة", "السبت" };
                var monthNames = new[]
                {
            "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر", ""
        };
                formatInfo.AbbreviatedMonthNames = formatInfo.MonthNames = formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;

                _cultureArabic.DateTimeFormat = new CultureInfo("en-US").DateTimeFormat;

                Calendar gregorianCalendar = new GregorianCalendar();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FieldInfo fieldInfo = _cultureArabic.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (fieldInfo != null)
                    fieldInfo.SetValue(_cultureArabic, gregorianCalendar);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (info != null)
                    info.SetValue(formatInfo, gregorianCalendar);

                _cultureArabic.NumberFormat.NumberDecimalSeparator = ".";
                _cultureArabic.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _cultureArabic.NumberFormat.NumberNegativePattern = 0;
            }
            return _cultureArabic;
        }

        public static CultureInfo GetEnCulture()
        {
            if (_cultureEn == null)
            {
                 _cultureEn =
                    new CultureInfo(name: "en-US");
            }
            return _cultureEn;
        }

        public static void SetCulture(this CultureInfo cultureInfo)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
