using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Customization.Sliders.Queries;

public class GetSlidersQuery
{
    public bool? SearchIsActive { get; set; }
}

public class SliderDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? SubTitle { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class GetSlidersHandler
{
    private readonly ECommerceDbContext _context;
    public GetSlidersHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<SliderDto>>> Handle(GetSlidersQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Sliders.AsNoTracking();

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(s => s.IsActive == query.SearchIsActive.Value);

        var result = await baseQuery
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
