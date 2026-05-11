using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Settings.EnumValues.Commands;

public class UpdateEnumValueCommand
{
    public long Id { get; set; }
    public string EnumType { get; set; } = string.Empty;
    public string EnumKey { get; set; } = string.Empty;
    public int EnumId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateEnumValueHandler
{
    private readonly AppDbContext _context;
    public UpdateEnumValueHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateEnumValueCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه مقدار enum نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.EnumType))
            return ResultOperation.ToFailedResult("نوع enum الزامی است");

        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام الزامی است");

        var enumValue = await _context.EnumValues.FindAsync(new object[] { command.Id }, ct);
        if (enumValue is null)
            return ResultOperation.ToFailedResult("مقدار enum یافت نشد");

        var exists = await _context.EnumValues.AnyAsync(
            e => e.EnumType == command.EnumType && e.EnumId == command.EnumId && e.Id != command.Id, ct);
        if (exists)
            return ResultOperation.ToFailedResult("این مقدار enum قبلاً ثبت شده است");

        enumValue.EnumType = command.EnumType;
        enumValue.EnumKey = command.EnumKey;
        enumValue.EnumId = command.EnumId;
        enumValue.Name = command.Name;
        enumValue.Description = command.Description;
        enumValue.DisplayOrder = command.DisplayOrder;
        enumValue.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("مقدار enum با موفقیت ویرایش شد");
    }
}
