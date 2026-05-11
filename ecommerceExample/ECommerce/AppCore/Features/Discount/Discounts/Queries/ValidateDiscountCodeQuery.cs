using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Discount.Discounts.Queries;

public class ValidateDiscountCodeQuery
{
    public string Code { get; set; } = string.Empty;
    public long? UserId { get; set; }
    public decimal? OrderAmount { get; set; }
}

public class ValidatedDiscountDto
{
    public bool IsValid { get; set; }
    public long DiscountId { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal Value { get; set; }
    public decimal? MaxDiscountAmount { get; set; }
    public decimal CalculatedDiscountAmount { get; set; }
}

public class ValidateDiscountCodeHandler
{
    private readonly ECommerceDbContext _context;
    public ValidateDiscountCodeHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<ValidatedDiscountDto>> Handle(ValidateDiscountCodeQuery query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.Code))
            return ResultOperation<ValidatedDiscountDto>.ToFailedResult("کد تخفیف الزامی است");

        var discount = await _context.Discounts
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Code == query.Code, ct);

        if (discount is null)
            return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });

        var now = DateTime.UtcNow;

        // Check basic validity
        if (!discount.IsActive)
            return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });

        if (discount.StartedAt > now)
            return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });

        if (discount.EndedAt.HasValue && discount.EndedAt.Value < now)
            return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });

        if (discount.UsageLimit.HasValue && discount.UsedCount >= discount.UsageLimit.Value)
            return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });

        // Check per user limit
        if (discount.PerUserLimit.HasValue && query.UserId.HasValue)
        {
            var userUsageCount = await _context.OrderDiscounts
                .CountAsync(od => od.DiscountId == discount.Id, ct);
            if (userUsageCount >= discount.PerUserLimit.Value)
                return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });
        }

        // Check minimum order amount
        if (discount.MinOrderAmount.HasValue && query.OrderAmount.HasValue)
        {
            if (query.OrderAmount.Value < discount.MinOrderAmount.Value)
                return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(new ValidatedDiscountDto { IsValid = false });
        }

        // Calculate discount amount
        var calculatedAmount = 0m;
        if (query.OrderAmount.HasValue)
        {
            calculatedAmount = discount.DiscountType == DiscountType.Percentage
                ? query.OrderAmount.Value * discount.Value / 100
                : discount.DiscountType == DiscountType.FixedAmount
                    ? discount.Value
                    : 0;

            if (discount.MaxDiscountAmount.HasValue)
                calculatedAmount = Math.Min(calculatedAmount, discount.MaxDiscountAmount.Value);
        }

        var result = new ValidatedDiscountDto
        {
            IsValid = true,
            DiscountId = discount.Id,
            DiscountType = discount.DiscountType,
            Value = discount.Value,
            MaxDiscountAmount = discount.MaxDiscountAmount,
            CalculatedDiscountAmount = calculatedAmount
        };

        return ResultOperation<ValidatedDiscountDto>.ToSuccessResult(result);
    }
}
