using AppCore.Data;
using AppCore.Domains.Inventory;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Inventory.StockLedgers.Commands;

public class AdjustStockCommand
{
    public long ProductVariantId { get; set; }
    public long WarehouseId { get; set; }
    public int NewQuantity { get; set; }
    public string? Description { get; set; }
}

public class AdjustStockHandler
{
    private readonly AppDbContext _context;
    public AdjustStockHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(AdjustStockCommand command, CancellationToken ct)
    {
        if (command.ProductVariantId <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        if (command.WarehouseId <= 0)
            return ResultOperation.ToFailedResult("شناسه انبار نامعتبر است");

        if (command.NewQuantity < 0)
            return ResultOperation.ToFailedResult("موجودی جدید نمی‌تواند منفی باشد");

        var variantExists = await _context.ProductVariants.AnyAsync(pv => pv.Id == command.ProductVariantId, ct);
        if (!variantExists)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == command.WarehouseId && w.IsActive, ct);
        if (!warehouseExists)
            return ResultOperation.ToFailedResult("انبار یافت نشد یا غیرفعال است");

        var currentStock = await _context.StockLedgers
            .Where(sl => sl.ProductVariantId == command.ProductVariantId && sl.WarehouseId == command.WarehouseId)
            .SumAsync(sl => sl.QuantityChange, ct);

        var diff = command.NewQuantity - currentStock;
        if (diff == 0)
            return ResultOperation.ToSuccessResult("موجودی تغییر نکرد");

        var ledger = new StockLedger
        {
            ProductVariantId = command.ProductVariantId,
            WarehouseId = command.WarehouseId,
            QuantityChange = diff,
            Reason = StockChangeReason.ManualAdjustment,
            Description = command.Description ?? $"تعدیل دستی: از {currentStock} به {command.NewQuantity}"
        };

        _context.Add(ledger);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("تعدیل موجودی با موفقیت ثبت شد");
    }
}
