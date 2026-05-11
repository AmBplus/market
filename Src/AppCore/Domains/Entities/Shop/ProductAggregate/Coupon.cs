using AppCore.Domains.Entities.Base;

namespace AppCore.Domains.Entities.Shop.ProductAgg;

public class Coupon : BaseEntity
{
    public required string Code { get; set; }

    public CouponType Type { get; set; }

    public decimal Value { get; set; }

    public decimal MinOrderAmount { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public int? UsageLimit { get; set; }

    public int UsageCount { get; set; }

    public int? PerUserLimit { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime StartsAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsDelete { get; set; } = false;
}
