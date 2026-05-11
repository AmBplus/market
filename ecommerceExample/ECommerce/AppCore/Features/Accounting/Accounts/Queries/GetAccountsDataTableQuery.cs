using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Accounting.Accounts.Queries;

public class GetAccountsDataTableQuery : DatatableFullRequest
{
    public string? SearchCode { get; set; }
    public string? SearchName { get; set; }
    public AccountType? AccountType { get; set; }
}

public class AccountListDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public string? ParentName { get; set; }
    public bool IsActive { get; set; }
}

public class GetAccountsDataTableHandler
{
    private readonly ECommerceDbContext _context;
    public GetAccountsDataTableHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<AccountListDto>>> Handle(GetAccountsDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Accounts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchCode))
            baseQuery = baseQuery.Where(a => a.Code.Contains(query.SearchCode));

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(a => a.Name.Contains(query.SearchName));

        if (query.AccountType.HasValue)
            baseQuery = baseQuery.Where(a => a.AccountType == query.AccountType.Value);

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(a => new AccountListDto
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                AccountType = a.AccountType,
                ParentName = a.Parent != null ? a.Parent.Name : null,
                IsActive = a.IsActive
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<AccountListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<AccountListDto>>.ToSuccessResult(response);
    }
}
