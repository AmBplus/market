using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Roles.Commands;

public class DeleteRoleCommand
{
    public long Id { get; set; }
}

public class DeleteRoleHandler
{
    private readonly AppDbContext _context;
    public DeleteRoleHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteRoleCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه نقش نامعتبر است");

        var role = await _context.Roles.FindAsync(new object[] { command.Id }, ct);
        if (role is null)
            return ResultOperation.ToFailedResult("نقش یافت نشد");

        var hasUsers = await _context.UserRoles.AnyAsync(ur => ur.RoleId == command.Id, ct);
        if (hasUsers)
            return ResultOperation.ToFailedResult("این نقش دارای کاربر است و قابل حذف نیست");

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("نقش با موفقیت حذف شد");
    }
}
