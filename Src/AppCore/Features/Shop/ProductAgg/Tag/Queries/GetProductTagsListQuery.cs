using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.Tag.Queries;

public class GetProductTagsListQuery : DatatableFullRequest
{
    public string? SearchTitle { get; set; }
    public string? SearchSlug { get; set; }
    public bool? IsActive { get; set; }
}

public class TagListDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetProductTagsListHandler
{
    private readonly AppDbContext _context;

    public GetProductTagsListHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<DataTableResponse<TagListDto>>> Handle(GetProductTagsListQuery query)
    {
        var baseQuery = _context.ProductTags
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTitle))
            baseQuery = baseQuery.Where(t => t.Title.Contains(query.SearchTitle));

        if (!string.IsNullOrWhiteSpace(query.SearchSlug))
            baseQuery = baseQuery.Where(t => t.Slug!.Contains(query.SearchSlug));

        if (query.IsActive.HasValue)
            baseQuery = baseQuery.Where(t => t.IsActive == query.IsActive.Value);

        var filteredCount = await baseQuery.CountAsync();

        var dtoQuery = baseQuery.Select(t => new TagListDto
        {
            Id = t.Id,
            Title = t.Title,
            Slug = t.Slug,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt
        });

        var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

        var tags = await orderedQuery
            .Skip(query.start)
            .Take(query.length)
            .ToListAsync();

        var response = new DataTableResponse<TagListDto>
        {
            Draw = query.draw,
            RecordsTotal = filteredCount,
            RecordsFiltered = filteredCount,
            Data = tags
        };

        return ResultOperation<DataTableResponse<TagListDto>>.ToSuccessResult(response);
    }
}
