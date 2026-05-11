using ECommerce.AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Pricing.Currencies.Queries;

public class GetCurrenciesDataTableQuery : DatatableFullRequest
{
    public string? SearchCode { get; set; }
    public string? SearchName { get; set; }
}

public class CurrencyListDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public bool IsBase { get; set; }
    public bool IsActive { get; set; }
}

public class GetCurrenciesDataTableHandler
{
    private readonly ECommerceDbContext _context;
    public GetCurrenciesDataTableHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<CurrencyListDto>>> Handle(GetCurrenciesDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Currencies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(c => c.Code.Contains(query.SearchCode));

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(c => c.Name.Contains(query.SearchName));

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(c => new CurrencyListDto
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                Symbol = c.Symbol,
                ExchangeRate = c.ExchangeRate,
                IsBase = c.IsBase,
                IsActive = c.IsActive
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<CurrencyListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<CurrencyListDto>>.ToSuccessResult(response);
    }
}
