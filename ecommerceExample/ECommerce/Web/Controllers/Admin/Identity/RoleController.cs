using ECommerce.AppCore.Features.Identity.Roles.Commands;
using ECommerce.AppCore.Features.Identity.Roles.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Identity;

[Route(Identity.Role.BaseRole)]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public RoleController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Identity.Role.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<RoleDto>>(new GetRoleByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Identity.Role.GetAll)]
    public async Task<IActionResult> GetAll(GetRolesDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<RoleListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Identity.Role.Create)]
    public async Task<IActionResult> Create(CreateRoleCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Identity.Role.Update)]
    public async Task<IActionResult> Update(UpdateRoleCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(Identity.Role.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteRoleCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Identity.Role.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectRoleQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<RoleSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
