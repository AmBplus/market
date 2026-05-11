using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Product.ProductVariants.Commands;

public class UpdateProductVariantCommand
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string VariantCode { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? VariantAttributesJson { get; set; }
    public long? VendorId { get; set; }
    public decimal? CostPrice { get; set; }
    public decimal DefaultPrice { get; set; }
    public long CurrencyId { get; set; }
    public decimal? WeightGrams { get; set; }
    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
}

public class UpdateProductVariantHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateProductVariantHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateProductVariantCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.VariantCode))
            return ResultOperation.ToFailedResult("کد تنوع الزامی است");

        if (string.IsNullOrWhiteSpace(command.Sku))
            return ResultOperation.ToFailedResult("SKU الزامی است");

        if (string.IsNullOrWhiteSpace(command.Title))
            return ResultOperation.ToFailedResult("عنوان تنوع الزامی است");

        if (command.DefaultPrice < 0)
            return ResultOperation.ToFailedResult("قیمت پیش‌فرض نمی‌تواند منفی باشد");

        var variant = await _context.ProductVariants.FindAsync(new object[] { command.Id }, ct);
        if (variant is null)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var codeExists = await _context.ProductVariants.AnyAsync(pv => pv.VariantCode == command.VariantCode && pv.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد تنوع تکراری است");

        var skuExists = await _context.ProductVariants.AnyAsync(pv => pv.Sku == command.Sku && pv.Id != command.Id, ct);
        if (skuExists)
            return ResultOperation.ToFailedResult("SKU تکراری است");

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == command.CurrencyId, ct);
        if (!currencyExists)
            return ResultOperation.ToFailedResult("واحد پول انتخاب شده معتبر نیست");

        variant.ProductId = command.ProductId;
        variant.VariantCode = command.VariantCode;
        variant.Sku = command.Sku;
        variant.Title = command.Title;
        variant.VariantAttributesJson = command.VariantAttributesJson;
        variant.VendorId = command.VendorId;
        variant.CostPrice = command.CostPrice;
        variant.DefaultPrice = command.DefaultPrice;
        variant.CurrencyId = command.CurrencyId;
        variant.WeightGrams = command.WeightGrams;
        variant.IsActive = command.IsActive;
        variant.IsDefault = command.IsDefault;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تنوع محصول با موفقیت ویرایش شد");
    }
}
