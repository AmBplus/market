using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.Tag.Queries;

public class GetTagByIdQuery
{
    public long Id { get; set; }
}

public class TagDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GetTagByIdHandler
{
    private readonly AppDbContext _context;

    public GetTagByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<TagDto>> Handle(GetTagByIdQuery query)
    {
        var tag = await _context.ProductTags
            .AsNoTracking()
            .Where(t => t.Id == query.Id && !t.IsDeleted)
            .Select(t => new TagDto
            {
                Id = t.Id,
                Title = t.Title,
                Slug = t.Slug,
                IsActive = t.IsActive,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (tag is null)
            return ResultOperation<TagDto>.ToFailedResult("تگ مورد نظر یافت نشد.");

        return ResultOperation<TagDto>.ToSuccessResult(tag);
    }
}
