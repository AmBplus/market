using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Pricing.Currencies.Queries;

public class SelectCurrencyQuery
{
    public string? SearchCode { get; set; }
}

public class CurrencySelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectCurrencyHandler
{
    private readonly ECommerceDbContext _context;
    public SelectCurrencyHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<CurrencySelect2Dto>>> Handle(SelectCurrencyQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Currencies.AsNoTracking().Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(c => c.Code.Contains(query.SearchCode) || c.Name.Contains(query.SearchCode));

        var result = await baseQuery
            .Select(c => new CurrencySelect2Dto
            {
                Id = c.Id,
                Text = c.Code + " - " + c.Name
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<CurrencySelect2Dto>>.ToSuccessResult(result);
    }
}
