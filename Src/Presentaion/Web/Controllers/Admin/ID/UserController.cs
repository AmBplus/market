using AppCore.Domains.Entities.ID;
using AppCore.Features.ID.Users.Commands;
using AppCore.Features.ID.Users.Queries;
using AppCore.Features.ID.Users.Queries.GetUserById;
using AppCore.Features.ID.Users.Queries.Shared;
using Framework.Aspc.Helper;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Web.Helper;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.ID
{
    [Route(ApiPathHelper.Admin.ID.User.BaseUser)]
    [ApiController]
    public class UserController : Controller
    {
        public UserController(IMessageBus bus, IDatatableResponseHelper datatableResponseHelper)
        {
            Bus = bus;
            DatatableResponseHelper = datatableResponseHelper;
        }

        public IMessageBus Bus { get; }
        public IDatatableResponseHelper DatatableResponseHelper { get; }
        [HttpGet(ApiPathHelper.Admin.ID.User.GetUserByID)]
        public async Task<IActionResult> GetUserByID(long id, CancellationToken cancellationToken)
        {
            var result = await Bus.InvokeAsync<ResultOperation<UserDto>>(new GetUserByIdQuery(id), cancellationToken);
            if(result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost(ApiPathHelper.Admin.ID.User.GetAll)]
        public async Task<IActionResult> GetAll(GetUsersDataTableQuery query,CancellationToken cancellationToken)
        {
            var result = await Bus.InvokeAsync<ResultOperation<DataTableResponse<UserListDto>>>(query,cancellationToken);
            return await DatatableResponseHelper.Get(datatableRequest: query, handlerResult: result, "UsersExport", cancellationToken: cancellationToken);
        }
        public class DeleteUserRequest
        {
            public long Id { get; set; }
        }
        [HttpPost("Delete2")]
        public async Task<IActionResult> Delete2(long id)
        {
            return Ok(new { message = id });
        }
        [HttpPost(ApiPathHelper.Admin.ID.User.Delete)]
        public async Task<IActionResult> Delete(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var result = await Bus.InvokeAsync<ResultOperation>(
                new DeleteUserCommand(request.Id, null), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.MessageSingle });
        }

    }


}
