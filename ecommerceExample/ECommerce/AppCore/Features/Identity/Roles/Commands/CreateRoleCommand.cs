using ECommerce.AppCore.Data;
using ECommerce.AppCore.Domain.Identity;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Identity.Roles.Commands;

public class CreateRoleCommand
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateRoleHandler
{
    private readonly ECommerceDbContext _context;
    public CreateRoleHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation> Handle(CreateRoleCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return ResultOperation.ToFailedResult("نام نقش الزامی است");

        if (string.IsNullOrWhiteSpace(command.DisplayName))
            return ResultOperation.ToFailedResult("نام نمایشی نقش الزامی است");

        var exists = await _context.Roles.AnyAsync(r => r.Name == command.Name, ct);
        if (exists)
            return ResultOperation.ToFailedResult("نام نقش تکراری است");

        var role = new Role
        {
            Name = command.Name,
            DisplayName = command.DisplayName,
            Description = command.Description,
            IsActive = true
        };

        _context.Add(role);
        await _context.SaveChangesAsync(ct);

        return ResultOperation.ToSuccessResult("نقش با موفقیت ایجاد شد");
    }
}
