using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.ProductVariants.Queries;

public class GetProductVariantByIdQuery
{
    public long Id { get; set; }
}

public class ProductVariantDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string VariantCode { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? VariantAttributesJson { get; set; }
    public decimal DefaultPrice { get; set; }
    public long CurrencyId { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
    public int CurrentStock { get; set; }
}

public class GetProductVariantByIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetProductVariantByIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<ProductVariantDto>> Handle(GetProductVariantByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<ProductVariantDto>.ToFailedResult("شناسه تنوع نامعتبر است");

        var variant = await _context.ProductVariants
            .AsNoTracking()
            .Where(pv => pv.Id == query.Id && !pv.IsDeleted)
            .Select(pv => new ProductVariantDto
            {
                Id = pv.Id,
                ProductId = pv.ProductId,
                VariantCode = pv.VariantCode,
                Sku = pv.Sku,
                Title = pv.Title,
                VariantAttributesJson = pv.VariantAttributesJson,
                DefaultPrice = pv.DefaultPrice,
                CurrencyId = pv.CurrencyId,
                IsActive = pv.IsActive,
                IsDefault = pv.IsDefault,
                CurrentStock = pv.StockLedgers.Sum(sl => sl.QuantityChange)
            })
            .FirstOrDefaultAsync(ct);

        if (variant is null)
            return ResultOperation<ProductVariantDto>.ToFailedResult("تنوع محصول یافت نشد");

        return ResultOperation<ProductVariantDto>.ToSuccessResult(variant);
    }
}
