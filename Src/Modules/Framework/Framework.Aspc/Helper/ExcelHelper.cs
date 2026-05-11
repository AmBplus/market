using Framework.ResultHelper;
using Microsoft.AspNetCore.Http;
namespace Framework.Aspc.Helper
{// کلاس گزینه‌ها - فعلاً خیلی ساده
    public class ExcelExportOptions
    {
        public string SheetName { get; set; } = "Sheet1";
    }
    public interface IExcelHelper
    {
        Task<ResultOperation<List<T>>> ExcelToClassAsync<T>(
               IFormFile file,
               Dictionary<int, string> columnIndexMappings,
               CancellationToken cancellationToken = default
           ) where T : class, new();

        // Export - ساده با گزینه اختیاری
        Task<ResultOperation<MemoryStream>> ClassToExcelAsync<T>(
            IEnumerable<T>? items,
            ExcelExportOptions? options = null,
            CancellationToken cancellationToken = default)
            where T : class;
    }

}
