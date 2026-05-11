using Framework.NumberHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Framework.TextHelper
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public static class TextFixer
    {
        public static string NameOf(this string str)
        {
            return nameof(str);
        }
        public static string Slugify(this string phrase)
        {
            var s = phrase.RemoveDiacritics().ToLower();
            s = Regex.Replace(s, @"[^\u0600-\u06FF\uFB8A\u067E\u0686\u06AF\u200C\u200Fa-z0-9\s-]",
                ""); // remove invalid characters
            s = Regex.Replace(s, @"\s+", " ").Trim(); // single space
            s = s.Substring(0, s.Length <= 100 ? s.Length : 45).Trim(); // cut and trim
            s = Regex.Replace(s, @"\s", "-"); // insert hyphens        
            s = Regex.Replace(s, @"‌", "-"); // half space
            return s.ToLower();
        }
        public static bool ContainsPersianLetters(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // محدوده یونیکد حروف فارسی: از 0x0600 تا 0x06FF
            foreach (char c in input)
            {
                if (c >= 0x0600 && c <= 0x06FF)
                    return true;
            }
            return false;
        }
   
        public static (string errorMessage ,bool result) IsValidEnglishText(this string text,string title)
        {
            if (text.ContainsPersianLetters())
            {
                return ($"استفاده از کلمات فارسی در {title} امکان پذیر نمی باشد",false);
            }
            
            if (text.ContainsSqlInjectionPatterns())
            {
                return ( $"از کلمات غیر مجاز استفاده شده ، از کلمات دیگری برای {title} استفاده کنید", false);
            }
          
            return ( "" ,true);
        }
        public static bool ContainsSqlInjectionPatterns(this string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // لیست کلمات و الگوهای خطرناک مرتبط با SQL Injection
            string[] sqlInjectionKeywords = new string[]
            {
        "SELECT", "INSERT", "UPDATE", "DELETE", "DROP", "TRUNCATE", "EXEC", "UNION", "WHERE",
        "FROM", "JOIN", "AND", "OR", "' OR '", "1=1", "--", ";--", "/*", "*/", "xp_", "sp_" ," "
            };

            // تبدیل رمز عبور به حروف کوچک برای مقایسه بدون حساسیت به بزرگی/کوچکی
            string lowerPassword = password.ToLower();

            // بررسی وجود هر یک از الگوها در رمز عبور
            foreach (string keyword in sqlInjectionKeywords)
            {
                if (lowerPassword.Contains(keyword.ToLower()))
                {
                    return true; // اگر الگوی خطرناک پیدا شد، true برگردان
                }
            }

            return false; // اگر هیچ الگوی خطرناکی پیدا نشد، false برگردان
        }
        public static string FixText(this string phrase)
        {
            var s = phrase.RemoveDiacritics().ToLower();

            s = Regex.Replace(s, @"\s+", " ").Trim(); // single space

            return s;
        }
        public static string GetDirection(this string dir)
        {
            var s = dir.RemoveDiacritics().ToLower();

            if (s == "desc")
                dir= "desc";
            else
                dir= "asc";

            return dir;
        }
        /// <summary>
        /// Remove Dangerous Sql Character
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public static string RemoveSqlInjection(this string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return query;
            string[] anyOf = { "'", ";", "%", "*", "--", "/*", "*/", "exec", "table_name", "information_schema.tables", "master", "create", "drop", "alter", "sysobjects", "syscolumns" };

            foreach (string Str in anyOf)
            {
                query = query.ToLower().Replace(Str, "");
            }

            return query;
        }
        /// <summary>
        /// for lighter version of remove injection , use case in condation 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string RemoveInnerSqlInjection(this string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return query;
            string[] blackList = {  ";", "--", "/*", "*/", "exec", "create", "drop", "alter", "master", "information_schema.tables", "sysobjects", "syscolumns" };

            foreach (string item in blackList)
            {
                query = query.Replace(item, "", StringComparison.OrdinalIgnoreCase);
            }

            return query;
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var normalizedString = text.Normalize(NormalizationForm.FormKC);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
    public static class TextUtilHelper
    {

        public static string FixText(this string text)
        {
            var stringBuilder = new StringBuilder(text.Trim());

            // Fix Double Space 
            stringBuilder.Replace("  ", " ");
            
            return stringBuilder.ToString();    

        }
        private const long zero_code_farsi = 0x660; // = 1632  = '0' Farsi
        private const long zero_code_farsi2 = 0x6F0; // = 1776  = '0' mobile Farsi
        private const long zero_code_latin = 0x30;  // = 48    = '0' Latin

        public static string Convert_Farsi_Numeric_to_Latin(this string mystr)
        {
            string s2;
            int i;
            long MyValue, maxlen;

            maxlen = mystr.Length - 1;
            s2 = "";

            var loopTo = maxlen;
            for (i = 0; i <= loopTo; i++)
            {
                MyValue = (long)(mystr[i]);
                if (MyValue >= zero_code_farsi & MyValue <= zero_code_farsi + 9)
                {
                    s2 = s2 + (char)(MyValue - zero_code_farsi + zero_code_latin);
                }
                else if (MyValue >= zero_code_farsi2 & MyValue <= zero_code_farsi2 + 9)
                {
                    s2 = s2 + (char)(MyValue - zero_code_farsi2 + zero_code_latin);
                }
                else
                {
                    s2 = s2 + (char)(MyValue);
                }
            }

            return s2;
        }

        public static string FixFarsiStr(this string s1)
        {
            string s2 = "";
            int i, j;
            char ch;

            /*
                --1740 unicode(N'ي')  --in apple
                --1610 unicode(N'ي')
                --1609 unicode(N'ى')
            */
            /*
             *                 Case "أ" : s2 = s2 + "ا"
                Case "إ" : s2 = s2 + "ا"
                Case "ک" : s2 = s2 + "ك"
                Case "ؤ" : s2 = s2 + "و"
                Case "ة" : s2 = s2 + "ه"
                Case "ي" : s2 = s2 + "ي"
                    'Case "ئ" : s2 = s2 + "ي"
                    'Case "ئ" : s2 = s2 + "ي"
                Case "ى" : s2 = s2 + "ي"
             */

            if (string.IsNullOrEmpty(s1)) return "";

            j = s1.Length;
            s2 = "";
            for (i = 0; i < j; i++)
            {
                ch = (char)s1[i];
                switch (ch)
                {
                
                    case 'أ':
                        s2 = s2 + "ا";
                        break;
                    case 'إ':
                        s2 = s2 + "ا";
                        break;
                    case 'ة':
                        s2 = s2 + "ه";
                        break;
                    case 'ؠ':
                        s2 = s2 + "ی";
                        break;
                    case 'ؽ':
                        s2 = s2 + "ی";
                        break;
                    case 'ؾ':
                        s2 = s2 + "ی";
                        break;
                    case 'ؿ':
                        s2 = s2 + "ی";
                        break;
                    case 'ٸ':
                        s2 = s2 + "ی";
                        break;
                    case 'ی':
                        s2 = s2 + "ی";
                        break;
                    case 'ۍ':
                        s2 = s2 + "ی";
                        break;
                    case 'ێ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﯼ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﯽ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﻯ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﻰ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﻱ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﻲ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﻳ':
                        s2 = s2 + "ی";
                        break;
                    case 'ﻴ':
                        s2 = s2 + "ی";
                        break;   
                    case 'ي':
                        s2 = s2 + "ی";
                        break;
                    case 'ک':
                        s2 = s2 + "ک";
                        break;
                    case 'ك':
                        s2 = s2 + "ک";
                        break;
                    case 'ؼ':
                        s2 = s2 + "ک";
                        break;
                    case 'ڬ':
                        s2 = s2 + "ک";
                        break;
                    case 'ڭ':
                        s2 = s2 + "ک";
                        break;
                    case 'ڮ':
                        s2 = s2 + "ک";
                        break;
                    case 'ݢ':
                        s2 = s2 + "ک";
                        break;
                    case 'ݣ':
                        s2 = s2 + "ک";
                        break;
                    case 'ݤ':
                        s2 = s2 + "ک";
                        break;
                    case 'ﮎ':
                        s2 = s2 + "ک";
                        break;
                    case 'ﮏ':
                        s2 = s2 + "ک";
                        break;
                    case 'ﮑ':
                        s2 = s2 + "ک";
                        break;



                    case 'و':
                        s2 = s2 + "و";
                        break;
                    case 'ؤ':
                        s2 = s2 + "و";
                        break;
                    case 'ٶ':
                        s2 = s2 + "و";
                        break;
                    case 'ٷ':
                        s2 = s2 + "و";
                        break;
                    case 'ۄ':
                        s2 = s2 + "و";
                        break;
                    case 'ۅ':
                        s2 = s2 + "و";
                        break;
                    case 'ۆ':
                        s2 = s2 + "و";
                        break;
                    case 'ۇ':
                        s2 = s2 + "و";
                        break;
                    case 'ۈ':
                        s2 = s2 + "و";
                        break;
                    case 'ۉ':
                        s2 = s2 + "و";
                        break;
                    case 'ۊ':
                        s2 = s2 + "و";
                        break;
                    case 'ۋ':
                        s2 = s2 + "و";
                        break;
                    case 'ݸ':
                        s2 = s2 + "و";
                        break;
                    case 'ݹ':
                        s2 = s2 + "و";
                        break;


                    default:
                        if ((int)ch == 1740 || (int)ch == 1609)
                            s2 = s2 + "ی";
                        else if ((int)ch == 1705)
                            s2 = s2 + "ک";
                        else
                            s2 = s2 + ch;
                        break;
                }
            }

            return s2;
        }


        public static string GenerateFileName( )
        {
            var newFileName = (Guid.NewGuid().ToString("D").Split('-'))[0] + GetCode(100, 999);
            return newFileName;
        }
        public static string GenerateFileName(string additionName)
        {
            var newFileName = additionName + "_" + (Guid.NewGuid().ToString("D").Split('-'))[0] + GetCode(100, 999);
            return newFileName;
        }
        public static string GetCode(int min, int max)
        {
            //Get 5 Digit Number
            return System.Security.Cryptography.RandomNumberGenerator.GetInt32(min, max).ToString();
        }

        public static string GetRawContentFromHtml(this string htmlString)
        {
            HtmlToText convert = new HtmlToText();
            htmlString = convert.Convert(htmlString);
            return htmlString;
        }


  
    }


    
}
class HtmlToText
{
    // Static data tables
    protected static Dictionary<string, string> _tags;
    protected static HashSet<string> _ignoreTags;

