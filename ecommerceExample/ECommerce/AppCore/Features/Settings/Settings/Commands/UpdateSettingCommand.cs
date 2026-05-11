using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Settings.Settings.Commands;

public class UpdateSettingCommand
{
    public long Id { get; set; }
    public string Value { get; set; } = string.Empty;
}

public class UpdateSettingHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateSettingHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateSettingCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه تنظیم نامعتبر است");

        var setting = await _context.Settings.FindAsync(new object[] { command.Id }, ct);
        if (setting is null)
            return ResultOperation.ToFailedResult("تنظیم یافت نشد");

        if (!setting.IsEditable)
            return ResultOperation.ToFailedResult("این تنظیم قابل ویرایش نیست");

        setting.Value = command.Value;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تنظیم با موفقیت بروزرسانی شد");
    }
}
