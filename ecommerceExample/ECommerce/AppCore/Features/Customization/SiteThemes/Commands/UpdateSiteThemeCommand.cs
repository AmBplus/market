using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.SiteThemes.Commands;

public class UpdateSiteThemeCommand
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#3B82F6";
    public string SecondaryColor { get; set; } = "#10B981";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string TextColor { get; set; } = "#1F2937";
    public string AccentColor { get; set; } = "#F59E0B";
    public string? FontFamily { get; set; }
    public string? ExtendedSettingsJson { get; set; }
}

public class UpdateSiteThemeHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateSiteThemeHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateSiteThemeCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه قالب نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام قالب الزامی است");

        var theme = await _context.SiteThemes.FindAsync(new object[] { command.Id }, ct);
        if (theme is null)
            return ResultOperation.ToFailedResult("قالب یافت نشد");

        theme.Name = command.Name;
        theme.PrimaryColor = command.PrimaryColor;
        theme.SecondaryColor = command.SecondaryColor;
        theme.BackgroundColor = command.BackgroundColor;
        theme.TextColor = command.TextColor;
        theme.AccentColor = command.AccentColor;
        theme.FontFamily = command.FontFamily;
        theme.ExtendedSettingsJson = command.ExtendedSettingsJson;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("قالب با موفقیت ویرایش شد");
    }
}
