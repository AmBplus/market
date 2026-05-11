using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Roles.Queries;

public class GetRolesDataTableQuery : DatatableFullRequest
{
    public string? SearchName { get; set; }
    public string? SearchDisplayName { get; set; }
}

public class RoleListDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetRolesDataTableHandler
{
    private readonly AppDbContext _context;
    public GetRolesDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<RoleListDto>>> Handle(GetRolesDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Roles.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(r => r.Name.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchDisplayName))
            baseQuery = baseQuery.Where(r => r.DisplayName.Contains(query.SearchDisplayName));

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(r => new RoleListDto
            {
                Id = r.Id,
                Name = r.Name,
                DisplayName = r.DisplayName,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<RoleListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<RoleListDto>>.ToSuccessResult(response);
    }
}