    // Instance variables
    protected TextBuilder _text;
    protected string _html;
    protected int _pos;

    // Static constructor (one time only)
    static HtmlToText()
    {
        _tags = new Dictionary<string, string>();
        _tags.Add("address", "\n");
        _tags.Add("blockquote", "\n");
        _tags.Add("div", "\n");
        _tags.Add("dl", "\n");
        _tags.Add("fieldset", "\n");
        _tags.Add("form", "\n");
        _tags.Add("h1", "\n");
        _tags.Add("/h1", "\n");
        _tags.Add("h2", "\n");
        _tags.Add("/h2", "\n");
        _tags.Add("h3", "\n");
        _tags.Add("/h3", "\n");
        _tags.Add("h4", "\n");
        _tags.Add("/h4", "\n");
        _tags.Add("h5", "\n");
        _tags.Add("/h5", "\n");
        _tags.Add("h6", "\n");
        _tags.Add("/h6", "\n");
        _tags.Add("p", "\n");
        _tags.Add("/p", "\n");
        _tags.Add("table", "\n");
        _tags.Add("/table", "\n");
        _tags.Add("ul", "\n");
        _tags.Add("/ul", "\n");
        _tags.Add("ol", "\n");
        _tags.Add("/ol", "\n");
        _tags.Add("/li", "\n");
        _tags.Add("br", "\n");
        _tags.Add("/td", "\t");
        _tags.Add("/tr", "\n");
        _tags.Add("/pre", "\n");

        _ignoreTags = new HashSet<string>();
        _ignoreTags.Add("script");
        _ignoreTags.Add("noscript");
        _ignoreTags.Add("style");
        _ignoreTags.Add("object");
    }

