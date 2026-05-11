using ECommerce.AppCore.Features.Shipping.DeliveryMethods.Commands;
using ECommerce.AppCore.Features.Shipping.DeliveryMethods.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Shipping;

[Route(ApiPathHelper.Admin.Shipping.DeliveryMethods.Base)]
[ApiController]
public class DeliveryMethodController : ControllerBase
{
    private readonly IMessageBus _bus;

    public DeliveryMethodController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Shipping.DeliveryMethods.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<DeliveryMethodDto>>>(new GetDeliveryMethodsQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Shipping.DeliveryMethods.Create)]
    public async Task<IActionResult> Create(CreateDeliveryMethodCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Shipping.DeliveryMethods.Update)]
    public async Task<IActionResult> Update(UpdateDeliveryMethodCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Shipping.DeliveryMethods.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectDeliveryMethodQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<DeliveryMethodSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
