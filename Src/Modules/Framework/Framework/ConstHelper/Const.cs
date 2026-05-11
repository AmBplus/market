using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ConstHelper
{
    public class SqlDbConnectionNameConst
    {
        public const string ConnectionName = "SqlServerDatabase";
    }
    public class TimerSecoundConst
    {
        public const long Hour = 3600;
        public const long Day = Hour * 24;
        public const long Week = Day * 7;
        public const long Month = Week * 4;

    }
}
