using ECommerce.AppCore.Features.Accounting.Transactions.Commands;
using ECommerce.AppCore.Features.Accounting.Transactions.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Accounting;

[Route(ApiPathHelper.Admin.Accounting.Transactions.Base)]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IMessageBus _bus;

    public TransactionController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost(ApiPathHelper.Admin.Accounting.Transactions.CreateBatch)]
    public async Task<IActionResult> CreateBatch(CreateTransactionBatchCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Accounting.Transactions.GetByAccountId)]
    public async Task<IActionResult> GetByAccountId(long accountId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<TransactionDto>>>(new GetTransactionsByAccountIdQuery { AccountId = accountId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Accounting.Transactions.GetByBatchId)]
    public async Task<IActionResult> GetByBatchId(Guid batchId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<TransactionDto>>>(new GetTransactionsByBatchIdQuery { BatchId = batchId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
