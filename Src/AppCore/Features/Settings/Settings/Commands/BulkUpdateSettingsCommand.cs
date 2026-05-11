using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Settings.Settings.Commands;

public class UpdateSettingItemDto
{
    public long Id { get; set; }
    public string Value { get; set; } = string.Empty;
}

public class BulkUpdateSettingsCommand
{
    public List<UpdateSettingItemDto> Settings { get; set; } = new();
}

public class BulkUpdateSettingsHandler
{
    private readonly AppDbContext _context;
    public BulkUpdateSettingsHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(BulkUpdateSettingsCommand command, CancellationToken ct)
    {
        if (command.Settings is null || !command.Settings.Any())
            return ResultOperation.ToFailedResult("لیست تنظیمات خالی است");

        var settingIds = command.Settings.Select(s => s.Id).Distinct().ToList();

        var settings = await _context.Settings
            .Where(s => settingIds.Contains(s.Id))
            .ToListAsync(ct);

        foreach (var item in command.Settings)
        {
            var setting = settings.FirstOrDefault(s => s.Id == item.Id);
            if (setting is null)
                continue;

            if (!setting.IsEditable)
                continue;

            setting.Value = item.Value;
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تنظیمات با موفقیت بروزرسانی شد");
    }
}
