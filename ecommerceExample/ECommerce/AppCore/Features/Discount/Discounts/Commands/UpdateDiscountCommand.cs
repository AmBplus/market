using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Discount;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Discount.Discounts.Commands;

public class UpdateDiscountCommand
{
    public long Id { get; set; }
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
    public bool IsActive { get; set; }
    public List<long>? ProductIds { get; set; }
    public List<long>? ProductVariantIds { get; set; }
    public List<long>? CategoryIds { get; set; }
}

public class UpdateDiscountHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateDiscountHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateDiscountCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه تخفیف نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد تخفیف الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام تخفیف الزامی است");

        if (command.Value <= 0)
            return ResultOperation.ToFailedResult("مقدار تخفیف باید بزرگتر از صفر باشد");

        if (command.DiscountType == DiscountType.Percentage && command.Value > 100)
            return ResultOperation.ToFailedResult("درصد تخفیف نمی‌تواند بیشتر از ۱۰۰ باشد");

        var discount = await _context.Discounts.FindAsync(new object[] { command.Id }, ct);
        if (discount is null)
            return ResultOperation.ToFailedResult("تخفیف یافت نشد");

        var codeExists = await _context.Discounts.AnyAsync(d => d.Code == command.Code && d.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد تخفیف تکراری است");

        if (command.EndedAt.HasValue && command.EndedAt.Value <= command.StartedAt)
            return ResultOperation.ToFailedResult("تاریخ پایان باید بعد از تاریخ شروع باشد");

        discount.Code = command.Code;
        discount.Name = command.Name;
        discount.Description = command.Description;
        discount.DiscountType = command.DiscountType;
        discount.Scope = command.Scope;
        discount.Value = command.Value;
        discount.MaxDiscountAmount = command.MaxDiscountAmount;
        discount.MinOrderAmount = command.MinOrderAmount;
        discount.UsageLimit = command.UsageLimit;
        discount.PerUserLimit = command.PerUserLimit;
        discount.StartedAt = command.StartedAt;
        discount.EndedAt = command.EndedAt;
        discount.IsActive = command.IsActive;

        // Sync product discounts
        var existingProductDiscounts = await _context.ProductDiscounts
            .Where(pd => pd.DiscountId == discount.Id)
            .ToListAsync(ct);
        _context.ProductDiscounts.RemoveRange(existingProductDiscounts);

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

        // Sync category discounts
        var existingCategoryDiscounts = await _context.CategoryDiscounts
            .Where(cd => cd.DiscountId == discount.Id)
            .ToListAsync(ct);
        _context.CategoryDiscounts.RemoveRange(existingCategoryDiscounts);

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

        return ResultOperation.ToSuccessResult("تخفیف با موفقیت ویرایش شد");
    }
}
