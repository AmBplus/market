using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Discount.Discounts.Queries;

public class GetDiscountByIdQuery
{
    public long Id { get; set; }
}

public class DiscountDto
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
    public int UsedCount { get; set; }
    public int? PerUserLimit { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool IsActive { get; set; }
    public List<long> ProductIds { get; set; } = new();
    public List<long> CategoryIds { get; set; } = new();
}

public class GetDiscountByIdHandler
{
    private readonly AppDbContext _context;
    public GetDiscountByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DiscountDto>> Handle(GetDiscountByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<DiscountDto>.ToFailedResult("شناسه تخفیف نامعتبر است");

        var discount = await _context.Discounts
            .AsNoTracking()
            .Where(d => d.Id == query.Id)
            .Select(d => new DiscountDto
            {
                Id = d.Id,
                Code = d.Code,
                Name = d.Name,
                Description = d.Description,
                DiscountType = d.DiscountType,
                Scope = d.Scope,
                Value = d.Value,
                MaxDiscountAmount = d.MaxDiscountAmount,
                MinOrderAmount = d.MinOrderAmount,
                UsageLimit = d.UsageLimit,
                UsedCount = d.UsedCount,
                PerUserLimit = d.PerUserLimit,
                StartedAt = d.StartedAt,
                EndedAt = d.EndedAt,
                IsActive = d.IsActive,
                ProductIds = d.ProductDiscounts.Select(pd => pd.ProductId).ToList(),
                CategoryIds = d.CategoryDiscounts.Select(cd => cd.CategoryId).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (discount is null)
            return ResultOperation<DiscountDto>.ToFailedResult("تخفیف یافت نشد");

        return ResultOperation<DiscountDto>.ToSuccessResult(discount);
    }
}
