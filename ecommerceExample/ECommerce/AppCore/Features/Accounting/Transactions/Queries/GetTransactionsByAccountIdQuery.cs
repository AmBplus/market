using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Accounting.Transactions.Queries;

public class GetTransactionsByAccountIdQuery
{
    public long AccountId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class TransactionDto
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public Guid BatchId { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Amount { get; set; }
    public TransactionSource Source { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetTransactionsByAccountIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetTransactionsByAccountIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<TransactionDto>>> Handle(GetTransactionsByAccountIdQuery query, CancellationToken ct)
    {
        if (query.AccountId <= 0)
            return ResultOperation<List<TransactionDto>>.ToFailedResult("شناسه حساب نامعتبر است");

        var transactionQuery = _context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == query.AccountId);

        if (query.FromDate.HasValue)
            transactionQuery = transactionQuery.Where(t => t.CreatedAt >= query.FromDate.Value);

        if (query.ToDate.HasValue)
            transactionQuery = transactionQuery.Where(t => t.CreatedAt <= query.ToDate.Value);

        var result = await transactionQuery
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto
            {
                AccountCode = t.Account.Code,
                AccountName = t.Account.Name,
                BatchId = t.BatchId,
                TransactionType = t.TransactionType,
                Amount = t.Amount,
                Source = t.Source,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync(ct);

        return ResultOperation<List<TransactionDto>>.ToSuccessResult(result);
    }
}
