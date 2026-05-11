using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Settings.Settings.Queries;

public class GetSettingByKeyQuery
{
    public string Key { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
}

public class SettingDto
{
    public long Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public bool IsEditable { get; set; }
}

public class GetSettingByKeyHandler
{
    private readonly AppDbContext _context;
    public GetSettingByKeyHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<SettingDto>> Handle(GetSettingByKeyQuery query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.Key))
            return ResultOperation<SettingDto>.ToFailedResult("کلید تنظیم الزامی است");

        var settingQuery = _context.Settings.AsNoTracking().Where(s => s.Key == query.Key);

        if (!string.IsNullOrWhiteSpace(query.Module))
            settingQuery = settingQuery.Where(s => s.Module == query.Module);

        var setting = await settingQuery
            .Select(s => new SettingDto
            {
                Id = s.Id,
                Key = s.Key,
                Value = s.Value,
                Module = s.Module,
                ValueType = s.ValueType,
                IsEditable = s.IsEditable
            })
            .FirstOrDefaultAsync(ct);

        if (setting is null)
            return ResultOperation<SettingDto>.ToFailedResult("تنظیم یافت نشد");

        return ResultOperation<SettingDto>.ToSuccessResult(setting);
    }
}
