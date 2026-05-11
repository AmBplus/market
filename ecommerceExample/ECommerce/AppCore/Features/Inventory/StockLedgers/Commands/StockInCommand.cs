using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Inventory.StockLedgers.Commands;

public class StockInCommand
{
    public long ProductVariantId { get; set; }
    public long WarehouseId { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
}

public class StockInHandler
{
    private readonly ECommerceDbContext _context;
    public StockInHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(StockInCommand command, CancellationToken ct)
    {
        if (command.ProductVariantId <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        if (command.WarehouseId <= 0)
            return ResultOperation.ToFailedResult("شناسه انبار نامعتبر است");

        if (command.Quantity <= 0)
            return ResultOperation.ToFailedResult("تعداد ورودی باید بزرگتر از صفر باشد");

        var variantExists = await _context.ProductVariants.AnyAsync(pv => pv.Id == command.ProductVariantId, ct);
        if (!variantExists)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == command.WarehouseId && w.IsActive, ct);
        if (!warehouseExists)
            return ResultOperation.ToFailedResult("انبار یافت نشد یا غیرفعال است");

        var ledger = new StockLedger
        {
            ProductVariantId = command.ProductVariantId,
            WarehouseId = command.WarehouseId,
            QuantityChange = command.Quantity,
            Reason = StockChangeReason.StockIn,
            Description = command.Description
        };

        _context.Add(ledger);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("ورودی موجودی با موفقیت ثبت شد");
    }
}
