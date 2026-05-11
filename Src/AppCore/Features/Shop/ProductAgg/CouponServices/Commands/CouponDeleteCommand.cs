using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Commands
{
    public class CouponDeleteCommand
    {
        public required long Id { get; set; }

        public required long UserId { get; set; }
    }

    public class CouponDeleteCommandHandler
    {
        private readonly AppDbContext _context;

        public CouponDeleteCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation> Handle(CouponDeleteCommand command, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.FindAsync(command.Id, cancellationToken);

            if (coupon == null)
            {
                return ResultOperation.ToFailedResult("کوپن مورد نظر یافت نشد.");
            }

            coupon.IsDelete = true;
            coupon.IsActive = false;
            coupon.UpdatedAt = DateTime.UtcNow;
            coupon.UpdatedBy = command.UserId;

            await _context.SaveChangesAsync(cancellationToken);

            return ResultOperation.ToSuccessResult("کوپن با موفقیت حذف شد.");
        }
    }
}
