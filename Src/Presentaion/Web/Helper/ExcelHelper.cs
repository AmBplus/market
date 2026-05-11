using ClosedXML.Excel;
using Framework;
using Framework.Aspc.Helper;
using Framework.Helpers;
using Framework.ResultHelper;
using Framework.TextHelper;
using LargeXlsx;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceHost.Helper;

public sealed class ExcelHelper : IExcelHelper
{
    // برای جلوگیری از انفجار خطاها در حافظه
    private const int MaxValidationErrors = 2000;

    /* =====================================================
     * IMPORT : Excel → Class  (ClosedXML)
     * ===================================================== */

    public async Task<ResultOperation<List<T>>> ExcelToClassAsync<T>(
        IFormFile file,
        Dictionary<int, string> columnIndexMappings,
        CancellationToken cancellationToken = default
    ) where T : class, new()
    {
        var validationResult = ValidateImportRequest<T>(file, columnIndexMappings);
        if (!validationResult.IsSuccess)
            return validationResult;

        var validEntities = new List<T>();
        var validationErrors = new List<string>();

        try
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet is null)
                return ResultOperation<List<T>>.ToFailedResult("فایل Excel هیچ worksheetای ندارد.");

            var rows = worksheet.RowsUsed().Skip(1); // skip header
            var propertyMap = CreatePropertyMap<T>(columnIndexMappings);

            foreach (var row in rows)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var entity = new T();
                bool hasError = false;

                foreach (var map in propertyMap)
                {
                    try
                    {
                        var cell = row.Cell(map.Key + 1); // mapping is 0-based
                        var value = cell.GetString();

                        AssignPropertyValue(entity, map.Value, value);
                    }
                    catch (Exception ex)
                    {
                        if (validationErrors.Count < MaxValidationErrors)
                        {
                            validationErrors.Add(
                                $"Row {row.RowNumber()} | Column {map.Key} | {ex.Message}"
                            );
                        }
                        hasError = true;
                    }
                }

                if (!ValidateEntity(entity, (uint)row.RowNumber(), validationErrors))
                    hasError = true;

                if (!hasError)
                    validEntities.Add(entity);

