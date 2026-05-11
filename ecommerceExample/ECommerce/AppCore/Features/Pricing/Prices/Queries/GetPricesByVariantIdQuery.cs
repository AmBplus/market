using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Pricing.Prices.Queries;

public class GetPricesByVariantIdQuery
{
    public long ProductVariantId { get; set; }
}

public class PriceDto
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public ECommerce.AppCore.Enums.PriceType PriceType { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int? MinQuantity { get; set; }
}

public class GetPricesByVariantIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetPricesByVariantIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<PriceDto>>> Handle(GetPricesByVariantIdQuery query, CancellationToken ct)
    {
        if (query.ProductVariantId <= 0)
            return ResultOperation<List<PriceDto>>.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        var variantExists = await _context.ProductVariants
            .AnyAsync(pv => pv.Id == query.ProductVariantId && !pv.IsDeleted, ct);
        if (!variantExists)
            return ResultOperation<List<PriceDto>>.ToFailedResult("تنوع محصول یافت نشد");

        var prices = await _context.Prices
            .AsNoTracking()
            .Where(p => p.ProductVariantId == query.ProductVariantId)
            .OrderByDescending(p => p.StartedAt)
            .Select(p => new PriceDto
            {
                Id = p.Id,
                Amount = p.Amount,
                CurrencyCode = p.Currency.Code,
                PriceType = p.PriceType,
                StartedAt = p.StartedAt,
                EndedAt = p.EndedAt,
                MinQuantity = p.MinQuantity
            })
            .ToListAsync(ct);

        return ResultOperation<List<PriceDto>>.ToSuccessResult(prices);
    }
}
