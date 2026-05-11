using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.Products.Queries;

public class SelectProductQuery
{
    public string? SearchName { get; set; }
    public string? SearchProductCode { get; set; }
}

public class ProductSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectProductHandler
{
    private readonly ECommerceDbContext _context;
    public SelectProductHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<ProductSelect2Dto>>> Handle(SelectProductQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Products.AsNoTracking().Where(p => !p.IsDeleted && p.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(p => p.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchProductCode))
            baseQuery = baseQuery.Where(p => p.ProductCode.Contains(query.SearchProductCode));

        var result = await baseQuery
            .Select(p => new ProductSelect2Dto
            {
                Id = p.Id,
                Text = p.ProductCode + " - " + p.Name
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<ProductSelect2Dto>>.ToSuccessResult(result);
    }
}
