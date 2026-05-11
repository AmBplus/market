using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Commands
{
    public class CouponCreateCommand
    {
        public required string Code { get; set; }

        public CouponType Type { get; set; }

        public decimal Value { get; set; }

        public decimal MinOrderAmount { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public int? UsageLimit { get; set; }

        public int? PerUserLimit { get; set; }

        public DateTime StartsAt { get; set; }

        public DateTime ExpiresAt { get; set; }
    }

    public class CouponCreateCommandHandler
    {
        private readonly AppDbContext _context;

        public CouponCreateCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<long>> Handle(CouponCreateCommand command, CancellationToken cancellationToken)
        {
            var codeExists = await _context.Coupons
                .AnyAsync(c => c.Code == command.Code && !c.IsDelete, cancellationToken);

            if (codeExists)
            {
                return ResultOperation<long>.ToFailedResult("این کد کوپن قبلاً استفاده شده است.");
            }

            var coupon = new Coupon
            {
                Code = command.Code,
                Type = command.Type,
                Value = command.Value,
                MinOrderAmount = command.MinOrderAmount,
                MaxDiscountAmount = command.MaxDiscountAmount,
                UsageLimit = command.UsageLimit,
                PerUserLimit = command.PerUserLimit,
                IsActive = true,
                IsDelete = false,
                StartsAt = command.StartsAt,
                ExpiresAt = command.ExpiresAt,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Coupons.AddAsync(coupon, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation<long>.ToSuccessResult("کوپن با موفقیت ایجاد شد.", coupon.Id);
        }
    }
}
