using ECommerce.AppCore.Features.Settings.Settings.Commands;
using ECommerce.AppCore.Features.Settings.Settings.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Settings;

[Route(ApiPathHelper.Admin.Settings.AppSettings.Base)]
[ApiController]
public class SettingController : ControllerBase
{
    private readonly IMessageBus _bus;

    public SettingController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Settings.AppSettings.GetByKey)]
    public async Task<IActionResult> GetByKey([FromQuery] string key, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<SettingDto>>(new GetSettingByKeyQuery { Key = key }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Settings.AppSettings.GetByModule)]
    public async Task<IActionResult> GetByModule([FromQuery] string module, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<SettingDto>>>(new GetSettingsByModuleQuery { Module = module }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Settings.AppSettings.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<SettingDto>>>(new GetAllSettingsQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Settings.AppSettings.Update)]
    public async Task<IActionResult> Update(UpdateSettingCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Settings.AppSettings.BulkUpdate)]
    public async Task<IActionResult> BulkUpdate(BulkUpdateSettingsCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
