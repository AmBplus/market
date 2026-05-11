using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.ProductVariants.Queries;

public class GetProductVariantsDataTableQuery : DatatableFullRequest
{
    public long? SearchProductId { get; set; }
    public string? SearchVariantCode { get; set; }
    public string? SearchSku { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class ProductVariantListDto
{
    public long Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string VariantCode { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public decimal DefaultPrice { get; set; }
    public int CurrentStock { get; set; }
    public bool IsActive { get; set; }
}

public class GetProductVariantsDataTableHandler
{
    private readonly AppDbContext _context;
    public GetProductVariantsDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<ProductVariantListDto>>> Handle(GetProductVariantsDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.ProductVariants.AsNoTracking().Where(pv => !pv.IsDeleted);

        if (query.SearchProductId.HasValue)
            baseQuery = baseQuery.Where(pv => pv.ProductId == query.SearchProductId.Value);

        if (!string.IsNullOrWhiteSpace(query.SearchVariantCode))
            baseQuery = baseQuery.Where(pv => pv.VariantCode.Contains(query.SearchVariantCode));

        if (!string.IsNullOrWhiteSpace(query.SearchSku))
            baseQuery = baseQuery.Where(pv => pv.Sku.Contains(query.SearchSku));

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(pv => pv.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(pv => new ProductVariantListDto
            {
                Id = pv.Id,
                ProductName = pv.Product.Name,
                VariantCode = pv.VariantCode,
                Sku = pv.Sku,
                Title = pv.Title,
                DefaultPrice = pv.DefaultPrice,
                CurrentStock = pv.StockLedgers.Sum(sl => sl.QuantityChange),
                IsActive = pv.IsActive
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<ProductVariantListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<ProductVariantListDto>>.ToSuccessResult(response);
    }
}
