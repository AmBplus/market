using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.CouponServices.Queries.GetCouponsDataTableQuery
{
    public class CouponDataTableQuery : DatatableFullRequest
    {
        public string? SearchCode { get; set; }
    }

    public class CouponListDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string TypeName => TypeToString(Type);
        public decimal Value => FormatMoney(Value);
        public DateTime CreatedAt { get; set; }
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

    public class CouponDataTableHandler
    {
        private readonly AppDbContext _context;

        public CouponDataTableHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<DataTableResponse<CouponListDto>>> Handle(CouponDataTableQuery query, CancellationToken cancellationToken)
        {
            var baseQuery = _context.Coupons
                .AsNoTracking()
                .Where(c => !c.IsDelete && c.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchCode))
            {
                baseQuery = baseQuery.Where(c => c.Code.Contains(query.SearchCode));
            }

            var filteredCount = await baseQuery.CountAsync(cancellationToken);

            var dtoQuery = baseQuery.Select(c => new CouponListDto
            {
                Id = c.Id,
                Code = c.Code,
                Type = (int)c.Type,
                TypeName = TypeToString((int)c.Type),
                Value = c.Value,
                CreatedAt = c.CreatedAt
            });

            var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

            var coupons = await orderedQuery
                .Skip(query.start)
                .Take(query.length)
                .ToListAsync(cancellationToken);

            var response = new DataTableResponse<CouponListDto>
            {
                Draw = query.draw,
                RecordsTotal = filteredCount,
                RecordsFiltered = filteredCount,
                Data = coupons
            };
            return response.ToSuccessResult();
        }
    }
}
