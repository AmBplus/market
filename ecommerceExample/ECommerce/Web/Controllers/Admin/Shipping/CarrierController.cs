using ECommerce.AppCore.Features.Shipping.Carriers.Commands;
using ECommerce.AppCore.Features.Shipping.Carriers.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Shipping;

[Route(ApiPathHelper.Admin.Shipping.Carriers.Base)]
[ApiController]
public class CarrierController : ControllerBase
{
    private readonly IMessageBus _bus;

    public CarrierController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Shipping.Carriers.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<CarrierDto>>>(new GetCarriersQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Shipping.Carriers.Create)]
    public async Task<IActionResult> Create(CreateCarrierCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Shipping.Carriers.Update)]
    public async Task<IActionResult> Update(UpdateCarrierCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Shipping.Carriers.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectCarrierQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<CarrierSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
