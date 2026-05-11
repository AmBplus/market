using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Carts.Commands;

public class UpdateCartItemQuantityCommand
{
    public long UserId { get; set; }
    public long CartItemId { get; set; }
    public int NewQuantity { get; set; }
}

public class UpdateCartItemQuantityHandler
{
    private readonly ECommerceDbContext _context;
    public UpdateCartItemQuantityHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateCartItemQuantityCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (command.CartItemId <= 0)
            return ResultOperation.ToFailedResult("شناسه آیتم سبد خرید نامعتبر است");

        if (command.NewQuantity < 0)
            return ResultOperation.ToFailedResult("تعداد نمی‌تواند منفی باشد");

        var cartItem = await _context.CartItems
            .Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == command.CartItemId && ci.Cart.UserId == command.UserId, ct);

        if (cartItem is null)
            return ResultOperation.ToFailedResult("آیتم سبد خرید یافت نشد");

        if (command.NewQuantity == 0)
        {
            _context.CartItems.Remove(cartItem);
        }
        else
        {
            cartItem.Quantity = command.NewQuantity;
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تعداد آیتم سبد خرید با موفقیت بروزرسانی شد");
    }
}
