using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Queries.GetCouponsSelect2Query
{
    public class CouponSelect2Query
    {
        public DateTime? MinDate { get; set; }

        public DateTime? MaxDate { get; set; }
    }

    public class CouponSelect2Dto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string TypeName => TypeToString(Type);
        public decimal Value => FormatMoney(Value);
        public DateTime StartsAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    private static string TypeToString(int type)
    {
        return type switch
        {
            1 => "درصدی",
            2 => "مبلغ ثابت",
            3 => "ارسال رایگان",
            _ => "نامشخص"
        };
    }

    private static string FormatMoney(decimal amount)
    {
        return $"{amount:F0} تومان";
    }

    public class CouponSelect2QueryHandler
    {
        private readonly AppDbContext _context;

        public CouponSelect2QueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<List<CouponSelect2Dto>>> Handle(CouponSelect2Query query, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var baseQuery = _context.Coupons
                .AsNoTracking()
                .Where(c => !c.IsDelete && c.IsActive)
                .Where(c => (query.MinDate == null || c.StartsAt <= query.MinDate.Value))
                .Where(c => (query.MaxDate == null || c.ExpiresAt >= query.MaxDate.Value));

            if (!string.IsNullOrWhiteSpace(query.SearchCode))
            {
                baseQuery = baseQuery.Where(c => c.Code.Contains(query.SearchCode));
            }

            var coupons = await baseQuery.Select(c => new CouponSelect2Dto
            {
                Id = c.Id,
                Code = c.Code,
                Type = (int)c.Type,
                TypeName = TypeToString((int)c.Type),
                Value = c.Value,
                StartsAt = c.StartsAt,
                ExpiresAt = c.ExpiresAt
            }).ToListAsync(cancellationToken);

            return ResultOperation<List<CouponSelect2Dto>>.ToSuccessResult(coupons);
        }
    }
}
