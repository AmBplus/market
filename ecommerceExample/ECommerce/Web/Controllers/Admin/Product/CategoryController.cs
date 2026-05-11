using ECommerce.AppCore.Features.Product.Categories.Commands;
using ECommerce.AppCore.Features.Product.Categories.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Product;

[Route(Product.Categories.Base)]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public CategoryController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Product.Categories.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<CategoryDto>>(new GetCategoryByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Product.Categories.GetAll)]
    public async Task<IActionResult> GetAll(GetCategoriesDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<CategoryListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Product.Categories.Create)]
    public async Task<IActionResult> Create(CreateCategoryCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Product.Categories.Update)]
    public async Task<IActionResult> Update(UpdateCategoryCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(Product.Categories.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteCategoryCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Product.Categories.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectCategoryQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<CategorySelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
