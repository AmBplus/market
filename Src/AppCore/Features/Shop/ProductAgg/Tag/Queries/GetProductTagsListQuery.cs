using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppCore.Features.Shop.ProductAgg.Tag.Queries;


public class GetProductTagsListQuery : DatatableFullRequest
{
    public string? SearchName { get; set; }
    public string? SearchSlug { get; set; }
    public bool? IsActive { get; set; }
}

public class TagListDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Slug { get; set; }
    public string? Description { get; set; }
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
            .Where(t => !t.IsDelete)
            .AsQueryable();

        // فیلترها
        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(t => t.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchSlug))
            baseQuery = baseQuery.Where(t => t.Slug!.Contains(query.SearchSlug));

        if (query.IsActive.HasValue)
            baseQuery = baseQuery.Where(t => t.IsActive == query.IsActive.Value);

        // شمارش
        var filteredCount = await baseQuery.CountAsync();

        // Map به DTO
        var dtoQuery = baseQuery.Select(t => new TagListDto
        {
            Id = t.Id,
            Name = t.Name,
            Slug = t.Slug,
            Description = t.Description,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt
        });

        // مرتب‌سازی داینامیک
        var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

        // صفحه‌بندی
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
