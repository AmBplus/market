using Framework.Aspc.Helper;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Services;

public class DatatableResponseHelper : IDatatableResponseHelper
{
    /// <summary>
    /// دریافت خروجی دیتاتیبل (Json یا Excel)
    /// </summary>

    public DatatableResponseHelper(IExcelHelper excelHelper)
    {
        ExcelHelper = excelHelper;
    }

    public IExcelHelper ExcelHelper { get; }

    public async Task<IActionResult> Get<T>(DatatableFullRequest datatableRequest, ResultOperation<DataTableResponse<T>> handlerResult,string name = null, CancellationToken cancellationToken = default) where T : class
    {
        if (!handlerResult.IsSuccess)
            return new BadRequestObjectResult(handlerResult);

        var isExcel =
            datatableRequest.ExportOption?.exportType
            == (int)DatatableExportType.Excel;

        // ===========================
        // EXCEL
        // ===========================
        if (isExcel)
        {
            var title = !string.IsNullOrWhiteSpace(name) ? name : "export";
            var fileName =
              $"{title}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var excelOptions = datatableRequest.ExportOption;
            var excelExportOption = new ExcelExportOptions
            {
                SheetName = title
            };
            var data = handlerResult.Data.Data!;
            var excel = await ExcelHelper.ClassToExcelAsync(
              items: data, options: excelExportOption, cancellationToken
              );

            if (!excel.IsSuccess || excel.Data == null)
            {
                return new BadRequestObjectResult(
                    excel.MessageSingle ?? "خطا در تولید فایل اکسل"
                );
            }
          

            return new FileStreamResult(
                excel.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            )
            {
                FileDownloadName = fileName
            };
        }

        // ===========================
        // JSON (DataTables)
        // ===========================
        return new OkObjectResult(handlerResult.Data);
    }
}