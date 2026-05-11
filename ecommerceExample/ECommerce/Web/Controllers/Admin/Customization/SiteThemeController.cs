using ECommerce.AppCore.Features.Customization.SiteThemes.Commands;
using ECommerce.AppCore.Features.Customization.SiteThemes.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Customization;

[Route(ApiPathHelper.Admin.Customization.Themes.Base)]
[ApiController]
public class SiteThemeController : ControllerBase
{
    private readonly IMessageBus _bus;

    public SiteThemeController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPathHelper.Admin.Customization.Themes.GetActive)]
    public async Task<IActionResult> GetActive(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<SiteThemeDto>>(new GetActiveThemeQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Customization.Themes.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<SiteThemeDto>>>(new GetAllThemesQuery(), ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Customization.Themes.Create)]
    public async Task<IActionResult> Create(CreateSiteThemeCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Customization.Themes.Update)]
    public async Task<IActionResult> Update(UpdateSiteThemeCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Customization.Themes.Activate)]
    public async Task<IActionResult> Activate(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new ActivateSiteThemeCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
