using AppCore.Data;
using AppCore.Domains.Discount;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Discount.Discounts.Commands;

public class CreateDiscountCommand
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DiscountType DiscountType { get; set; }
    public DiscountScope Scope { get; set; }
    public decimal Value { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal? MinOrderAmount { get; set; }
    public int? UsageLimit { get; set; }
    public int? PerUserLimit { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public List<long>? ProductIds { get; set; }
    public List<long>? ProductVariantIds { get; set; }
    public List<long>? CategoryIds { get; set; }
}

public class CreateDiscountHandler
{
    private readonly AppDbContext _context;
    public CreateDiscountHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateDiscountCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد تخفیف الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام تخفیف الزامی است");

        if (command.Value <= 0)
            return ResultOperation.ToFailedResult("مقدار تخفیف باید بزرگتر از صفر باشد");

        if (command.DiscountType == DiscountType.Percentage && command.Value > 100)
            return ResultOperation.ToFailedResult("درصد تخفیف نمی‌تواند بیشتر از ۱۰۰ باشد");

        var codeExists = await _context.Discounts.AnyAsync(d => d.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد تخفیف تکراری است");

        if (command.EndedAt.HasValue && command.EndedAt.Value <= command.StartedAt)
            return ResultOperation.ToFailedResult("تاریخ پایان باید بعد از تاریخ شروع باشد");

        var discount = new Domains.Discount.Discount
        {
            Code = command.Code,
            Name = command.Name,
            Description = command.Description,
            DiscountType = command.DiscountType,
            Scope = command.Scope,
            Value = command.Value,
            MaxDiscountAmount = command.MaxDiscountAmount,
            MinOrderAmount = command.MinOrderAmount,
            UsageLimit = command.UsageLimit,
            PerUserLimit = command.PerUserLimit,
            StartedAt = command.StartedAt,
            EndedAt = command.EndedAt,
            IsActive = command.IsActive
        };

        _context.Add(discount);
        await _context.SaveChangesAsync(ct);

        // Add product discounts
        if (command.ProductIds is { Count: > 0 })
        {
            foreach (var productId in command.ProductIds)
            {
                _context.Add(new ProductDiscount
                {
                    DiscountId = discount.Id,
                    ProductId = productId
                });
            }
        }

        // Add product variant discounts
        if (command.ProductVariantIds is { Count: > 0 })
        {
            foreach (var variantId in command.ProductVariantIds)
            {
                _context.Add(new ProductDiscount
                {
                    DiscountId = discount.Id,
                    ProductId = 0,
                    ProductVariantId = variantId
                });
            }
        }

        // Add category discounts
        if (command.CategoryIds is { Count: > 0 })
        {
            foreach (var categoryId in command.CategoryIds)
            {
                _context.Add(new CategoryDiscount
                {
                    DiscountId = discount.Id,
                    CategoryId = categoryId
                });
            }
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تخفیف با موفقیت ایجاد شد");
    }
}
