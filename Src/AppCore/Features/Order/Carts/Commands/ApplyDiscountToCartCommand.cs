using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Order.Carts.Commands;

public class ApplyDiscountToCartCommand
{
    public long UserId { get; set; }
    public string DiscountCode { get; set; } = string.Empty;
}

public class ApplyDiscountToCartHandler
{
    private readonly AppDbContext _context;
    public ApplyDiscountToCartHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(ApplyDiscountToCartCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.DiscountCode))
            return ResultOperation.ToFailedResult("کد تخفیف الزامی است");

        var cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == command.UserId && c.IsActive, ct);

        if (cart is null)
            return ResultOperation.ToFailedResult("سبد خرید فعال یافت نشد");

        var discount = await _context.Discounts
            .FirstOrDefaultAsync(d => d.Code == command.DiscountCode && d.IsActive, ct);

        if (discount is null)
            return ResultOperation.ToFailedResult("کد تخفیف معتبر نیست");

        var now = DateTime.UtcNow;

        if (discount.StartedAt > now)
            return ResultOperation.ToFailedResult("زمان شروع تخفیف فرا نرسیده است");

        if (discount.EndedAt.HasValue && discount.EndedAt.Value < now)
            return ResultOperation.ToFailedResult("تخفیف منقضی شده است");

        if (discount.UsageLimit.HasValue && discount.UsedCount >= discount.UsageLimit.Value)
            return ResultOperation.ToFailedResult("سقف استفاده از تخفیف تکمیل شده است");

        if (discount.PerUserLimit.HasValue)
        {
            var userUsageCount = await _context.OrderDiscounts
                .CountAsync(od => od.DiscountId == discount.Id, ct);
            if (userUsageCount >= discount.PerUserLimit.Value)
                return ResultOperation.ToFailedResult("سقف استفاده از تخفیف برای این کاربر تکمیل شده است");
        }

        cart.DiscountCode = command.DiscountCode;
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("کد تخفیف با موفقیت اعمال شد");
    }
}
