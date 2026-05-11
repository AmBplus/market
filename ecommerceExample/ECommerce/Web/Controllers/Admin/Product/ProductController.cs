using ECommerce.AppCore.Features.Product.Products.Commands;
using ECommerce.AppCore.Features.Product.Products.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Product;

[Route(Product.Products.Base)]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public ProductController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Product.Products.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<ProductDto>>(new GetProductByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Product.Products.GetAll)]
    public async Task<IActionResult> GetAll(GetProductsDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<ProductListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Product.Products.Create)]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Product.Products.Update)]
    public async Task<IActionResult> Update(UpdateProductCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(Product.Products.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteProductCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Product.Products.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectProductQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<ProductSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
