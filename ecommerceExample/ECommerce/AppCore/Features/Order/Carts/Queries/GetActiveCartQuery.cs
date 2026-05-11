using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Carts.Queries;

public class GetActiveCartQuery
{
    public long UserId { get; set; }
}

public class CartItemDto
{
    public long ProductVariantId { get; set; }
    public string VariantCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class CartDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string? DiscountCode { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class GetActiveCartHandler
{
    private readonly ECommerceDbContext _context;
    public GetActiveCartHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<CartDto>> Handle(GetActiveCartQuery query, CancellationToken ct)
    {
        if (query.UserId <= 0)
            return ResultOperation<CartDto>.ToFailedResult("شناسه کاربر نامعتبر است");

        var cart = await _context.Carts
            .AsNoTracking()
            .Where(c => c.UserId == query.UserId && c.IsActive)
            .Select(c => new CartDto
            {
                Id = c.Id,
                UserId = c.UserId,
                DiscountCode = c.DiscountCode,
                Items = c.Items.Select(ci => new CartItemDto
                {
                    ProductVariantId = ci.ProductVariantId,
                    VariantCode = ci.ProductVariant.VariantCode,
                    Title = ci.ProductVariant.Title,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (cart is null)
            return ResultOperation<CartDto>.ToFailedResult("سبد خرید فعال یافت نشد");

        return ResultOperation<CartDto>.ToSuccessResult(cart);
    }
}
