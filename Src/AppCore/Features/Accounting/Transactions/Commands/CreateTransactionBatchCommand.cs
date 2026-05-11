using AppCore.Data;
using AppCore.Domains.Accounting;
using AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Accounting.Transactions.Commands;

public class TransactionEntryDto
{
    public long AccountId { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public long CurrencyId { get; set; }
    public TransactionSource Source { get; set; }
    public long? ReferenceId { get; set; }
    public string? Description { get; set; }
}

public class CreateTransactionBatchCommand
{
    public List<TransactionEntryDto> Entries { get; set; } = new();
}

public class CreateTransactionBatchHandler
{
    private readonly AppDbContext _context;
    public CreateTransactionBatchHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<Guid>> Handle(CreateTransactionBatchCommand command, CancellationToken ct)
    {
        if (command.Entries is null || !command.Entries.Any())
            return ResultOperation<Guid>.ToFailedResult("حداقل یک ردیف تراکنش الزامی است");

        // Validate debit total == credit total
        var debitTotal = command.Entries.Where(e => e.TransactionType == TransactionType.Debit).Sum(e => e.Amount);
        var creditTotal = command.Entries.Where(e => e.TransactionType == TransactionType.Credit).Sum(e => e.Amount);

        if (debitTotal != creditTotal)
            return ResultOperation<Guid>.ToFailedResult($"مجموع بدهکار ({debitTotal}) با مجموع بستانکار ({creditTotal}) برابر نیست");

        if (debitTotal == 0)
            return ResultOperation<Guid>.ToFailedResult("مبلغ تراکنش نمی‌تواند صفر باشد");

        // Validate all accounts exist
        var accountIds = command.Entries.Select(e => e.AccountId).Distinct().ToList();
        var existingAccountIds = await _context.Accounts
            .Where(a => accountIds.Contains(a.Id) && a.IsActive)
            .Select(a => a.Id)
            .ToListAsync(ct);

        var missingAccounts = accountIds.Except(existingAccountIds).ToList();
        if (missingAccounts.Any())
            return ResultOperation<Guid>.ToFailedResult($"حساب‌های زیر یافت نشد یا غیرفعال هستند: {string.Join(", ", missingAccounts)}");

        var batchId = Guid.NewGuid();

        foreach (var entry in command.Entries)
        {
            if (entry.Amount <= 0)
                return ResultOperation<Guid>.ToFailedResult("مبلغ تراکنش باید بزرگتر از صفر باشد");

            var transaction = new Transaction
            {
                AccountId = entry.AccountId,
                BatchId = batchId,
                TransactionType = entry.TransactionType,
                Amount = entry.Amount,
                CurrencyId = entry.CurrencyId,
                Source = entry.Source,
                ReferenceId = entry.ReferenceId,
                Description = entry.Description
            };
            _context.Add(transaction);
        }

        await _context.SaveChangesAsync(ct);

        return ResultOperation<Guid>.ToSuccessResult("دسته تراکنش با موفقیت ثبت شد", batchId);
    }
}
