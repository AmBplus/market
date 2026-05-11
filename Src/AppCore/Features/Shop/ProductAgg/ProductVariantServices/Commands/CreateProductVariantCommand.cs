using AppCore.Data;
using AppCore.Domains.Product;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.ProductVariants.Commands;

public class CreateProductVariantCommand
{
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
    public bool IsActive { get; set; } = true;
    public bool IsDefault { get; set; }
}

public class CreateProductVariantHandler
{
    private readonly AppDbContext _context;
    public CreateProductVariantHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateProductVariantCommand command, CancellationToken ct)
    {
        if (command.ProductId <= 0)
            return ResultOperation.ToFailedResult("شناسه محصول الزامی است");

        if (string.IsNullOrWhiteSpace(command.VariantCode))
            return ResultOperation.ToFailedResult("کد تنوع الزامی است");

        if (string.IsNullOrWhiteSpace(command.Sku))
            return ResultOperation.ToFailedResult("SKU الزامی است");

        if (string.IsNullOrWhiteSpace(command.Title))
            return ResultOperation.ToFailedResult("عنوان تنوع الزامی است");

        if (command.DefaultPrice < 0)
            return ResultOperation.ToFailedResult("قیمت پیش‌فرض نمی‌تواند منفی باشد");

        var productExists = await _context.Products.AnyAsync(p => p.Id == command.ProductId && !p.IsDeleted, ct);
        if (!productExists)
            return ResultOperation.ToFailedResult("محصول یافت نشد");

        var codeExists = await _context.ProductVariants.AnyAsync(pv => pv.VariantCode == command.VariantCode, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد تنوع تکراری است");

        var skuExists = await _context.ProductVariants.AnyAsync(pv => pv.Sku == command.Sku, ct);
        if (skuExists)
            return ResultOperation.ToFailedResult("SKU تکراری است");

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == command.CurrencyId, ct);
        if (!currencyExists)
            return ResultOperation.ToFailedResult("واحد پول انتخاب شده معتبر نیست");

        var variant = new ProductVariant
        {
            ProductId = command.ProductId,
            VariantCode = command.VariantCode,
            Sku = command.Sku,
            Title = command.Title,
            VariantAttributesJson = command.VariantAttributesJson,
            VendorId = command.VendorId,
            CostPrice = command.CostPrice,
            DefaultPrice = command.DefaultPrice,
            CurrencyId = command.CurrencyId,
            WeightGrams = command.WeightGrams,
            IsActive = command.IsActive,
            IsDefault = command.IsDefault
        };

        _context.Add(variant);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تنوع محصول با موفقیت ایجاد شد");
    }
}
