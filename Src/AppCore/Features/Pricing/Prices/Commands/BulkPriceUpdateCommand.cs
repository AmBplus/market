using AppCore.Data;
using AppCore.Domains.Pricing;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Pricing.Prices.Commands;

public class BulkPriceUpdateCommand
{
    public long? CategoryId { get; set; }
    public List<long>? ProductIds { get; set; }
    public decimal PercentageIncrease { get; set; }
    public PriceType PriceType { get; set; }
    public long CurrencyId { get; set; }
}

public class BulkPriceUpdateHandler
{
    private readonly AppDbContext _context;
    public BulkPriceUpdateHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(BulkPriceUpdateCommand command, CancellationToken ct)
    {
        if (command.PercentageIncrease <= 0)
            return ResultOperation.ToFailedResult("درصد افزایش باید بزرگتر از صفر باشد");

        if (command.CurrencyId <= 0)
            return ResultOperation.ToFailedResult("شناسه واحد پول الزامی است");

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == command.CurrencyId, ct);
        if (!currencyExists)
            return ResultOperation.ToFailedResult("واحد پول یافت نشد");

        // Find matching product variant IDs
        IQueryable<long> productQuery = _context.Products
            .Where(p => !p.IsDeleted && p.IsActive)
            .Select(p => p.Id);

        if (command.CategoryId.HasValue)
        {
            productQuery = _context.ProductCategories
                .Where(pc => pc.CategoryId == command.CategoryId.Value)
                .Select(pc => pc.ProductId);
        }

        if (command.ProductIds is { Count: > 0 })
        {
            var filteredIds = await productQuery.ToListAsync(ct);
            var targetIds = command.ProductIds.Intersect(filteredIds).ToList();
            productQuery = targetIds.AsQueryable();
        }

        var productIds = await productQuery.ToListAsync(ct);

        if (productIds.Count == 0)
            return ResultOperation.ToFailedResult("محصولی برای بروزرسانی قیمت یافت نشد");

        var variantIds = await _context.ProductVariants
            .Where(pv => productIds.Contains(pv.ProductId) && !pv.IsDeleted && pv.IsActive)
            .Select(pv => pv.Id)
            .ToListAsync(ct);

        if (variantIds.Count == 0)
            return ResultOperation.ToFailedResult("تنوع محصولی برای بروزرسانی قیمت یافت نشد");

        // Close active prices of the specified type for these variants
        var activePrices = await _context.Prices
            .Where(p => variantIds.Contains(p.ProductVariantId) &&
                        p.PriceType == command.PriceType &&
                        p.EndedAt == null)
            .ToListAsync(ct);

        var now = DateTime.UtcNow;

        foreach (var activePrice in activePrices)
        {
            activePrice.EndedAt = now;

            var newAmount = activePrice.Amount + (activePrice.Amount * command.PercentageIncrease / 100m);

            var newPrice = new Price
            {
                ProductVariantId = activePrice.ProductVariantId,
                Amount = newAmount,
                CurrencyId = command.CurrencyId,
                PriceType = command.PriceType,
                StartedAt = now,
                MinQuantity = activePrice.MinQuantity
            };

            _context.Add(newPrice);
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult($"قیمت {activePrices.Count} تنوع محصول با موفقیت بروزرسانی شد");
    }
}
