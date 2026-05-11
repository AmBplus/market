using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Settings.AppSettings
{
    public class ReportSettings
    {
        public  string? FontPath { get; set; }
        public  string? ReportPath { get; set; }  
        public string? ReportApiUrl { get; set; }
        public string? ReportValidateUrl { get; set; }

    }
}
