using AppCore.Data;
using AppCore.Enums;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Queries;

public class GetVendorsDataTableQuery : DatatableFullRequest
{
    public string? SearchStoreName { get; set; }
    public VendorStatus? SearchStatus { get; set; }
    public bool? SearchIsActive { get; set; }
}

public class VendorListDto
{
    public long Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public decimal CommissionRate { get; set; }
    public VendorStatus Status { get; set; }
    public bool IsActive { get; set; }
}

public class GetVendorsDataTableHandler
{
    private readonly AppDbContext _context;
    public GetVendorsDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<VendorListDto>>> Handle(GetVendorsDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Vendors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchStoreName))
            baseQuery = baseQuery.Where(v => v.StoreName.Contains(query.SearchStoreName));

        if (query.SearchStatus.HasValue)
            baseQuery = baseQuery.Where(v => v.Status == query.SearchStatus.Value);

        if (query.SearchIsActive.HasValue)
            baseQuery = baseQuery.Where(v => v.IsActive == query.SearchIsActive.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(v => new VendorListDto
            {
                Id = v.Id,
                StoreName = v.StoreName,
                UserName = v.User.UserName ?? "",
                CommissionRate = v.CommissionRate,
                Status = v.Status,
                IsActive = v.IsActive
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<VendorListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<VendorListDto>>.ToSuccessResult(response);
    }
}
