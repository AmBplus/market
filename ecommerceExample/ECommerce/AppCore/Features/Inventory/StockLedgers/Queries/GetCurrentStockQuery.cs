using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Inventory.StockLedgers.Queries;

public class GetCurrentStockQuery
{
    public long ProductVariantId { get; set; }
    public long? WarehouseId { get; set; }
}

public class CurrentStockDto
{
    public long ProductVariantId { get; set; }
    public long? WarehouseId { get; set; }
    public int Quantity { get; set; }
}

public class GetCurrentStockHandler
{
    private readonly ECommerceDbContext _context;
    public GetCurrentStockHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<CurrentStockDto>> Handle(GetCurrentStockQuery query, CancellationToken ct)
    {
        if (query.ProductVariantId <= 0)
            return ResultOperation<CurrentStockDto>.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        var variantExists = await _context.ProductVariants.AnyAsync(pv => pv.Id == query.ProductVariantId, ct);
        if (!variantExists)
            return ResultOperation<CurrentStockDto>.ToFailedResult("تنوع محصول یافت نشد");

        var stockQuery = _context.StockLedgers
            .Where(sl => sl.ProductVariantId == query.ProductVariantId);

        if (query.WarehouseId.HasValue)
            stockQuery = stockQuery.Where(sl => sl.WarehouseId == query.WarehouseId.Value);

        var quantity = await stockQuery.SumAsync(sl => sl.QuantityChange, ct);

        var result = new CurrentStockDto
        {
            ProductVariantId = query.ProductVariantId,
            WarehouseId = query.WarehouseId,
            Quantity = quantity
        };

        return ResultOperation<CurrentStockDto>.ToSuccessResult(result);
    }
}
