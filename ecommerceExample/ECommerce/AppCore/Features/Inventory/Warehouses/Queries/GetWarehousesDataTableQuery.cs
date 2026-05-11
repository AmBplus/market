using ECommerce.AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Inventory.Warehouses.Queries;

public class GetWarehousesDataTableQuery : DatatableFullRequest
{
    public string? SearchName { get; set; }
    public string? SearchCode { get; set; }
    public string? SearchCity { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class WarehouseListDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? Province { get; set; }
    public bool IsActive { get; set; }
}

public class GetWarehousesDataTableHandler
{
    private readonly ECommerceDbContext _context;
    public GetWarehousesDataTableHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<WarehouseListDto>>> Handle(GetWarehousesDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Warehouses.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(w => w.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(w => w.Code.Contains(query.SearchCode));

        if (!string.IsNullOrWhiteSpace(query.SearchCity))
            baseQuery = baseQuery.Where(w => w.City != null && w.City.Contains(query.SearchCity));

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(w => w.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(w => new WarehouseListDto
            {
                Id = w.Id,
                Code = w.Code,
                Name = w.Name,
                City = w.City,
                Province = w.Province,
                IsActive = w.IsActive
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<WarehouseListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<WarehouseListDto>>.ToSuccessResult(response);
    }
}
