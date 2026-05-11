using AppCore.Data;
using AppCore.Domains.Order;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Order.Carts.Commands;

public class AddToCartCommand
{
    public long UserId { get; set; }
    public long ProductVariantId { get; set; }
    public int Quantity { get; set; }
}

public class AddToCartHandler
{
    private readonly AppDbContext _context;
    public AddToCartHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(AddToCartCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (command.ProductVariantId <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        if (command.Quantity <= 0)
            return ResultOperation.ToFailedResult("تعداد باید بزرگتر از صفر باشد");

        var variantExists = await _context.ProductVariants.AnyAsync(pv => pv.Id == command.ProductVariantId, ct);
        if (!variantExists)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        // Find or create active cart
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == command.UserId && c.IsActive, ct);

        if (cart is null)
        {
            cart = new Cart
            {
                UserId = command.UserId,
                IsActive = true
            };
            _context.Add(cart);
            await _context.SaveChangesAsync(ct);
        }

        // Find or create cart item
        var cartItem = cart.Items.FirstOrDefault(ci => ci.ProductVariantId == command.ProductVariantId);

        if (cartItem is not null)
        {
            cartItem.Quantity += command.Quantity;
        }
        else
        {
            var price = await _context.Prices
                .Where(p => p.ProductVariantId == command.ProductVariantId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(ct);

            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductVariantId = command.ProductVariantId,
                Quantity = command.Quantity,
                UnitPrice = price?.Amount ?? 0,
                CurrencyId = price?.CurrencyId ?? 1
            };
            _context.Add(cartItem);
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("محصول با موفقیت به سبد خرید اضافه شد");
    }
}