    /// <summary>
    /// Converts the given HTML to plain text and returns the result.
    /// </summary>
    /// <param name="html">HTML to be converted</param>
    /// <returns>Resulting plain text</returns>
    public string Convert(string html)
    {
        // Initialize state variables
        _text = new TextBuilder();
        _html = html;
        _pos = 0;

        // Process input
        while (!EndOfText)
        {
            if (Peek() == '<')
            {
                // HTML tag
                bool selfClosing;
                string tag = ParseTag(out selfClosing);

                // Handle special tag cases
                if (tag == "body")
                {
                    // Discard content before <body>
                    _text.Clear();
                }
                else if (tag == "/body")
                {
                    // Discard content after </body>
                    _pos = _html.Length;
                }
                else if (tag == "pre")
                {
                    // Enter preformatted mode
                    _text.Preformatted = true;
                    EatWhitespaceToNextLine();
                }
                else if (tag == "/pre")
                {
                    // Exit preformatted mode
                    _text.Preformatted = false;
                }

                string value;
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                if (_tags.TryGetValue(tag, out value))
                    _text.Write(value);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                if (_ignoreTags.Contains(tag))
                    EatInnerContent(tag);
            }
            else if (Char.IsWhiteSpace(Peek()))
            {
                // Whitespace (treat all as space)
                _text.Write(_text.Preformatted ? Peek() : ' ');
                MoveAhead();
            }
            else
            {
                // Other text
                _text.Write(Peek());
                MoveAhead();
            }
        }
        // Return result
        return HttpUtility.HtmlDecode(_text.ToString());
    }

