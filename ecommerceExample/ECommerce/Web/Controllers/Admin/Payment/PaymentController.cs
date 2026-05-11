using ECommerce.AppCore.Features.Payment.Payments.Commands;
using ECommerce.AppCore.Features.Payment.Payments.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Payment;

[Route(ApiPathHelper.Admin.Payment.Payments.Base)]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IMessageBus _bus;

    public PaymentController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Payment.Payments.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<PaymentDto>>(new GetPaymentByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Payment.Payments.GetByOrderId)]
    public async Task<IActionResult> GetByOrderId(long orderId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<PaymentDto>>>(new GetPaymentsByOrderIdQuery { OrderId = orderId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Payment.Payments.Create)]
    public async Task<IActionResult> Create(CreatePaymentCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Payment.Payments.Verify)]
    public async Task<IActionResult> Verify(VerifyPaymentCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Payment.Payments.Refund)]
    public async Task<IActionResult> Refund(RefundPaymentCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
