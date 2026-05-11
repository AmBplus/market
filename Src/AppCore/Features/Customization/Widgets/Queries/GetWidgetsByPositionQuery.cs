using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.Widgets.Queries;

public class GetWidgetsByPositionQuery
{
    public string Position { get; set; } = string.Empty;
}

public class WidgetDto
{
    public long Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class GetWidgetsByPositionHandler
{
    private readonly AppDbContext _context;
    public GetWidgetsByPositionHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<WidgetDto>>> Handle(GetWidgetsByPositionQuery query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.Position))
            return ResultOperation<List<WidgetDto>>.ToFailedResult("موقعیت ویجت الزامی است");

        var now = DateTime.UtcNow;

        var result = await _context.Widgets
            .AsNoTracking()
            .Where(w => w.Position == query.Position
                && w.IsActive
                && (w.StartedAt == null || w.StartedAt <= now)
                && (w.EndedAt == null || w.EndedAt >= now))
            .OrderBy(w => w.DisplayOrder)
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
