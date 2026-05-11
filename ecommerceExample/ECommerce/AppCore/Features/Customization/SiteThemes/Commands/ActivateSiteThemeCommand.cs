using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.SiteThemes.Commands;

public class ActivateSiteThemeCommand
{
    public long Id { get; set; }
}

public class ActivateSiteThemeHandler
{
    private readonly ECommerceDbContext _context;
    public ActivateSiteThemeHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(ActivateSiteThemeCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه قالب نامعتبر است");

        var theme = await _context.SiteThemes.FindAsync(new object[] { command.Id }, ct);
        if (theme is null)
            return ResultOperation.ToFailedResult("قالب یافت نشد");

        // Deactivate all themes
        var allThemes = await _context.SiteThemes.ToListAsync(ct);
        foreach (var t in allThemes)
            t.IsActive = false;

        // Activate the selected theme
        theme.IsActive = true;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("قالب با موفقیت فعال شد");
    }
}