                if (validationErrors.Count >= MaxValidationErrors)
                    break;
            }

            if (validationErrors.Any())
                return ResultOperation<List<T>>.ToFailedResult(validationErrors, validEntities);

            return ResultOperation<List<T>>.ToSuccessResult(
                $"Operation completed successfully. Records count: {validEntities.Count}",
                validEntities
            );
        }
        catch (OperationCanceledException)
        {
            return ResultOperation<List<T>>.ToFailedResult("عملیات توسط کاربر لغو شد.");
        }
        catch (Exception ex)
        {
            return ResultOperation<List<T>>.ToFailedResult(ex.Message);
        }
    }

    /* =====================================================
     * EXPORT : Class → Excel  (LargeXlsx streaming)
     * ===================================================== */

    public async Task<ResultOperation<MemoryStream>> ClassToExcelAsync<T>(
        IEnumerable<T> items,
        ExcelExportOptions? options = null,
        CancellationToken cancellationToken = default
    ) where T : class
    {
        options ??= new ExcelExportOptions();

        var swTotal = Stopwatch.StartNew();
        try
        {
            long mem0 = GC.GetTotalMemory(false) / (1024 * 1024);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT start | mem={mem0}MB | sheet={options.SheetName}");

            // 1) Plan فقط یک بار برای هر T ساخته می‌شود (cache per closed generic)
            var sw = Stopwatch.StartNew();
            var plan = ExportPlanCache<T>.Plan;
            sw.Stop();
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT phase=GetExportPlan ms={sw.ElapsedMilliseconds} | cols={plan.Columns.Length}");

            if (plan.Columns.Length == 0)
                return ResultOperation<MemoryStream>.ToFailedResult("هیچ پراپرتی قابل خروجی یافت نشد.");

            // 2) Stream خروجی (برای جلوگیری از Dispose شدن stream توسط writer، wrapper می‌گذاریم)
            sw.Restart();
            var output = new MemoryStream(capacity: 16 * 1024 * 1024);
            var nonClosing = new NonDisposingStream(output);

            // نکته: LargeXlsx در Dispose نهایی‌سازی/zip را انجام می‌دهد.
            // requireCellReferences=false طبق خود پروژه می‌تواند سریع‌تر باشد. :contentReference[oaicite:2]{index=2}
            using (var xlsx = new XlsxWriter(
                nonClosing,
                compressionLevel: XlsxCompressionLevel.Fastest,
                requireCellReferences: false,
                skipInvalidCharacters: true,
                commitThreshold: 1 * 1024 * 1024
            ))
            {
                sw.Stop();
                long mem1 = GC.GetTotalMemory(false) / (1024 * 1024);
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT phase=CreateWorkbook ms={sw.ElapsedMilliseconds} | mem={mem1}MB");

                // 3) Sheet
                sw.Restart();
                // rightToLeft=true برای RTL :contentReference[oaicite:3]{index=3}
                xlsx.BeginWorksheet(options.SheetName, rightToLeft: true);

                // 4) Header
                xlsx.BeginRow();
                foreach (var col in plan.Columns)
                    xlsx.Write(col.Header);
                sw.Stop();
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT phase=Header ms={sw.ElapsedMilliseconds}");

                // 5) Data
                sw.Restart();
                int rowCount = 0;

                foreach (var item in items ?? Enumerable.Empty<T>())
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    xlsx.BeginRow();
                    for (int i = 0; i < plan.Columns.Length; i++)
                    {
                        // اینجا دیگر type-check نداریم؛ writer هر ستون از قبل آماده است
                        plan.Columns[i].Write(xlsx, item);
                    }

                    rowCount++;

                    // کمی فرصت به cancellation و flush
                    if ((rowCount & 4095) == 0)
                        xlsx.TryCommit();
                }

                sw.Stop();
                long mem2 = GC.GetTotalMemory(false) / (1024 * 1024);
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT phase=InsertData ms={sw.ElapsedMilliseconds} | rows={rowCount} cols={plan.Columns.Length} | mem={mem2}MB");

                // پایان worksheet به صورت implicit در Dispose انجام می‌شود
            }

            // 6) Finalize done (Dispose writer) => stream آماده است
            output.Position = 0;
            swTotal.Stop();

            long mem3 = GC.GetTotalMemory(false) / (1024 * 1024);
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT done totalMs={swTotal.ElapsedMilliseconds} | bytes={output.Length} | mem={mem3}MB");

            // (برای حذف warning async)
            await Task.CompletedTask;

            return ResultOperation<MemoryStream>.ToSuccessResult(
                $"فایل اکسل با موفقیت ایجاد شد. تعداد ردیف‌ها: {Math.Max(0, (items?.Count() ?? 0))}",
                output
            );
        }
        catch (OperationCanceledException)
        {
            swTotal.Stop();
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT canceled totalMs={swTotal.ElapsedMilliseconds}");
            return ResultOperation<MemoryStream>.ToFailedResult("عملیات خروجی اکسل لغو شد.");
        }
        catch (Exception ex)
        {
            swTotal.Stop();
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|EXCEL|EXPORT error totalMs={swTotal.ElapsedMilliseconds} | {ex.Message}");
            return ResultOperation<MemoryStream>.ToFailedResult(ex.Message);
        }
    }

    /* =====================================================
     * EXPORT PLAN (cached per T)
     * ===================================================== */

    private static class ExportPlanCache<T> where T : class
    {
        public static readonly ExportPlan<T> Plan = ExportPlan<T>.Create();
    }

    private sealed class ExportPlan<T> where T : class
    {
        public ExportColumn<T>[] Columns { get; }

        private ExportPlan(ExportColumn<T>[] cols) => Columns = cols;

        public static ExportPlan<T> Create()
        {
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Select(p => new
                {
                    Prop = p,
                    Meta = ResolveExportMeta(p)
                })
                .Where(x => x.Meta.Enabled)
                .ToList();

            var cols = new List<ExportColumn<T>>(props.Count);

            foreach (var x in props)
            {
                var writer = CreateWriter(x.Prop, x.Meta.PersianDate);
                cols.Add(new ExportColumn<T>(x.Meta.Header, writer));
            }

            return new ExportPlan<T>(cols.ToArray());
        }

        private static ExportWriter<T> CreateWriter(PropertyInfo prop, bool persianDate)
        {
            var propType = prop.PropertyType;
            var nn = Nullable.GetUnderlyingType(propType);
            var coreType = nn ?? propType;

            // DateTime / DateTime?
            if (coreType == typeof(DateTime))
            {
                if (nn is not null)
                {
                    var getter = CompileGetter<DateTime?>(prop);

                    if (persianDate)
                    {
                        return (w, obj) =>
                        {
                            var v = getter(obj);
                            if (v.HasValue) w.Write(v.Value.ToPersianDateString());
                            else w.Write();
                        };
                    }

                    return (w, obj) =>
                    {
                        var v = getter(obj);
                        if (v.HasValue) w.Write(v.Value);
                        else w.Write();
                    };
                }
                else
                {
                    var getter = CompileGetter<DateTime>(prop);

                    if (persianDate)
                    {
                        return (w, obj) => w.Write(getter(obj).ToPersianDateString());
                    }

                    return (w, obj) => w.Write(getter(obj));
                }
            }

            // string
            if (coreType == typeof(string))
            {
                var getter = CompileGetter<string?>(prop);
                return (w, obj) => w.Write(getter(obj)); // null => blank :contentReference[oaicite:4]{index=4}
            }

            // bool / bool?
            if (coreType == typeof(bool))
            {
                if (nn is not null)
                {
                    var getter = CompileGetter<bool?>(prop);
                    return (w, obj) =>
                    {
                        var v = getter(obj);
                        if (v.HasValue) w.Write(v.Value);
                        else w.Write();
                    };
                }
                else
                {
                    var getter = CompileGetter<bool>(prop);
                    return (w, obj) => w.Write(getter(obj));
                }
            }

            // int / int?
            if (coreType == typeof(int))
            {
                if (nn is not null)
                {
                    var getter = CompileGetter<int?>(prop);
                    return (w, obj) =>
                    {
                        var v = getter(obj);
                        if (v.HasValue) w.Write(v.Value);
                        else w.Write();
                    };
                }
                else
                {
                    var getter = CompileGetter<int>(prop);
                    return (w, obj) => w.Write(getter(obj));
                }
            }

            // long / long?  (LargeXlsx مستقیم long ندارد؛ امن‌ترین راه: string یا decimal)
            // برای جلوگیری از خراب شدن کد ملی/شناسه‌ها، اینجا string می‌نویسیم.
            if (coreType == typeof(long))
            {
                if (nn is not null)
                {
                    var getter = CompileGetter<long?>(prop);
                    return (w, obj) =>
                    {
                        var v = getter(obj);
                        if (v.HasValue) w.Write(v.Value.ToString());
                        else w.Write();
                    };
                }
                else
                {
                    var getter = CompileGetter<long>(prop);
                    return (w, obj) => w.Write(getter(obj).ToString());
                }
            }

            // decimal / decimal?
            if (coreType == typeof(decimal))
            {
                if (nn is not null)
                {
                    var getter = CompileGetter<decimal?>(prop);
                    return (w, obj) =>
                    {
                        var v = getter(obj);
                        if (v.HasValue) w.Write(v.Value);
                        else w.Write();
                    };
                }
                else
                {
                    var getter = CompileGetter<decimal>(prop);
                    return (w, obj) => w.Write(getter(obj));
                }
            }

            // double / double?
            if (coreType == typeof(double))
            {
                if (nn is not null)
                {
                    var getter = CompileGetter<double?>(prop);
                    return (w, obj) =>
                    {
                        var v = getter(obj);
                        if (v.HasValue) w.Write(v.Value);
                        else w.Write();
                    };
                }
                else
                {
                    var getter = CompileGetter<double>(prop);
                    return (w, obj) => w.Write(getter(obj));
                }
            }

            // fallback: ToString (بدون reflection per row)
            var objGetter = CompileGetter<object?>(prop);
            return (w, obj) =>
            {
                var v = objGetter(obj);
                if (v is null) w.Write();
                else w.Write(v.ToString());
            };
        }

        private static Func<T, TValue> CompileGetter<TValue>(PropertyInfo prop)
        {
            var param = Expression.Parameter(typeof(T), "x");

            Expression body = Expression.Property(
                prop.DeclaringType != typeof(T)
                    ? Expression.Convert(param, prop.DeclaringType!)
                    : param,
                prop
            );

            // اگر نوع property دقیقاً TValue نیست، Convert می‌زنیم
            if (body.Type != typeof(TValue))
                body = Expression.Convert(body, typeof(TValue));

            return Expression.Lambda<Func<T, TValue>>(body, param).Compile();
        }
    }

    private delegate void ExportWriter<T>(XlsxWriter writer, T obj);

    private readonly struct ExportColumn<T>
    {
        public string Header { get; }
        public ExportWriter<T> Write { get; }

        public ExportColumn(string header, ExportWriter<T> write)
        {
            Header = header;
            Write = write;
        }
    }

    private sealed class ExportMeta
    {
        public bool Enabled { get; init; }
        public string Header { get; init; } = "";
        public bool PersianDate { get; init; } = true;
    }

    private static ExportMeta ResolveExportMeta(PropertyInfo prop)
    {
        var exportAttr = prop.GetCustomAttribute<ExportColumnAttribute>();

        if (exportAttr is not null && exportAttr.Enabled == false)
            return new ExportMeta { Enabled = false };

        string header;

        if (!string.IsNullOrWhiteSpace(exportAttr?.DisplayName))
            header = exportAttr!.DisplayName!.Trim();
        else
        {
            var displayAttr = prop.GetCustomAttribute<DisplayAttribute>();
            if (!string.IsNullOrWhiteSpace(displayAttr?.Name))
                header = displayAttr!.Name!.Trim();
            else
            {
                var displayNameAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (!string.IsNullOrWhiteSpace(displayNameAttr?.DisplayName))
                    header = displayNameAttr!.DisplayName.Trim();
                else
                    header = prop.Name;
            }
        }

        // PersianDate: اگر در ExportColumnAttribute شما اضافه شد، استفاده می‌کنیم.
        // اگر نبود، پیشفرض true.
        bool persianDate = true;
        if (exportAttr is not null)
        {
            var pi = exportAttr.GetType().GetProperty("PersianDate", BindingFlags.Public | BindingFlags.Instance);
            if (pi is not null && pi.PropertyType == typeof(bool))
            {
                persianDate = (bool)(pi.GetValue(exportAttr) ?? true);
            }
        }

        return new ExportMeta
        {
            Enabled = true,
            Header = header,
            PersianDate = persianDate
        };
    }

    /* =====================================================
     * Import Helpers (همان منطق قبلی، بدون اضافه کردن منطق جدید)
     * ===================================================== */

    private Dictionary<int, PropertyInfo> CreatePropertyMap<T>(
        Dictionary<int, string> columnIndexMappings
    )
    {
        var properties = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(p => p.Name, p => p);

        var map = new Dictionary<int, PropertyInfo>();

        foreach (var kv in columnIndexMappings)
        {
            if (!properties.TryGetValue(kv.Value, out var prop))
                throw new InvalidOperationException(
                    $"Property '{kv.Value}' does not exist on type {typeof(T).Name}"
                );

            map[kv.Key] = prop;
        }

        return map;
    }

    private void AssignPropertyValue<T>(
        T entity,
        PropertyInfo property,
        string value
    )
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        if (property.PropertyType == typeof(string))
            value = value.FixFarsiStr();

        var targetType =
            Nullable.GetUnderlyingType(property.PropertyType)
            ?? property.PropertyType;

        property.SetValue(entity, Convert.ChangeType(value, targetType));
    }

    private bool ValidateEntity<T>(
        T entity,
        uint rowIndex,
        List<string> errors
    )
    {
        var context = new ValidationContext(entity);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(entity, context, results, true))
        {
            foreach (var r in results)
            {
                if (errors.Count < MaxValidationErrors)
                    errors.Add($"Row {rowIndex} | {r.ErrorMessage}");
            }
            return false;
        }

        return true;
    }

    private ResultOperation<List<T>> ValidateImportRequest<T>(
        IFormFile file,
        Dictionary<int, string> columnIndexMappings
    )
    {
        if (!file.IsRealExcel())
            return ResultOperation<List<T>>.ToFailedResult("فایل معتبر Excel نیست.");

        if (columnIndexMappings == null || !columnIndexMappings.Any())
            return ResultOperation<List<T>>.ToFailedResult("Column mappings تعریف نشده است.");

        return ResultOperation<List<T>>.ToSuccessResult(new List<T>());
    }

    /* =====================================================
     * Non-disposing stream wrapper
     * ===================================================== */

    private sealed class NonDisposingStream : Stream
    {
        private readonly Stream _inner;
        public NonDisposingStream(Stream inner) => _inner = inner;

        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => _inner.CanWrite;
        public override long Length => _inner.Length;

        public override long Position { get => _inner.Position; set => _inner.Position = value; }

        public override void Flush() => _inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => _inner.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
        public override void SetLength(long value) => _inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);

        protected override void Dispose(bool disposing)
        {
            // intentionally do NOT dispose _inner
        }
    }
}
