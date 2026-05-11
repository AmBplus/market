using AppCore.Data;
using AppCore.Enums;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Discount.Discounts.Queries;

public class GetDiscountsDataTableQuery : DatatableFullRequest
{
    public string? SearchCode { get; set; }
    public string? SearchName { get; set; }
    public DiscountType? SearchDiscountType { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class DiscountListDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DiscountType DiscountType { get; set; }
    public DiscountScope Scope { get; set; }
    public decimal Value { get; set; }
    public int UsedCount { get; set; }
    public int? UsageLimit { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}

public class GetDiscountsDataTableHandler
{
    private readonly AppDbContext _context;
    public GetDiscountsDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<DiscountListDto>>> Handle(GetDiscountsDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Discounts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(d => d.Code.Contains(query.SearchCode));

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(d => d.Name.Contains(query.SearchName));

        if (query.SearchDiscountType.HasValue)
            baseQuery = baseQuery.Where(d => d.DiscountType == query.SearchDiscountType.Value);

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(d => d.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(d => new DiscountListDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                DiscountType = d.DiscountType,
                Scope = d.Scope,
                Value = d.Value,
                UsedCount = d.UsedCount,
                UsageLimit = d.UsageLimit,
                IsActive = d.IsActive,
                StartedAt = d.StartedAt,
                EndedAt = d.EndedAt
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<DiscountListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<DiscountListDto>>.ToSuccessResult(response);
    }
}
