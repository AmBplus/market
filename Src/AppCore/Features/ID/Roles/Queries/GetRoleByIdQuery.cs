using AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Roles.Queries;

public class GetRoleByIdQuery
{
    public long Id { get; set; }
}

public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class GetRoleByIdHandler
{
    private readonly AppDbContext _context;
    public GetRoleByIdHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<RoleDto>> Handle(GetRoleByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<RoleDto>.ToFailedResult("شناسه نقش نامعتبر است");

        var role = await _context.Roles
            .Where(r => r.Id == query.Id)
            .Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                DisplayName = r.DisplayName,
                Description = r.Description,
                IsActive = r.IsActive
            })
            .FirstOrDefaultAsync(ct);

        if (role is null)
            return ResultOperation<RoleDto>.ToFailedResult("نقش یافت نشد");

        return ResultOperation<RoleDto>.ToSuccessResult(role);
    }
}
