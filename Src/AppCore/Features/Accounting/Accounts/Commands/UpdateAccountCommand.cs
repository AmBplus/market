using AppCore.Data;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Accounting.Accounts.Commands;

public class UpdateAccountCommand
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public long? ParentId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateAccountHandler
{
    private readonly AppDbContext _context;
    public UpdateAccountHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateAccountCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه حساب نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد حساب الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام حساب الزامی است");

        var account = await _context.Accounts.FindAsync(new object[] { command.Id }, ct);
        if (account is null)
            return ResultOperation.ToFailedResult("حساب یافت نشد");

        var codeExists = await _context.Accounts.AnyAsync(a => a.Code == command.Code && a.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد حساب تکراری است");

        if (command.ParentId.HasValue)
        {
            if (command.ParentId.Value == command.Id)
                return ResultOperation.ToFailedResult("حساب نمی‌تواند والد خودش باشد");

            var parentExists = await _context.Accounts.AnyAsync(a => a.Id == command.ParentId.Value && a.IsActive, ct);
            if (!parentExists)
                return ResultOperation.ToFailedResult("حساب والد یافت نشد یا غیرفعال است");
        }

        account.Code = command.Code;
        account.Name = command.Name;
        account.AccountType = command.AccountType;
        account.ParentId = command.ParentId;
        account.Description = command.Description;
        account.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("حساب با موفقیت ویرایش شد");
    }
}
