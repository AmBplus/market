using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Identity.Roles.Queries;

public class SelectRoleQuery
{
    public string? SearchName { get; set; }
}

public class RoleSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectRoleHandler
{
    private readonly ECommerceDbContext _context;
    public SelectRoleHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<RoleSelect2Dto>>> Handle(SelectRoleQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(r => r.Name.Contains(query.SearchName) || r.DisplayName.Contains(query.SearchName));

        var result = await baseQuery
            .Select(r => new RoleSelect2Dto
            {
                Id = r.Id,
                Text = r.DisplayName
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<RoleSelect2Dto>>.ToSuccessResult(result);
    }
}
