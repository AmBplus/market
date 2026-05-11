using ECommerce.AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Brands.Queries;

public class GetBrandsDataTableQuery : DatatableFullRequest
{
    public string? SearchName { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class BrandListDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public int ProductCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetBrandsDataTableHandler
{
    private readonly ECommerceDbContext _context;
    public GetBrandsDataTableHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<BrandListDto>>> Handle(GetBrandsDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Brands.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(b => b.Name.Contains(query.SearchName));

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(b => b.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(b => new BrandListDto
            {
                Id = b.Id,
                Name = b.Name,
                Slug = b.Slug,
                ProductCount = b.Products.Count(p => !p.IsDeleted),
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<BrandListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<BrandListDto>>.ToSuccessResult(response);
    }
}
