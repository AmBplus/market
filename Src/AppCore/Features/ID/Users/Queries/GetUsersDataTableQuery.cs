using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Identity.Users.Queries;

public class GetUsersDataTableQuery : DatatableFullRequest
{
    public string? SearchUserName { get; set; }
    public string? SearchEmail { get; set; }
    public string? SearchMobile { get; set; }
    public string? SearchFirstName { get; set; }
    public string? SearchLastName { get; set; }
}

public class UserListDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int RolesCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetUsersDataTableHandler
{
    private readonly AppDbContext _context;
    public GetUsersDataTableHandler(AppDbContext context) => _context = context;

    public async Task<ResultOperation<DataTableResponse<UserListDto>>> Handle(GetUsersDataTableQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchUserName))
            baseQuery = baseQuery.Where(u => u.UserName.Contains(query.SearchUserName));

        if (!string.IsNullOrWhiteSpace(query.SearchEmail))
            baseQuery = baseQuery.Where(u => u.Email != null && u.Email.Contains(query.SearchEmail));

        if (!string.IsNullOrWhiteSpace(query.SearchMobile))
            baseQuery = baseQuery.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(query.SearchMobile));

        if (!string.IsNullOrWhiteSpace(query.SearchFirstName))
            baseQuery = baseQuery.Where(u => u.FirstName.Contains(query.SearchFirstName));

        if (!string.IsNullOrWhiteSpace(query.SearchLastName))
            baseQuery = baseQuery.Where(u => u.LastName.Contains(query.SearchLastName));

        var totalRecords = await baseQuery.CountAsync(ct);

        var dataQuery = baseQuery
            .Select(u => new UserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Mobile = u.PhoneNumber,
                FirstName = u.FirstName,
                LastName = u.LastName,
                RolesCount = u.UserRoles.Count,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });

        dataQuery = dataQuery.ApplyDataTableOrdering(query);

        var pageSize = query.GetPageSize();
        var pageNumber = query.GetPageNumber();

        var data = pageSize == int.MaxValue
            ? await dataQuery.ToListAsync(ct)
            : await dataQuery.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(ct);

        var response = new DataTableResponse<UserListDto>
        {
            Draw = query.draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords,
            Data = data
        };

        return ResultOperation<DataTableResponse<UserListDto>>.ToSuccessResult(response);
    }
}
