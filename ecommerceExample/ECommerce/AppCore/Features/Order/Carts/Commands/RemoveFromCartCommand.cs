using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Order.Carts.Commands;

public class RemoveFromCartCommand
{
    public long UserId { get; set; }
    public long CartItemId { get; set; }
}

public class RemoveFromCartHandler
{
    private readonly ECommerceDbContext _context;
    public RemoveFromCartHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(RemoveFromCartCommand command, CancellationToken ct)
    {
        if (command.UserId <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        if (command.CartItemId <= 0)
            return ResultOperation.ToFailedResult("شناسه آیتم سبد خرید نامعتبر است");

        var cartItem = await _context.CartItems
            .Include(ci => ci.Cart)
            .FirstOrDefaultAsync(ci => ci.Id == command.CartItemId && ci.Cart.UserId == command.UserId, ct);

        if (cartItem is null)
            return ResultOperation.ToFailedResult("آیتم سبد خرید یافت نشد");

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("آیتم با موفقیت از سبد خرید حذف شد");
    }
}
