using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHost.Helper
{
    public static class ExcelValidationExtensions
    {
        public static bool IsRealExcel(this IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            try
            {
                using var stream = file.OpenReadStream();
                using var document = SpreadsheetDocument.Open(stream, false);

                // اگر Workbook یا Sheet نداشت => اکسل واقعی نیست
                return document.WorkbookPart?.Workbook?.Sheets?.Count() > 0;
            }
            catch
            {
                // هر Exception یعنی OpenXML نتوانسته باز کند
                return false;
            }
        }
    }
}
