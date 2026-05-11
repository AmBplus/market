using AppCore.Data;
using Framework.DatatableModels;
using Framework.ResultHelper;

using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.ID.Users.Queries;

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
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RolesCount { get; set; }
    public bool IsActive{ get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetUsersDataTableHandler
{
    private readonly AppDbContext _context;

    public GetUsersDataTableHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<DataTableResponse<UserListDto>>> Handle(GetUsersDataTableQuery query)
    {
        var baseQuery = _context.Users
                        .AsNoTracking()
                        .Where(t => !t.IsDelete)
           /*   .Include(u => u.DomainRoles)  */ // اگر RoleCount به صورت eager loading نیاز دارید
                        .AsQueryable();

        // 2. فیلترهای جستجو
        if (!string.IsNullOrWhiteSpace(query.SearchUserName))
            baseQuery = baseQuery.Where(u => u.UserName.Contains(query.SearchUserName));

        if (!string.IsNullOrWhiteSpace(query.SearchEmail))
            baseQuery = baseQuery.Where(u =>  u.Email!.Contains(query.SearchEmail));

        if (!string.IsNullOrWhiteSpace(query.SearchMobile))
            baseQuery = baseQuery.Where(u => u.Mobile!.Contains(query.SearchMobile));

        if (!string.IsNullOrWhiteSpace(query.SearchFirstName))
            baseQuery = baseQuery.Where(u => u.FirstName!.Contains(query.SearchFirstName));

        if (!string.IsNullOrWhiteSpace(query.SearchLastName))
            baseQuery = baseQuery.Where(u => u.LastName!.Contains(query.SearchLastName));

        // 3. شمارش کل رکوردها
        var filteredCount = await baseQuery.CountAsync();

        // 4. انتخاب به DTO
        var dtoQuery = baseQuery.Select(u => new UserListDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email ?? string.Empty,
            Mobile = u.Mobile ?? string.Empty,
            FirstName = u.FirstName ?? string.Empty,
            LastName = u.LastName ?? string.Empty,
            RolesCount = u.DomainRoles.Count(),
            CreatedAt = u.CreatedAt,
            IsActive = u.IsActive
        });

        // 5. ترتیب‌سازی داینامیک
        var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

        // 6. صفحه‌بندی
        var users = await orderedQuery
            .Skip(query.start)
            .Take(query.length)
            .ToListAsync();

        // 7. پاسخ
        var response = new DataTableResponse<UserListDto>
        {
            Draw = query.draw,
            RecordsTotal = filteredCount,
            RecordsFiltered = filteredCount,
            Data = users
        };

        return ResultOperation<DataTableResponse<UserListDto>>.ToSuccessResult(response);

    }
}

