using ECommerce.AppCore.Features.Vendor.Vendors.Commands;
using ECommerce.AppCore.Features.Vendor.Vendors.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Vendor;

[Route(ApiPathHelper.Admin.Vendor.Vendors.Base)]
[ApiController]
public class VendorController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public VendorController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(ApiPathHelper.Admin.Vendor.Vendors.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<VendorDto>>(new GetVendorByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(ApiPathHelper.Admin.Vendor.Vendors.GetAll)]
    public async Task<IActionResult> GetAll(GetVendorsDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<VendorListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(ApiPathHelper.Admin.Vendor.Vendors.Create)]
    public async Task<IActionResult> Create(CreateVendorCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Vendor.Vendors.Update)]
    public async Task<IActionResult> Update(UpdateVendorCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Vendor.Vendors.Approve)]
    public async Task<IActionResult> Approve(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new ApproveVendorCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(ApiPathHelper.Admin.Vendor.Vendors.Suspend)]
    public async Task<IActionResult> Suspend(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(new SuspendVendorCommand { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(ApiPathHelper.Admin.Vendor.Vendors.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectVendorQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<VendorSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
