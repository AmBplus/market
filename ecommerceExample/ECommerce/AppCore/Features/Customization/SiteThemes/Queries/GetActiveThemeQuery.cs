using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.SiteThemes.Queries;

public class GetActiveThemeQuery
{
}

public class SiteThemeDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = string.Empty;
    public string SecondaryColor { get; set; } = string.Empty;
    public string BackgroundColor { get; set; } = string.Empty;
    public string TextColor { get; set; } = string.Empty;
    public string AccentColor { get; set; } = string.Empty;
    public string? FontFamily { get; set; }
    public string? ExtendedSettingsJson { get; set; }
    public bool IsActive { get; set; }
}

public class GetActiveThemeHandler
{
    private readonly ECommerceDbContext _context;
    public GetActiveThemeHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<SiteThemeDto>> Handle(GetActiveThemeQuery query, CancellationToken ct)
    {
        var theme = await _context.SiteThemes
            .AsNoTracking()
            .Where(t => t.IsActive)
            .Select(t => new SiteThemeDto
            {
                Id = t.Id,
                Name = t.Name,
                PrimaryColor = t.PrimaryColor,
                SecondaryColor = t.SecondaryColor,
                BackgroundColor = t.BackgroundColor,
                TextColor = t.TextColor,
                AccentColor = t.AccentColor,
                FontFamily = t.FontFamily,
                ExtendedSettingsJson = t.ExtendedSettingsJson,
                IsActive = t.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (theme is null)
            return ResultOperation<SiteThemeDto>.ToFailedResult("قالب فعالی یافت نشد");

        return ResultOperation<SiteThemeDto>.ToSuccessResult(theme);
    }
}
