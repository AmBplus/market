using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Roles.Commands;

public class UpdateRoleCommand
{
    public long Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateRoleHandler
{
    private readonly AppDbContext _context;
    public UpdateRoleHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(UpdateRoleCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه نقش نامعتبر است");

        if (string.IsNullOrWhiteSpace(command.DisplayName))
            return ResultOperation.ToFailedResult("نام نمایشی نقش الزامی است");

        var role = await _context.Roles.FindAsync(new object[] { command.Id }, ct);
        if (role is null)
            return ResultOperation.ToFailedResult("نقش یافت نشد");

        role.DisplayName = command.DisplayName;
        role.Description = command.Description;
        role.IsActive = command.IsActive;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("نقش با موفقیت ویرایش شد");
    }
}
