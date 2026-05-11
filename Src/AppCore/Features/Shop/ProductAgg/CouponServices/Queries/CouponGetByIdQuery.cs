using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Queries.GetCouponByIdQuery
{
    public class CouponGetByIdQuery
    {
        public required long Id { get; set; }
    }

    public class CouponDetailDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public decimal Value { get; set; }
        public decimal MinOrderAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }
        public int? PerUserLimit { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class CouponGetByIdQueryHandler
    {
        private readonly AppDbContext _context;

        public CouponGetByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<CouponDetailDto>> Handle(CouponGetByIdQuery query, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.FindAsync(query.Id, cancellationToken);
            if (coupon == null)
            {
                return ResultOperation<CouponDetailDto>.ToFailedResult("کوپن مورد نظر یافت نشد.");
            }

            var couponDto = new CouponDetailDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Type = (int)coupon.Type,
                Value = coupon.Value,
                MinOrderAmount = coupon.MinOrderAmount,
                MaxDiscountAmount = coupon.MaxDiscountAmount,
                UsageLimit = coupon.UsageLimit,
                UsageCount = coupon.UsageCount,
                PerUserLimit = coupon.PerUserLimit,
                IsActive = coupon.IsActive,
                StartsAt = coupon.StartsAt,
                ExpiresAt = coupon.ExpiresAt
            };
            return couponDto.ToSuccessResult();
        }
    }
}
