using ECommerce.AppCore.Features.Order.Orders.Commands;
using ECommerce.AppCore.Features.Order.Orders.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Order;

[Route(ApiPathHelper.Admin.Order.Orders.Base)]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public OrderController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(ApiPathHelper.Admin.Order.Orders.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<OrderDto>>(new GetOrderByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Order.Orders.GetAll)]
    public async Task<IActionResult> GetAll(GetOrdersDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<OrderListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(ApiPathHelper.Admin.Order.Orders.Create)]
    public async Task<IActionResult> Create(CreateOrderCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Order.Orders.Cancel)]
    public async Task<IActionResult> Cancel(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new CancelOrderCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Order.Orders.UpdateStatus)]
    public async Task<IActionResult> UpdateStatus(UpdateOrderStatusCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
