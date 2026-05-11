using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Inventory.Warehouses.Queries;

public class GetWarehouseByIdQuery
{
    public long Id { get; set; }
}

public class WarehouseDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public bool IsActive { get; set; }
}

public class GetWarehouseByIdHandler
{
    private readonly AppDbContext _context;
    public GetWarehouseByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<WarehouseDto>> Handle(GetWarehouseByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<WarehouseDto>.ToFailedResult("شناسه انبار نامعتبر است");

        var warehouse = await _context.Warehouses
            .AsNoTracking()
            .Where(w => w.Id == query.Id)
            .Select(w => new WarehouseDto
            {
                Id = w.Id,
                Name = w.Name,
                Code = w.Code,
                Address = w.Address,
                City = w.City,
                Province = w.Province,
                IsActive = w.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (warehouse is null)
            return ResultOperation<WarehouseDto>.ToFailedResult("انبار یافت نشد");

        return ResultOperation<WarehouseDto>.ToSuccessResult(warehouse);
    }
}
