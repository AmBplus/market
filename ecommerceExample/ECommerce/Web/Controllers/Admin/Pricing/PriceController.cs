using ECommerce.AppCore.Features.Pricing.Prices.Commands;
using ECommerce.AppCore.Features.Pricing.Prices.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Pricing;

[Route(Pricing.Prices.Base)]
[ApiController]
public class PriceController : ControllerBase
{
    private readonly IMessageBus _bus;

    public PriceController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost(Pricing.Prices.Create)]
    public async Task<IActionResult> Create(CreatePriceCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Pricing.Prices.BulkUpdate)]
    public async Task<IActionResult> BulkUpdate(BulkPriceUpdateCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Pricing.Prices.GetByVariantId)]
    public async Task<IActionResult> GetByVariantId(long variantId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<PriceDto>>>(new GetPricesByVariantIdQuery { ProductVariantId = variantId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Pricing.Prices.GetHistory)]
    public async Task<IActionResult> GetHistory(long variantId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<PriceHistoryDto>>>(new GetPriceHistoryQuery { ProductVariantId = variantId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
