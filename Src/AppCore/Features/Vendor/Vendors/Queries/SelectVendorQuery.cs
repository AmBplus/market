using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Vendor.Vendors.Queries;

public class SelectVendorQuery
{
    public string? SearchName { get; set; }
}

public class VendorSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectVendorHandler
{
    private readonly AppDbContext _context;
    public SelectVendorHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<VendorSelect2Dto>>> Handle(SelectVendorQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Vendors.AsNoTracking().Where(v => v.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(v => v.StoreName.Contains(query.SearchName) || v.StoreSlug.Contains(query.SearchName));

        var result = await baseQuery
            .Select(v => new VendorSelect2Dto
            {
                Id = v.Id,
                Text = v.StoreName
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<VendorSelect2Dto>>.ToSuccessResult(result);
    }
}
