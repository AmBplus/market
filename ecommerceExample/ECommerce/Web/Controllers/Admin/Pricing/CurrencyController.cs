using ECommerce.AppCore.Features.Pricing.Currencies.Commands;
using ECommerce.AppCore.Features.Pricing.Currencies.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Pricing;

[Route(Pricing.Currencies.Base)]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public CurrencyController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Pricing.Currencies.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<CurrencyDto>>(new GetCurrencyByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Pricing.Currencies.GetAll)]
    public async Task<IActionResult> GetAll(GetCurrenciesDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<CurrencyListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Pricing.Currencies.Create)]
    public async Task<IActionResult> Create(CreateCurrencyCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Pricing.Currencies.Update)]
    public async Task<IActionResult> Update(UpdateCurrencyCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Pricing.Currencies.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectCurrencyQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<CurrencySelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
