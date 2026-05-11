using AppCore.Data;
using AppCore.Domains.Identity;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Users.Commands;

public class DeleteUserCommand
{
    public long Id { get; set; }
    public long? DeletedBy { get; set; }
}

public class DeleteUserHandler
{
    private readonly AppDbContext _context;
    public DeleteUserHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(DeleteUserCommand command, CancellationToken ct)
    {
        if (command.Id <= 0)
            return ResultOperation.ToFailedResult("شناسه کاربر نامعتبر است");

        var user = await _context.Users.FindAsync(new object[] { command.Id }, ct);
        if (user is null)
            return ResultOperation.ToFailedResult("کاربر یافت نشد");

        var userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == command.Id)
            .ToListAsync(ct);

        _context.UserRoles.RemoveRange(userRoles);

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;
        user.DeletedBy = command.DeletedBy;

        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("کاربر با موفقیت حذف شد");
    }
}
