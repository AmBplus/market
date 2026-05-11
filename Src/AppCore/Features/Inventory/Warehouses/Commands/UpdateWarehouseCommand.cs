using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Inventory.Warehouses.Commands;

public class UpdateWarehouseCommand
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateWarehouseHandler
{
    private readonly AppDbContext _context;
    public UpdateWarehouseHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateWarehouseCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه انبار نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام انبار الزامی است");

        if (string.IsNullOrWhiteSpace(command.Code))
            return ResultOperation.ToFailedResult("کد انبار الزامی است");

        var warehouse = await _context.Warehouses.FindAsync(new object[] { command.Id }, ct);
        if (warehouse is null)
            return ResultOperation.ToFailedResult("انبار یافت نشد");

        var codeExists = await _context.Warehouses.AnyAsync(w => w.Code == command.Code && w.Id != command.Id, ct);
        if (codeExists)
            return ResultOperation.ToFailedResult("کد انبار تکراری است");

        warehouse.Name = command.Name;
        warehouse.Code = command.Code;
        warehouse.Address = command.Address;
        warehouse.City = command.City;
        warehouse.Province = command.Province;
        warehouse.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("انبار با موفقیت ویرایش شد");
    }
}
