using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Accounting.Accounts.Queries;

public class GetAccountByIdQuery
{
    public long Id { get; set; }
}

public class AccountDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public long? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class GetAccountByIdHandler
{
    private readonly AppDbContext _context;
    public GetAccountByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<AccountDto>> Handle(GetAccountByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<AccountDto>.ToFailedResult("شناسه حساب نامعتبر است");

        var account = await _context.Accounts
            .AsNoTracking()
            .Where(a => a.Id == query.Id)
            .Select(a => new AccountDto
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                AccountType = a.AccountType,
                ParentId = a.ParentId,
                ParentName = a.Parent != null ? a.Parent.Name : null,
                Description = a.Description,
                IsActive = a.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (account is null)
            return ResultOperation<AccountDto>.ToFailedResult("حساب یافت نشد");

        return ResultOperation<AccountDto>.ToSuccessResult(account);
    }
}
