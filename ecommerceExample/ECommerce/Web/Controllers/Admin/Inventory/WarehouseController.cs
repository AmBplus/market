using ECommerce.AppCore.Features.Inventory.Warehouses.Commands;
using ECommerce.AppCore.Features.Inventory.Warehouses.Queries;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Inventory;

[Route(Inventory.Warehouses.Base)]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly IMessageBus _bus;
    private readonly IDatatableResponseHelper _datatableHelper;

    public WarehouseController(IMessageBus bus, IDatatableResponseHelper datatableHelper)
    {
        _bus = bus;
        _datatableHelper = datatableHelper;
    }

    [HttpGet(Inventory.Warehouses.GetById)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<WarehouseDto>>(new GetWarehouseByIdQuery { Id = id }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Inventory.Warehouses.GetAll)]
    public async Task<IActionResult> GetAll(GetWarehousesDataTableQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<DataTableResponse<WarehouseListDto>>>(query, ct);
        return await _datatableHelper.Get(query, result, ct: ct);
    }

    [HttpPost(Inventory.Warehouses.Create)]
    public async Task<IActionResult> Create(CreateWarehouseCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut(Inventory.Warehouses.Update)]
    public async Task<IActionResult> Update(UpdateWarehouseCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Inventory.Warehouses.Select2)]
    public async Task<IActionResult> Select2([FromQuery] SelectWarehouseQuery query, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<WarehouseSelect2Dto>>>(query, ct);
        return Ok(result);
    }
}
