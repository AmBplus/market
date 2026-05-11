using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.Widgets.Queries;

public class GetAllWidgetsQuery
{
    public string? SearchType { get; set; }
    public string? SearchPosition { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class GetAllWidgetsHandler
{
    private readonly AppDbContext _context;
    public GetAllWidgetsHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<WidgetDto>>> Handle(GetAllWidgetsQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Widgets.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchType))
            baseQuery = baseQuery.Where(w => w.Type.Contains(query.SearchType));

        if (!string.IsNullOrWhiteSpace(query.SearchPosition))
            baseQuery = baseQuery.Where(w => w.Position.Contains(query.SearchPosition));

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(w => w.IsActive == query.SearchIsActive.Value);

        var result = await baseQuery
            .OrderBy(w => w.Position)
            .ThenBy(w => w.DisplayOrder)
            .Select(w => new WidgetDto
            {
                Id = w.Id,
                Type = w.Type,
                Title = w.Title,
                ConfigJson = w.ConfigJson,
                Position = w.Position,
                DisplayOrder = w.DisplayOrder,
                IsActive = w.IsActive,
                StartedAt = w.StartedAt,
                EndedAt = w.EndedAt
            })
            .ToListAsync(ct);

        return ResultOperation<List<WidgetDto>>.ToSuccessResult(result);
    }
}
