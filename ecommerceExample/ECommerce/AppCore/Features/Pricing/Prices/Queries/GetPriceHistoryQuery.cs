using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Pricing.Prices.Queries;

public class GetPriceHistoryQuery
{
    public long ProductVariantId { get; set; }
    public PriceType? PriceType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class GetPriceHistoryHandler
{
    private readonly ECommerceDbContext _context;
    public GetPriceHistoryHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<PriceDto>>> Handle(GetPriceHistoryQuery query, CancellationToken ct)
    {
        if (query.ProductVariantId <= 0)
            return ResultOperation<List<PriceDto>>.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        var baseQuery = _context.Prices
            .AsNoTracking()
            .Where(p => p.ProductVariantId == query.ProductVariantId);

        if (query.PriceType.HasValue)
            baseQuery = baseQuery.Where(p => p.PriceType == query.PriceType.Value);

        if (query.FromDate.HasValue)
            baseQuery = baseQuery.Where(p => p.StartedAt >= query.FromDate.Value);

        if (query.ToDate.HasValue)
            baseQuery = baseQuery.Where(p => p.StartedAt <= query.ToDate.Value);

        var prices = await baseQuery
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
