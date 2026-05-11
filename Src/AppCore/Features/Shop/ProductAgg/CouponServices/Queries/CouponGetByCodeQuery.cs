using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Queries.GetCouponByCodeQuery
{
    public class CouponGetByCodeQuery
    {
        public required string Code { get; set; }
    }

    public class CouponGetByCodeDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public decimal Value { get; set; }
        public decimal MinOrderAmount { get; set; }
        public bool IsActive { get; set; }
    }

    public class CouponGetByCodeQueryHandler
    {
        private readonly AppDbContext _context;

        public CouponGetByCodeQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<CouponGetByCodeDto>> Handle(CouponGetByCodeQuery query, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == query.Code && !c.IsDelete, cancellationToken);

            if (coupon == null)
            {
                return ResultOperation<CouponGetByCodeDto>.ToFailedResult("کوپن با این کد یافت نشد.");
            }

            var couponDto = new CouponGetByCodeDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Type = (int)coupon.Type,
                Value = coupon.Value,
                MinOrderAmount = coupon.MinOrderAmount,
                IsActive = coupon.IsActive
            };
            return couponDto.ToSuccessResult();
        }
    }
}
