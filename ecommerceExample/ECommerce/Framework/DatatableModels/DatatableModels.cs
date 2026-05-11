using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Framework.DatatableModels;

/// <summary>
/// درخواست استاندارد DataTable از سمت کلاینت
/// </summary>
public class DatatableFullRequest
{
    public int draw { get; set; }
    public Column[] columns { get; set; } = [];
    public Order[] order { get; set; } = [];
    public int start { get; set; }
    public int length { get; set; }
    public Search search { get; set; } = new();
    public DatatableExportOption ExportOption { get; set; } = new();

    public class Column
    {
        public string data { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; } = new();
    }

    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; } = "asc";
    }

    public class Search
    {
        public string value { get; set; } = string.Empty;
        public bool regex { get; set; }
    }
}

public class DatatableExportOption
{
    public int exportType { get; set; } = (int)DatatableExportType.Json;
}

public enum DatatableExportType : int
{
    Json = 1,
    Excel = 2
}

/// <summary>
/// پاسخ استاندارد DataTable به سمت کلاینت
/// </summary>
public class DataTableResponse<T>
{
    public int Draw { get; set; }
    public int RecordsTotal { get; set; }
    public int RecordsFiltered { get; set; }
    public List<T> Data { get; set; } = [];
}

/// <summary>
/// اکستنشن‌های کمکی برای DataTable
/// </summary>
public static class DatatableFullRequestExtensions
{
    public static int GetPageSize(this DatatableFullRequest request)
    {
        if (request.ExportOption.exportType == (int)DatatableExportType.Excel)
            return int.MaxValue;
        return request.length <= 0 || request.length > 10000 ? 10 : request.length;
    }

    public static int GetPageNumber(this DatatableFullRequest request)
    {
        if (request.ExportOption.exportType == (int)DatatableExportType.Excel)
            return 0;
        return request.start / Math.Max(request.length, 1);
    }

    /// <summary>
    /// اعمال ترتیب‌سازی داینامیک بر اساس درخواست DataTable
    /// </summary>
    public static IQueryable<T> ApplyDataTableOrdering<T>(this IQueryable<T> query, DatatableFullRequest request)
    {
        if (request.order == null || request.order.Length == 0)
            return query;

        var order = request.order[0];
        var columnName = order.column < request.columns.Length
            ? request.columns[order.column].data
            : null;

        if (string.IsNullOrWhiteSpace(columnName))
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = typeof(T).GetProperty(columnName,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
            return query;

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        var methodName = order.dir?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";
        var resultExp = Expression.Call(typeof(Queryable), methodName,
            [typeof(T), property.PropertyType], query.Expression, Expression.Quote(orderByExp));

        return query.Provider.CreateQuery<T>(resultExp);
    }
}

/// <summary>
/// کمکی برای ساخت پاسخ DataTable
/// </summary>
public interface IDatatableResponseHelper
{
    Task<IActionResult> Get<T>(DatatableFullRequest datatableRequest,
        ResultOperation<DataTableResponse<T>> handlerResult,
        string? exportFileName = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// پیاده‌سازی پیش‌فرض - می‌توان در پروژه Web بازنویسی کرد
/// </summary>
public class DatatableResponseHelper : IDatatableResponseHelper
{
    public async Task<IActionResult> Get<T>(DatatableFullRequest datatableRequest,
        ResultOperation<DataTableResponse<T>> handlerResult,
        string? exportFileName = null,
        CancellationToken cancellationToken = default)
    {
        if (!handlerResult.IsSuccess)
            return new BadRequestObjectResult(new { message = handlerResult.MessageSingle });

        if (datatableRequest.ExportOption.exportType == (int)DatatableExportType.Excel)
        {
            // TODO: پیاده‌سازی خروجی Excel
            return new OkObjectResult(handlerResult.Data);
        }

        return new OkObjectResult(handlerResult.Data);
    }
}

// نیاز به using Microsoft.AspNetCore.Mvc برای IActionResult
using Microsoft.AspNetCore.Mvc;
