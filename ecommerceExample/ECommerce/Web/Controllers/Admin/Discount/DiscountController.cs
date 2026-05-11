using ECommerce.AppCore.Features.Discount.Discounts.Commands;
using ECommerce.AppCore.Features.Discount.Discounts.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Discount;

[Route(ApiPathHelper.Admin.Discount.Discounts.Base)]
[ApiController]
public class DiscountController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public DiscountController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(ApiPathHelper.Admin.Discount.Discounts.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DiscountDto>>(new GetDiscountByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Discount.Discounts.GetAll)]
    public async Task<IActionResult> GetAll(GetDiscountsDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<DiscountListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(ApiPathHelper.Admin.Discount.Discounts.Create)]
    public async Task<IActionResult> Create(CreateDiscountCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Discount.Discounts.Update)]
    public async Task<IActionResult> Update(UpdateDiscountCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(ApiPathHelper.Admin.Discount.Discounts.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteDiscountCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Discount.Discounts.ValidateCode)]
    public async Task<IActionResult> ValidateCode([FromQuery] string code, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DiscountValidationDto>>(new ValidateDiscountCodeQuery { Code = code }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
