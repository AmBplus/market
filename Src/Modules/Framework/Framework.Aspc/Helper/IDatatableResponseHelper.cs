using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
namespace Framework.Aspc.Helper;
public interface IDatatableResponseHelper
{
    /// <summary>
    /// دریافت خروجی دیتاتیبل (Json یا Excel)
    /// </summary>
    Task<IActionResult> Get<T>(
        DatatableFullRequest datatableRequest,
        ResultOperation<DataTableResponse<T>> handlerResult, string name = null,
       CancellationToken cancellationToken = default
    ) where T : class;
}