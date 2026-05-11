using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Products.Queries;

public class GetProductsDataTableQuery : DatatableFullRequest
{
    public string? SearchName { get; set; }
    public string? SearchProductCode { get; set; }
    public long? SearchBrandId { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class ProductListDto
{
    public long Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? BrandName { get; set; }
    public int VariantCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetProductsDataTableHandler
{
    private readonly AppDbContext _context;
    public GetProductsDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<ProductListDto>>> Handle(GetProductsDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Products.AsNoTracking().Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(p => p.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchProductCode))
            baseQuery = baseQuery.Where(p => p.ProductCode.Contains(query.SearchProductCode));

        if (query.SearchBrandId.HasValue)
            baseQuery = baseQuery.Where(p => p.BrandId == query.SearchBrandId.Value);

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(p => p.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(p => new ProductListDto
            {
                Id = p.Id,
                ProductCode = p.ProductCode,
                Name = p.Name,
                BrandName = p.Brand != null ? p.Brand.Name : null,
                VariantCount = p.Variants.Count(v => !v.IsDeleted),
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<ProductListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<ProductListDto>>.ToSuccessResult(response);
    }
}
