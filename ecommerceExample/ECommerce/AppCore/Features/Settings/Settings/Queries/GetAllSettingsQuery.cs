using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Settings.Settings.Queries;

public class GetAllSettingsQuery
{
}

public class GetAllSettingsHandler
{
    private readonly ECommerceDbContext _context;
    public GetAllSettingsHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<SettingDto>>> Handle(GetAllSettingsQuery query, CancellationToken ct)
    {
        var settings = await _context.Settings
            .AsNoTracking()
            .OrderBy(s => s.Module)
            .ThenBy(s => s.Key)
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
