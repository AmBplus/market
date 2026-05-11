using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.SiteThemes.Queries;

public class GetAllThemesQuery
{
}

public class GetAllThemesHandler
{
    private readonly AppDbContext _context;
    public GetAllThemesHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<SiteThemeDto>>> Handle(GetAllThemesQuery query, CancellationToken ct)
    {
        var themes = await _context.SiteThemes
            .AsNoTracking()
            .OrderByDescending(t => t.IsActive)
            .ThenBy(t => t.Name)
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
            .ToListAsync(ct);

        return ResultOperation<List<SiteThemeDto>>.ToSuccessResult(themes);
    }
}
