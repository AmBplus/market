using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Customization.Sliders.Queries;

public class GetActiveSlidersQuery
{
}

public class GetActiveSlidersHandler
{
    private readonly AppDbContext _context;
    public GetActiveSlidersHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<SliderDto>>> Handle(GetActiveSlidersQuery query, CancellationToken ct)
    {
        var now = DateTime.UtcNow;

        var result = await _context.Sliders
            .AsNoTracking()
            .Where(s => s.IsActive
                && (s.StartedAt == null || s.StartedAt <= now)
                && (s.EndedAt == null || s.EndedAt >= now))
            .OrderBy(s => s.DisplayOrder)
            .Select(s => new SliderDto
            {
                Id = s.Id,
                Title = s.Title,
                SubTitle = s.SubTitle,
                ImageUrl = s.ImageUrl,
                MobileImageUrl = s.MobileImageUrl,
                LinkUrl = s.LinkUrl,
                LinkText = s.LinkText,
                DisplayOrder = s.DisplayOrder,
                IsActive = s.IsActive,
                StartedAt = s.StartedAt,
                EndedAt = s.EndedAt
            })
            .ToListAsync(ct);

        return ResultOperation<List<SliderDto>>.ToSuccessResult(result);
    }
}
