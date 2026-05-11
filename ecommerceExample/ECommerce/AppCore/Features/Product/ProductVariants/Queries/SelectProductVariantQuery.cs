using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.ProductVariants.Queries;

public class SelectProductVariantQuery
{
    public long? SearchProductId { get; set; }
    public string? SearchTitle { get; set; }
}

public class ProductVariantSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectProductVariantHandler
{
    private readonly ECommerceDbContext _context;
    public SelectProductVariantHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<ProductVariantSelect2Dto>>> Handle(SelectProductVariantQuery query, CancellationToken ct)
    {
        var baseQuery = _context.ProductVariants.AsNoTracking().Where(pv => !pv.IsDeleted && pv.IsActive);

        if (query.SearchProductId.HasValue)
            baseQuery = baseQuery.Where(pv => pv.ProductId == query.SearchProductId.Value);

        if (!string.IsNullOrWhiteSpace(query.SearchTitle))
            baseQuery = baseQuery.Where(pv => pv.Title.Contains(query.SearchTitle));

        var result = await baseQuery
            .Select(pv => new ProductVariantSelect2Dto
            {
                Id = pv.Id,
                Text = pv.VariantCode + " - " + pv.Title
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<ProductVariantSelect2Dto>>.ToSuccessResult(result);
    }
}
