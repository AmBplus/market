using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Settings.Settings.Queries;

public class GetSettingsByModuleQuery
{
    public string Module { get; set; } = string.Empty;
}

public class GetSettingsByModuleHandler
{
    private readonly AppDbContext _context;
    public GetSettingsByModuleHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<SettingDto>>> Handle(GetSettingsByModuleQuery query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.Module))
            return ResultOperation<List<SettingDto>>.ToFailedResult("نام ماژول الزامی است");

        var settings = await _context.Settings
            .AsNoTracking()
            .Where(s => s.Module == query.Module)
            .OrderBy(s => s.Key)
            .Select(s => new SettingDto
            {
                Id = s.Id,
                Key = s.Key,
                Value = s.Value,
                Module = s.Module,
                ValueType = s.ValueType,
                IsEditable = s.IsEditable
            })
            .ToListAsync(ct);

        return ResultOperation<List<SettingDto>>.ToSuccessResult(settings);
    }
}
