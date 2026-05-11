using ECommerce.AppCore.Features.Customization.Widgets.Commands;
using ECommerce.AppCore.Features.Customization.Widgets.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Customization;

[Route(ApiPathHelper.Admin.Customization.Widgets.Base)]
[ApiController]
public class WidgetController : ControllerBase
{
    private readonly IMessageBus _bus;

    public WidgetController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Customization.Widgets.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<WidgetDto>>>(new GetAllWidgetsQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Customization.Widgets.GetByPosition)]
    public async Task<IActionResult> GetByPosition([FromQuery] string position, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<WidgetDto>>>(new GetWidgetsByPositionQuery { Position = position }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Customization.Widgets.Create)]
    public async Task<IActionResult> Create(CreateWidgetCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Customization.Widgets.Update)]
    public async Task<IActionResult> Update(UpdateWidgetCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(ApiPathHelper.Admin.Customization.Widgets.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteWidgetCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
