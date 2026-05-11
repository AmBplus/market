using AppCore.Data;
using AppCore.Domains.Pricing;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Pricing.Prices.Commands;

public class CreatePriceCommand
{
    public long ProductVariantId { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public PriceType PriceType { get; set; }
    public DateTime StartedAt { get; set; }
    public int? MinQuantity { get; set; }
}

public class CreatePriceHandler
{
    private readonly AppDbContext _context;
    public CreatePriceHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreatePriceCommand command, CancellationToken ct)
    {
        if (command.ProductVariantId <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع محصول الزامی است");

        if (command.Amount < 0)
            return ResultOperation.ToFailedResult("مبلغ قیمت نمی‌تواند منفی باشد");

        if (command.CurrencyId <= 0)
            return ResultOperation.ToFailedResult("شناسه واحد پول الزامی است");

        var variantExists = await _context.ProductVariants
            .AnyAsync(pv => pv.Id == command.ProductVariantId && !pv.IsDeleted, ct);
        if (!variantExists)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == command.CurrencyId, ct);
        if (!currencyExists)
            return ResultOperation.ToFailedResult("واحد پول یافت نشد");

        // Close previous active price of the same type for this variant
        var activePrice = await _context.Prices
            .FirstOrDefaultAsync(p =>
                p.ProductVariantId == command.ProductVariantId &&
                p.PriceType == command.PriceType &&
                p.EndedAt == null, ct);

        if (activePrice is not null)
        {
            activePrice.EndedAt = DateTime.UtcNow;
        }

        var price = new Price
        {
            ProductVariantId = command.ProductVariantId,
            Amount = command.Amount,
            CurrencyId = command.CurrencyId,
            PriceType = command.PriceType,
            StartedAt = command.StartedAt == default ? DateTime.UtcNow : command.StartedAt,
            MinQuantity = command.MinQuantity
        };

        _context.Add(price);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("قیمت با موفقیت ایجاد شد");
    }
}
