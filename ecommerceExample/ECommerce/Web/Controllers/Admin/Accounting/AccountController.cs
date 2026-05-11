using ECommerce.AppCore.Features.Accounting.Accounts.Commands;
using ECommerce.AppCore.Features.Accounting.Accounts.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Accounting;

[Route(ApiPathHelper.Admin.Accounting.Accounts.Base)]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public AccountController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(ApiPathHelper.Admin.Accounting.Accounts.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<AccountDto>>(new GetAccountByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Accounting.Accounts.GetAll)]
    public async Task<IActionResult> GetAll(GetAccountsDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<AccountListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(ApiPathHelper.Admin.Accounting.Accounts.Create)]
    public async Task<IActionResult> Create(CreateAccountCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Accounting.Accounts.Update)]
    public async Task<IActionResult> Update(UpdateAccountCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Accounting.Accounts.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectAccountQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<AccountSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
