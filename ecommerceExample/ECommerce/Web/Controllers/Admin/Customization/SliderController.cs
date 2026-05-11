using ECommerce.AppCore.Features.Customization.Sliders.Commands;
using ECommerce.AppCore.Features.Customization.Sliders.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Customization;

[Route(ApiPathHelper.Admin.Customization.Sliders.Base)]
[ApiController]
public class SliderController : ControllerBase
{
    private readonly IMessageBus _bus;

    public SliderController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Customization.Sliders.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<SliderDto>>>(new GetSlidersQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Customization.Sliders.GetActive)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<SliderDto>>>(new GetActiveSlidersQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Customization.Sliders.Create)]
    public async Task<IActionResult> Create(CreateSliderCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Customization.Sliders.Update)]
    public async Task<IActionResult> Update(UpdateSliderCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(ApiPathHelper.Admin.Customization.Sliders.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteSliderCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