    // Eats all characters that are part of the current tag
    // and returns information about that tag
    protected string ParseTag(out bool selfClosing)
    {
        string tag = String.Empty;
        selfClosing = false;

        if (Peek() == '<')
        {
            MoveAhead();

            // Parse tag name
            EatWhitespace();
            int start = _pos;
            if (Peek() == '/')
                MoveAhead();
            while (!EndOfText && !Char.IsWhiteSpace(Peek()) &&
                Peek() != '/' && Peek() != '>')
                MoveAhead();
            tag = _html.Substring(start, _pos - start).ToLower();

            // Parse rest of tag
            while (!EndOfText && Peek() != '>')
            {
                if (Peek() == '"' || Peek() == '\'')
                    EatQuotedValue();
                else
                {
                    if (Peek() == '/')
                        selfClosing = true;
                    MoveAhead();
                }
            }
            MoveAhead();
        }
        return tag;
    }

    // Consumes inner content from the current tag
    protected void EatInnerContent(string tag)
    {
        string endTag = "/" + tag;

        while (!EndOfText)
        {
            if (Peek() == '<')
            {
                // Consume a tag
                bool selfClosing;
                if (ParseTag(out selfClosing) == endTag)
                    return;
                // Use recursion to consume nested tags
                if (!selfClosing && !tag.StartsWith("/"))
                    EatInnerContent(tag);
            }
            else MoveAhead();
        }
    }

    // Returns true if the current position is at the end of
    // the string
    protected bool EndOfText
    {
        get { return (_pos >= _html.Length); }
    }

    // Safely returns the character at the current position
    protected char Peek()
    {
        return (_pos < _html.Length) ? _html[_pos] : (char)0;
    }

    // Safely advances to current position to the next character
    protected void MoveAhead()
    {
        _pos = Math.Min(_pos + 1, _html.Length);
    }

