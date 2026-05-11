using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Discount.Discounts.Commands;

public class DeleteDiscountCommand
{
    public long Id { get; set; }
}

public class DeleteDiscountHandler
{
    private readonly AppDbContext _context;
    public DeleteDiscountHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteDiscountCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه تخفیف نامعتبر است");

        var discount = await _context.Discounts.FindAsync(new object[] { command.Id }, ct);
        if (discount is null)
            return ResultOperation.ToFailedResult("تخفیف یافت نشد");

        var hasOrders = await _context.OrderDiscounts.AnyAsync(od => od.DiscountId == command.Id, ct);
        if (hasOrders)
            return ResultOperation.ToFailedResult("این تخفیف در سفارشات استفاده شده و قابل حذف نیست");

        // Remove associated product discounts
        var productDiscounts = await _context.ProductDiscounts
            .Where(pd => pd.DiscountId == command.Id)
            .ToListAsync(ct);
        _context.ProductDiscounts.RemoveRange(productDiscounts);

        // Remove associated category discounts
        var categoryDiscounts = await _context.CategoryDiscounts
            .Where(cd => cd.DiscountId == command.Id)
            .ToListAsync(ct);
        _context.CategoryDiscounts.RemoveRange(categoryDiscounts);

        _context.Discounts.Remove(discount);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تخفیف با موفقیت حذف شد");
    }
}
