using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shipping.Carriers.Queries;

public class SelectCarrierQuery
{
    public string? SearchName { get; set; }
}

public class CarrierSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectCarrierHandler
{
    private readonly AppDbContext _context;
    public SelectCarrierHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<CarrierSelect2Dto>>> Handle(SelectCarrierQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Carriers.AsNoTracking().Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(c => c.Name.Contains(query.SearchName) || c.Code.Contains(query.SearchName));

        var result = await baseQuery
            .Select(c => new CarrierSelect2Dto
            {
                Id = c.Id,
                Text = c.Name
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<CarrierSelect2Dto>>.ToSuccessResult(result);
    }
}
