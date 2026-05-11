using ECommerce.AppCore.Features.Payment.PaymentGateways.Commands;
using ECommerce.AppCore.Features.Payment.PaymentGateways.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Payment;

[Route(ApiPathHelper.Admin.Payment.Gateways.Base)]
[ApiController]
public class PaymentGatewayController : ControllerBase
{
    private readonly IMessageBus _bus;

    public PaymentGatewayController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Payment.Gateways.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<PaymentGatewayDto>>>(new GetPaymentGatewaysQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Payment.Gateways.GetActive)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<PaymentGatewayDto>>>(new GetActivePaymentGatewaysQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Payment.Gateways.Create)]
    public async Task<IActionResult> Create(CreatePaymentGatewayCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Payment.Gateways.Update)]
    public async Task<IActionResult> Update(UpdatePaymentGatewayCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