    // Moves the current position to the next non-whitespace
    // character.
    protected void EatWhitespace()
    {
        while (Char.IsWhiteSpace(Peek()))
            MoveAhead();
    }

    // Moves the current position to the next non-whitespace
    // character or the start of the next line, whichever
    // comes first
    protected void EatWhitespaceToNextLine()
    {
        while (Char.IsWhiteSpace(Peek()))
        {
            char c = Peek();
            MoveAhead();
            if (c == '\n')
                break;
        }
    }

    // Moves the current position past a quoted value
    protected void EatQuotedValue()
    {
        char c = Peek();
        if (c == '"' || c == '\'')
        {
            // Opening quote
            MoveAhead();
            // Find end of value
            int start = _pos;
            _pos = _html.IndexOfAny(new char[] { c, '\r', '\n' }, _pos);
            if (_pos < 0)
                _pos = _html.Length;
            else
                MoveAhead();    // Closing quote
        }
    }

    /// <summary>
    /// A StringBuilder class that helps eliminate excess whitespace.
    /// </summary>
    protected class TextBuilder
    {
        private StringBuilder _text;
        private StringBuilder _currLine;
        private int _emptyLines;
        private bool _preformatted;

        // Construction
        public TextBuilder()
        {
            _text = new StringBuilder();
            _currLine = new StringBuilder();
            _emptyLines = 0;
            _preformatted = false;
        }

        /// <summary>
        /// Normally, extra whitespace characters are discarded.
        /// If this property is set to true, they are passed
        /// through unchanged.
        /// </summary>
        public bool Preformatted
        {
            get
            {
                return _preformatted;
            }
            set
            {
                if (value)
                {
                    // Clear line buffer if changing to
                    // preformatted mode
                    if (_currLine.Length > 0)
                        FlushCurrLine();
                    _emptyLines = 0;
                }
                _preformatted = value;
            }
        }

        /// <summary>
        /// Clears all current text.
        /// </summary>
        public void Clear()
        {
            _text.Length = 0;
            _currLine.Length = 0;
            _emptyLines = 0;
        }

        /// <summary>
        /// Writes the given string to the output buffer.
        /// </summary>
        /// <param name="s"></param>
        public void Write(string s)
        {
            foreach (char c in s)
                Write(c);
        }

        /// <summary>
        /// Writes the given character to the output buffer.
        /// </summary>
        /// <param name="c">Character to write</param>
        public void Write(char c)
        {
            if (_preformatted)
            {
                // Write preformatted character
                _text.Append(c);
            }
            else
            {
                if (c == '\r')
                {
                    // Ignore carriage returns. We'll process
                    // '\n' if it comes next
                }
                else if (c == '\n')
                {
                    // Flush current line
                    FlushCurrLine();
                }
                else if (Char.IsWhiteSpace(c))
                {
                    // Write single space character
                    int len = _currLine.Length;
                    if (len == 0 || !Char.IsWhiteSpace(_currLine[len - 1]))
                        _currLine.Append(' ');
                }
                else
                {
                    // Add character to current line
                    _currLine.Append(c);
                }
            }
        }

        // Appends the current line to output buffer
        protected void FlushCurrLine()
        {
            // Get current line
            string line = _currLine.ToString().Trim();

            // Determine if line contains non-space characters
            string tmp = line.Replace("&nbsp;", String.Empty);
            if (tmp.Length == 0)
            {
                // An empty line
                _emptyLines++;
                if (_emptyLines < 2 && _text.Length > 0)
                    _text.AppendLine(line);
            }
            else
            {
                // A non-empty line
                _emptyLines = 0;
                _text.AppendLine(line);
            }

            // Reset current line
            _currLine.Length = 0;
        }

        /// <summary>
        /// Returns the current output as a string.
        /// </summary>
        public override string ToString()
        {
            if (_currLine.Length > 0)
                FlushCurrLine();
            return _text.ToString();
        }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
