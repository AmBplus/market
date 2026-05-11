using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Commands
{
    public class CouponApplyCommand
    {
        public required long Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderAmount { get; set; }
    }

    public class CouponApplyCommandHandler
    {
        private readonly AppDbContext _context;

        public CouponApplyCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<decimal>> Handle(CouponApplyCommand command, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.FindAsync(command.Id, cancellationToken);

            if (coupon == null)
            {
                return ResultOperation<decimal>.ToFailedResult("کوپن مورد نظر یافت نشد.");
            }

            if (coupon.IsDelete || !coupon.IsActive)
            {
                return ResultOperation<decimal>.ToFailedResult("این کوپن در حال حاضر فعال نیست.");
            }

            var now = DateTime.UtcNow;
            if (command.OrderDate < coupon.StartsAt || command.OrderDate > coupon.ExpiresAt)
            {
                return ResultOperation<decimal>.ToFailedResult("این کوپن در تاریخ جاری قابل استفاده نمی‌باشد.");
            }

            var minOrderMet = command.OrderAmount >= coupon.MinOrderAmount;
            if (!minOrderMet)
            {
                return ResultOperation<decimal>.ToFailedResult($"مبلغ سفارش باید حداقل {coupon.MinOrderAmount} باشد.");
            }

            decimal discountAmount = 0;

            switch (coupon.Type)
            {
                case CouponType.Percentage:
                    discountAmount = coupon.Value / 100 * command.OrderAmount;
                    break;

                case CouponType.FixedAmount:
                    discountAmount = coupon.Value;
                    if (coupon.MaxDiscountAmount.HasValue && discountAmount > coupon.MaxDiscountAmount)
                    {
                        discountAmount = coupon.MaxDiscountAmount.Value;
                    }
                    break;

                case CouponType.FreeShipping:
                    // Assuming there's a FreeShippingDiscount or similar - for now returning 0 as placeholder
                    return ResultOperation<decimal>.ToSuccessResult("هزینه ارسال رایگان اعمال شد.", 0);
            }

            coupon.UsageCount++;
            if (coupon.UsageLimit.HasValue && coupon.UsageCount > coupon.UsageLimit)
            {
                await _context.SaveChangesAsync(cancellationToken);
                return ResultOperation<decimal>.ToFailedResult("این کوپن از حد نصاب مصرف‌ها گذراده است.");
            }

            if (coupon.PerUserLimit.HasValue)
            {
                var userCoupons = await _context.Coupons.CountAsync(
                    c => c.Id == coupon.Id && !c.IsDelete && c.UsageCount >= coupon.PerUserLimit,
                    cancellationToken);
                if (userCoupons > 0)
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    return ResultOperation<decimal>.ToFailedResult("این کوپن از حد نصاب مصرف کاربران گذراده است.");
                }
            }

            coupon.UpdatedAt = DateTime.UtcNow;
            coupon.UpdatedBy = command.UserId ?? 1;

            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation<decimal>.ToSuccessResult($"تخفیف {discountAmount} مبلغ اعمال شد.", discountAmount);
        }
    }
}
