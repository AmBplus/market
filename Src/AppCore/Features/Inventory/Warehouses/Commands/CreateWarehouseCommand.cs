using AppCore.Data;
using AppCore.Domains.Inventory;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Inventory.Warehouses.Commands;

public class CreateWarehouseCommand
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateWarehouseHandler
{
    private readonly AppDbContext _context;
    public CreateWarehouseHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateWarehouseCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام انبار الزامی است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد انبار الزامی است");

        var codeExists = await _context.Warehouses.AnyAsync(w => w.Code == command.Code, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد انبار تکراری است");

        var warehouse = new Warehouse
        {
            Name = command.Name,
            Code = command.Code,
            Address = command.Address,
            City = command.City,
            Province = command.Province,
            IsActive = command.IsActive
        };

        _context.Add(warehouse);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("انبار با موفقیت ایجاد شد");
    }
}
