using AppCore.Data;
using AppCore.Domains.Accounting;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Accounting.Accounts.Commands;

public class CreateAccountCommand
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public long? ParentId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateAccountHandler
{
    private readonly AppDbContext _context;
    public CreateAccountHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateAccountCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد حساب الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام حساب الزامی است");

        var codeExists = await _context.Accounts.AnyAsync(a => a.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد حساب تکراری است");

        if (command.ParentId.HasValue)
        {
            var parentExists = await _context.Accounts.AnyAsync(a => a.Id == command.ParentId.Value && a.IsActive, ct);
            if (!parentExists)
                return ResultOperation.ToFailedResult("حساب والد یافت نشد یا غیرفعال است");
        }

        var account = new Account
        {
            Code = command.Code,
            Name = command.Name,
            AccountType = command.AccountType,
            ParentId = command.ParentId,
            Description = command.Description,
            IsActive = command.IsActive
        };

        _context.Add(account);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("حساب با موفقیت ایجاد شد");
    }
}
