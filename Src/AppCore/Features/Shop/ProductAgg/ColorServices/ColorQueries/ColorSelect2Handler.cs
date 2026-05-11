using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.Queries;

public class ColorSelect2Query
{
    public string? SearchTitle { get; set; }
    public string? SearchColorCode { get; set; }
}

public class ColorSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class ColorSelect2Handler
{
    private readonly AppDbContext _context;

    public ColorSelect2Handler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<List<ColorSelect2Dto>>> Handle(ColorSelect2Query query, CancellationToken cancellationToken)
    {
        var baseQuery = _context.Colors
            .AsNoTracking()
            .Where(c => !c.IsDeleted && c.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTitle))
            baseQuery = baseQuery.Where(c => c.Title.Contains(query.SearchTitle));

        if (!string.IsNullOrWhiteSpace(query.SearchColorCode))
            baseQuery = baseQuery.Where(c => c.ColorCode.Contains(query.SearchColorCode));

        var colors = await baseQuery.ToListAsync(cancellationToken);

        var result = colors.Select(c => new ColorSelect2Dto
        {
            Id = c.Id,
            Text = $"{c.Title} ({c.ColorCode})"
        }).ToList();

        return ResultOperation<List<ColorSelect2Dto>>.ToSuccessResult(result);
    }
}
