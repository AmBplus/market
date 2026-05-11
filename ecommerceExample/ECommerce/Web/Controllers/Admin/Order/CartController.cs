using ECommerce.AppCore.Features.Order.Carts.Commands;
using ECommerce.AppCore.Features.Order.Carts.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Order;

[Route(ApiPathHelper.Admin.Order.Carts.Base)]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IMessageBus _bus;

    public CartController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost(ApiPathHelper.Admin.Order.Carts.AddItem)]
    public async Task<IActionResult> AddItem(AddToCartCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Order.Carts.RemoveItem)]
    public async Task<IActionResult> RemoveItem(RemoveFromCartCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Order.Carts.UpdateQuantity)]
    public async Task<IActionResult> UpdateQuantity(UpdateCartItemQuantityCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Order.Carts.ApplyDiscount)]
    public async Task<IActionResult> ApplyDiscount(ApplyDiscountToCartCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Order.Carts.GetActive)]
    public async Task<IActionResult> GetActive(long userId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<CartDto>>(new GetActiveCartQuery { UserId = userId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
