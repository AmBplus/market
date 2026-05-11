using ECommerce.AppCore.Data;
using ECommerce.AppCore.Enums;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Accounting.Transactions.Queries;

public class GetTransactionsByBatchIdQuery
{
    public Guid BatchId { get; set; }
}

public class GetTransactionsByBatchIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetTransactionsByBatchIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<TransactionDto>>> Handle(GetTransactionsByBatchIdQuery query, CancellationToken ct)
    {
        if (query.BatchId == Guid.Empty)
            return ResultOperation<List<TransactionDto>>.ToFailedResult("شناسه دسته نامعتبر است");

        var result = await _context.Transactions
            .AsNoTracking()
            .Where(t => t.BatchId == query.BatchId)
            .OrderBy(t => t.TransactionType)
            .ThenBy(t => t.AccountId)
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

        if (!result.Any())
            return ResultOperation<List<TransactionDto>>.ToFailedResult("تراکنشی برای این دسته یافت نشد");

        return ResultOperation<List<TransactionDto>>.ToSuccessResult(result);
    }
}
