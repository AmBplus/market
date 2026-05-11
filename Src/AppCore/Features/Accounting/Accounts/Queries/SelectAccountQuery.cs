using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Accounting.Accounts.Queries;

public class SelectAccountQuery
{
    public string? SearchName { get; set; }
}

public class AccountSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectAccountHandler
{
    private readonly AppDbContext _context;
    public SelectAccountHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<List<AccountSelect2Dto>>> Handle(SelectAccountQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Accounts.AsNoTracking().Where(a => a.IsActive);

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(a => a.Name.Contains(query.SearchName) || a.Code.Contains(query.SearchName));

        var result = await baseQuery
            .Select(a => new AccountSelect2Dto
            {
                Id = a.Id,
                Text = a.Code + " - " + a.Name
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<AccountSelect2Dto>>.ToSuccessResult(result);
    }
}
