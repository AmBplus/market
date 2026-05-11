using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Product.Brands.Queries;

public class SelectBrandQuery
{
    public string? SearchName { get; set; }
}

public class BrandSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectBrandHandler
{
    private readonly AppDbContext _context;
    public SelectBrandHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<BrandSelect2Dto>>> Handle(SelectBrandQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Brands.AsNoTracking().Where(b => b.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(b => b.Name.Contains(query.SearchName));

        var result = await baseQuery
            .Select(b => new BrandSelect2Dto
            {
                Id = b.Id,
                Text = b.Name
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<BrandSelect2Dto>>.ToSuccessResult(result);
    }
}
