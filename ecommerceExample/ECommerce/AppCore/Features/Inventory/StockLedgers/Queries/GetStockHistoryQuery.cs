using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Inventory.StockLedgers.Queries;

public class GetStockHistoryQuery
{
    public long ProductVariantId { get; set; }
    public long? WarehouseId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public StockChangeReason? Reason { get; set; }
}

public class StockHistoryDto
{
    public DateTime Date { get; set; }
    public int QuantityChange { get; set; }
    public StockChangeReason Reason { get; set; }
    public string? Description { get; set; }
    public long? CreatedBy { get; set; }
}

public class GetStockHistoryHandler
{
    private readonly ECommerceDbContext _context;
    public GetStockHistoryHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<StockHistoryDto>>> Handle(GetStockHistoryQuery query, CancellationToken ct)
    {
        if (query.ProductVariantId <= 0)
            return ResultOperation<List<StockHistoryDto>>.ToFailedResult("شناسه تنوع محصول نامعتبر است");

        var stockQuery = _context.StockLedgers
            .AsNoTracking()
            .Where(sl => sl.ProductVariantId == query.ProductVariantId);

        if (query.WarehouseId.HasValue)
            stockQuery = stockQuery.Where(sl => sl.WarehouseId == query.WarehouseId.Value);

        if (query.FromDate.HasValue)
            stockQuery = stockQuery.Where(sl => sl.CreatedAt >= query.FromDate.Value);

        if (query.ToDate.HasValue)
            stockQuery = stockQuery.Where(sl => sl.CreatedAt <= query.ToDate.Value);

        if (query.Reason.HasValue)
            stockQuery = stockQuery.Where(sl => sl.Reason == query.Reason.Value);

        var result = await stockQuery
            .OrderByDescending(sl => sl.CreatedAt)
            .Select(sl => new StockHistoryDto
            {
                Date = sl.CreatedAt,
                QuantityChange = sl.QuantityChange,
                Reason = sl.Reason,
                Description = sl.Description,
                CreatedBy = sl.CreatedBy
            })
            .ToListAsync(ct);

        return ResultOperation<List<StockHistoryDto>>.ToSuccessResult(result);
    }
}
