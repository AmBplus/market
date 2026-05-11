using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Inventory.Warehouses.Queries;

public class SelectWarehouseQuery
{
    public string? SearchName { get; set; }
}

public class WarehouseSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectWarehouseHandler
{
    private readonly ECommerceDbContext _context;
    public SelectWarehouseHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<WarehouseSelect2Dto>>> Handle(SelectWarehouseQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Warehouses.AsNoTracking().Where(w => w.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(w => w.Name.Contains(query.SearchName) || w.Code.Contains(query.SearchName));

        var result = await baseQuery
            .Select(w => new WarehouseSelect2Dto
            {
                Id = w.Id,
                Text = w.Code + " - " + w.Name + (w.City != null ? " (" + w.City + ")" : "")
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<WarehouseSelect2Dto>>.ToSuccessResult(result);
    }
}
