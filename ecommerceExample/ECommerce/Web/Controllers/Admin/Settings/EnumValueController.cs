using ECommerce.AppCore.Features.Settings.EnumValues.Commands;
using ECommerce.AppCore.Features.Settings.EnumValues.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Settings;

[Route(ApiPathHelper.Admin.Settings.EnumValues.Base)]
[ApiController]
public class EnumValueController : ControllerBase
{
    private readonly IMessageBus _bus;

    public EnumValueController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Settings.EnumValues.GetByType)]
    public async Task<IActionResult> GetByType([FromQuery] string enumType, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<EnumValueDto>>>(new GetEnumValuesByTypeQuery { EnumType = enumType }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Settings.EnumValues.Create)]
    public async Task<IActionResult> Create(CreateEnumValueCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Settings.EnumValues.Update)]
    public async Task<IActionResult> Update(UpdateEnumValueCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
