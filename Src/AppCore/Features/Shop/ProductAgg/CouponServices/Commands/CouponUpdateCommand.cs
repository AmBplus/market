using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Commands
{
    public class CouponUpdateCommand
    {
        public required long Id { get; set; }

        public string? Code { get; set; }

        public CouponType? Type { get; set; }

        public decimal? Value { get; set; }

        public decimal? MinOrderAmount { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public int? UsageLimit { get; set; }

        public int? PerUserLimit { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? StartsAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public long? UserId { get; set; }
    }

    public class CouponUpdateCommandHandler
    {
        private readonly AppDbContext _context;

        public CouponUpdateCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(CouponUpdateCommand command, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.FindAsync(command.Id, cancellationToken);

            if (coupon == null)
            {
                return ResultOperation.ToFailedResult("کوپن مورد نظر یافت نشد.");
            }

            coupon.Code = command.Code;
            coupon.Type = command.Type ?? coupon.Type;
            coupon.Value = command.Value ?? coupon.Value;
            coupon.MinOrderAmount = command.MinOrderAmount ?? coupon.MinOrderAmount;
            coupon.MaxDiscountAmount = command.MaxDiscountAmount ?? coupon.MaxDiscountAmount;
            coupon.UsageLimit = command.UsageLimit ?? coupon.UsageLimit;
            coupon.PerUserLimit = command.PerUserLimit ?? coupon.PerUserLimit;
            coupon.IsActive = command.IsActive ?? coupon.IsActive;
            coupon.StartsAt = command.StartsAt ?? coupon.StartsAt;
            coupon.ExpiresAt = command.ExpiresAt ?? coupon.ExpiresAt;
            coupon.UpdatedAt = DateTime.UtcNow;
            coupon.UpdatedBy = command.UserId;

            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult("کوپن با موفقیت ویرایش شد.");
        }
    }
}
