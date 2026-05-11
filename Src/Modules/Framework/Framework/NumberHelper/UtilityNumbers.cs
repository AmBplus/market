

using System;
using System.Security.Cryptography;

namespace Framework.NumberHelper
{
  public static class UtilityNumbers
  {

        public static int GenerateRandomNumber(int min, int max)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                int value = BitConverter.ToInt32(randomNumber, 0) % (max - min + 1);
                return Math.Abs(value) + min; // Ensure the number is within the specified range
            }
        }
        public static string ToEnglishNumber(this string number)
        {
            var EnglishNumber = number
         .Replace("۰", "0")
         .Replace("۱", "1")
         .Replace("۲", "2")
         .Replace("۳", "3")
         .Replace("۴", "4")
         .Replace("۵", "5")
         .Replace("۶", "6")
         .Replace("۷", "7")
         .Replace("۸", "8")
         .Replace("۹", "9")
         .Replace(",", ",");
            return EnglishNumber;
        }
        public static bool IsContainPersianNumber(this string text)
        {
            if (text
                .Contains("۰")
               || text.Contains("۱")
               || text.Contains("۲")
               || text.Contains("۳")
               || text.Contains("۴")
               || text.Contains("۵")
               || text.Contains("۶")
               || text.Contains("۷")
               || text.Contains("۸")
               || text.Contains("۹") 
                )
            {
                return true;
            }
            return false;
        }
        public static string  ToPersianNumber(this string number)
    {
      var persianNumber = number.Replace("0","۰")
               .Replace("1", "۱" )
               .Replace("2", "۲")
               .Replace("3", "۳")
               .Replace("4", "۴")
               .Replace("5", "۵")
               .Replace("6", "۶")
               .Replace("7", "۷")
               .Replace("8", "۸")
               .Replace("9", "۹")
               .Replace(",", ",");
      return persianNumber; 
    }
    public static long ConvertRialToToman(long rial)
    {
      if (rial == 0)
        return rial;
      return rial / 10;
    }
    public static long ConvertRialToToman(decimal rial)
    {
      if (rial == 0)
        return (long)rial;
      return (long)rial / 10;
    }
    public static long ConvertToRial(long rial)
    {
      if (rial == 0)
        return (long)rial;
      return rial / 10;
    }
    public static long ConvertToRial(decimal rial)
    {
      if (rial == 0)
        return (long)rial;
      return (long)rial / 10;
    }

    }
}
