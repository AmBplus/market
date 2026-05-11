using ECommerce.AppCore.Features.Product.Brands.Commands;
using ECommerce.AppCore.Features.Product.Brands.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Product;

[Route(Product.Brands.Base)]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public BrandController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Product.Brands.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<BrandDto>>(new GetBrandByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Product.Brands.GetAll)]
    public async Task<IActionResult> GetAll(GetBrandsDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<BrandListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Product.Brands.Create)]
    public async Task<IActionResult> Create(CreateBrandCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Product.Brands.Update)]
    public async Task<IActionResult> Update(UpdateBrandCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete(Product.Brands.Delete)]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new DeleteBrandCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Product.Brands.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectBrandQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<BrandSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
