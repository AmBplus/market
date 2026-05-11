using ECommerce.AppCore.Features.Shipping.OrderDeliveries.Commands;
using ECommerce.AppCore.Features.Shipping.OrderDeliveries.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Shipping;

[Route(ApiPathHelper.Admin.Shipping.OrderDeliveries.Base)]
[ApiController]
public class OrderDeliveryController : ControllerBase
{
    private readonly IMessageBus _bus;

    public OrderDeliveryController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Shipping.OrderDeliveries.GetByOrderId)]
    public async Task<IActionResult> GetByOrderId(long orderId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<OrderDeliveryDto>>(new GetDeliveryByOrderIdQuery { OrderId = orderId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Shipping.OrderDeliveries.UpdateStatus)]
    public async Task<IActionResult> UpdateStatus(UpdateDeliveryStatusCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Shipping.OrderDeliveries.SetTrackingCode)]
    public async Task<IActionResult> SetTrackingCode(SetTrackingCodeCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
