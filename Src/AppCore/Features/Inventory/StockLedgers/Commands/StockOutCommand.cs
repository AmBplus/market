using AppCore.Data;
using AppCore.Domains.Inventory;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Inventory.StockLedgers.Commands;

public class StockOutCommand
{
    public long ProductVariantId { get; set; }
    public long WarehouseId { get; set; }
    public int Quantity { get; set; }
    public long? ReferenceId { get; set; }
    public string? Description { get; set; }
}

public class StockOutHandler
{
    private readonly AppDbContext _context;
    public StockOutHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(StockOutCommand command, CancellationToken ct)
    {
        if (command.ProductVariantId <= 0)
            return ResultOperation.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        if (command.WarehouseId <= 0)
            return ResultOperation.ToFailedResult("شناسه انبار نامعتبر است");

        if (command.Quantity <= 0)
            return ResultOperation.ToFailedResult("تعداد خروجی باید بزرگتر از صفر باشد");

        var variantExists = await _context.ProductVariants.AnyAsync(pv => pv.Id == command.ProductVariantId, ct);
        if (!variantExists)
            return ResultOperation.ToFailedResult("تنوع محصول یافت نشد");

        var warehouseExists = await _context.Warehouses.AnyAsync(w => w.Id == command.WarehouseId && w.IsActive, ct);
        if (!warehouseExists)
            return ResultOperation.ToFailedResult("انبار یافت نشد یا غیرفعال است");

        var currentStock = await _context.StockLedgers
            .Where(sl => sl.ProductVariantId == command.ProductVariantId && sl.WarehouseId == command.WarehouseId)
            .SumAsync(sl => sl.QuantityChange, ct);

        if (currentStock < command.Quantity)
            return ResultOperation.ToFailedResult($"موجودی کافی نیست. موجودی فعلی: {currentStock}");

        var ledger = new StockLedger
        {
            ProductVariantId = command.ProductVariantId,
            WarehouseId = command.WarehouseId,
            QuantityChange = -command.Quantity,
            Reason = StockChangeReason.StockOut,
            ReferenceId = command.ReferenceId,
            Description = command.Description
        };

        _context.Add(ledger);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("خروجی موجودی با موفقیت ثبت شد");
    }
}
