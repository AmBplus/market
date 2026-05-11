using ECommerce.AppCore.Features.Product.ProductVariants.Commands;
using ECommerce.AppCore.Features.Product.ProductVariants.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Product;

[Route(Product.ProductVariants.Base)]
[ApiController]
public class ProductVariantController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public ProductVariantController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Product.ProductVariants.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<ProductVariantDto>>(new GetProductVariantByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Product.ProductVariants.GetAll)]
    public async Task<IActionResult> GetAll(GetProductVariantsDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<ProductVariantListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Product.ProductVariants.Create)]
    public async Task<IActionResult> Create(CreateProductVariantCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Product.ProductVariants.Update)]
    public async Task<IActionResult> Update(UpdateProductVariantCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(Product.ProductVariants.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteProductVariantCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Product.ProductVariants.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectProductVariantQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<ProductVariantSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
