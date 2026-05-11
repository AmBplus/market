using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Pricing.Currencies.Queries;

public class GetCurrencyByIdQuery
{
    public long Id { get; set; }
}

public class CurrencyDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public bool IsBase { get; set; }
    public bool IsActive { get; set; }
}

public class GetCurrencyByIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetCurrencyByIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<CurrencyDto>> Handle(GetCurrencyByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<CurrencyDto>.ToFailedResult("شناسه واحد پول نامعتبر است");

        var currency = await _context.Currencies
            .AsNoTracking()
            .Where(c => c.Id == query.Id)
            .Select(c => new CurrencyDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Symbol = c.Symbol,
                ExchangeRate = c.ExchangeRate,
                IsBase = c.IsBase,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (currency is null)
            return ResultOperation<CurrencyDto>.ToFailedResult("واحد پول یافت نشد");

        return ResultOperation<CurrencyDto>.ToSuccessResult(currency);
    }
}
