using ECommerce.AppCore.Features.Inventory.StockLedgers.Commands;
using ECommerce.AppCore.Features.Inventory.StockLedgers.Queries;
using Framework.ResultHelper;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using static Web.Helper.ApiPathHelper.Admin;

namespace Web.Controllers.Admin.Inventory;

[Route(Inventory.Stock.Base)]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IMessageBus _bus;

    public StockController(IMessageBus bus)
    {
        _bus = bus;
    }

    [HttpPost(Inventory.Stock.StockIn)]
    public async Task<IActionResult> StockIn(StockInCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Inventory.Stock.StockOut)]
    public async Task<IActionResult> StockOut(StockOutCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Inventory.Stock.Adjust)]
    public async Task<IActionResult> Adjust(AdjustStockCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Inventory.Stock.Initial)]
    public async Task<IActionResult> Initial(InitialStockCommand command, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation>(command, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Inventory.Stock.GetCurrent)]
    public async Task<IActionResult> GetCurrent(long variantId, long warehouseId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<CurrentStockDto>>(new GetCurrentStockQuery { ProductVariantId = variantId, WarehouseId = warehouseId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet(Inventory.Stock.GetHistory)]
    public async Task<IActionResult> GetHistory(long variantId, long warehouseId, CancellationToken ct)
    {
        var result = await _bus.InvokeAsync<ResultOperation<List<StockHistoryDto>>>(new GetStockHistoryQuery { ProductVariantId = variantId, WarehouseId = warehouseId }, ct);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
