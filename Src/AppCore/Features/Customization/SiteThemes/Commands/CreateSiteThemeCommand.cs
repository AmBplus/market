using AppCore.Data;
using AppCore.Domains.Customization;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.SiteThemes.Commands;

public class CreateSiteThemeCommand
{
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#3B82F6";
    public string SecondaryColor { get; set; } = "#10B981";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string TextColor { get; set; } = "#1F2937";
    public string AccentColor { get; set; } = "#F59E0B";
    public string? FontFamily { get; set; }
    public string? ExtendedSettingsJson { get; set; }
}

public class CreateSiteThemeHandler
{
    private readonly AppDbContext _context;
    public CreateSiteThemeHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateSiteThemeCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام قالب الزامی است");

        var theme = new SiteTheme
        {
            Name = command.Name,
            PrimaryColor = command.PrimaryColor,
            SecondaryColor = command.SecondaryColor,
            BackgroundColor = command.BackgroundColor,
            TextColor = command.TextColor,
            AccentColor = command.AccentColor,
            FontFamily = command.FontFamily,
            ExtendedSettingsJson = command.ExtendedSettingsJson,
            IsActive = false
        };

        _context.Add(theme);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("قالب با موفقیت ایجاد شد");
    }
}
