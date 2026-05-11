using ECommerce.AppCore.Features.Identity.Users.Commands;
using ECommerce.AppCore.Features.Identity.Users.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Identity;

[Route(Identity.User.BaseUser)]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public UserController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Identity.User.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<UserDto>>(new GetUserByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Identity.User.GetAll)]
    public async Task<IActionResult> GetAll(GetUsersDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<UserListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Identity.User.Create)]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Identity.User.Update)]
    public async Task<IActionResult> Update(UpdateUserCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(Identity.User.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteUserCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Identity.User.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Identity.User.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectUserQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<UserSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
