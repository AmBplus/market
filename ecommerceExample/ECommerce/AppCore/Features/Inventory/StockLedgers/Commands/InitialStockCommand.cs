using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Inventory;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Inventory.StockLedgers.Commands;

public class InitialStockCommand
{
    public long ProductVariantId { get; set; }
    public long WarehouseId { get; set; }
    public int Quantity { get; set; }
}

public class InitialStockHandler
{
    private readonly ECommerceDbContext _context;
    public InitialStockHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(InitialStockCommand command, CancellationToken ct)
    {
        if (command.ProductVariantId <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        if (command.WarehouseId <= 0)
            return ResultOperation.ToFailedResult("شناسه انبار نامعتبر است");

        if (command.Quantity <= 0)
            return ResultOperation.ToFailedResult("تعداد موجودی اولیه باید بزرگتر از صفر باشد");

        var variantExists = await _context.ProductVariants.AnyAsync(pv => pv.Id == command.ProductVariantId, ct);
        if (!variantExists)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == command.WarehouseId && w.IsActive, ct);
        if (!warehouseExists)
            return ResultOperation.ToFailedResult("انبار یافت نشد یا غیرفعال است");

        var hasInitial = await _context.StockLedgers.AnyAsync(
            sl => sl.ProductVariantId == command.ProductVariantId
               && sl.WarehouseId == command.WarehouseId
               && sl.Reason == StockChangeReason.Initial, ct);

        if (hasInitial)
            return ResultOperation.ToFailedResult("موجودی اولیه قبلاً برای این تنوع و انبار ثبت شده است");

        var ledger = new StockLedger
        {
            ProductVariantId = command.ProductVariantId,
            WarehouseId = command.WarehouseId,
            QuantityChange = command.Quantity,
            Reason = StockChangeReason.Initial,
            Description = "ثبت موجودی اولیه"
        };

        _context.Add(ledger);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("موجودی اولیه با موفقیت ثبت شد");
    }
}
