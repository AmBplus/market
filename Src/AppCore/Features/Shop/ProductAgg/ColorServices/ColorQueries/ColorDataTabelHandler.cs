using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.Queries;

public class ColorDataTableQuery : DatatableFullRequest
{
    public string? SearchTitle { get; set; }
    public string? SearchColorCode { get; set; }
}

public class ColorListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class ColorDataTableHandler
{
    private readonly AppDbContext _context;

    public ColorDataTableHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<DataTableResponse<ColorListDto>>> Handle(ColorDataTableQuery query)
    {
        var baseQuery = _context.Colors
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTitle))
            baseQuery = baseQuery.Where(c => c.Title.Contains(query.SearchTitle));

        if (!string.IsNullOrWhiteSpace(query.SearchColorCode))
            baseQuery = baseQuery.Where(c => c.ColorCode.Contains(query.SearchColorCode));

        var filteredCount = await baseQuery.CountAsync();

        var dtoQuery = baseQuery.Select(c => new ColorListDto
        {
            Id = c.Id,
            Title = c.Title,
            ColorCode = c.ColorCode,
            CreatedAt = c.CreatedAt,
            IsActive = c.IsActive
        });

        var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

        var colors = await orderedQuery
            .Skip(query.start)
            .Take(query.length)
            .ToListAsync();

        var response = new DataTableResponse<ColorListDto>
        {
            Draw = query.draw,
            RecordsTotal = filteredCount,
            RecordsFiltered = filteredCount,
            Data = colors
        };

        return ResultOperation<DataTableResponse<ColorListDto>>.ToSuccessResult(response);
    }
}
