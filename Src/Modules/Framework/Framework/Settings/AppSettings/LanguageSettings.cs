using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Settings.AppSettings;

using System;
using System.Globalization;

public class LanguageSettings
{
    public LanguageSettings()
    {
        var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        IsPersian = currentCulture.Equals(Persian, StringComparison.OrdinalIgnoreCase);
        IsEnglish = currentCulture.Equals(English, StringComparison.OrdinalIgnoreCase);
        IsArabic = currentCulture.Equals(Arabic, StringComparison.OrdinalIgnoreCase);
        LanguageId = IsPersian ? 1 : IsEnglish ? 2 : IsArabic ? 3 : 1 ;
        LanguageName = IsPersian ? "Persian" : IsEnglish ? "English" : IsArabic ? "Arabic" : "Persian" ;
        LanguageNameCulture = IsPersian ? "fa" : IsEnglish ? "en" : IsArabic ? "ar" : "fa" ;
    }

    public bool IsPersian { get; }
    public bool IsEnglish { get; }
    public bool IsArabic { get; }
    public int LanguageId { get; }
    public string LanguageName { get; }
    public string LanguageNameCulture { get; }

    public const string Persian = "fa";
    public const string English = "en";
    public const string Arabic = "ar";
}
