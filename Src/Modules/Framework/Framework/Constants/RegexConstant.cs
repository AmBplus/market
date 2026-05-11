using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Constants
{
    public class RegexConstant
    {
        public class Number {
            public const string FiveDigit = "^[1-9][0-9]{4}";
        }
        public class PhoneNumber
        {
            public const string PersianPhoneNumber = "^(0|\\+98)?9[0-9]\\d{8}$";
        }
    }
}
