using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Identity.Users.Queries;

public class SelectUserQuery
{
    public string? SearchMobile { get; set; }
    public string? SearchName { get; set; }
    public string? SearchFamily { get; set; }
    public string? SearchUserName { get; set; }
    public string? SearchNationalCode { get; set; }
}

public class UserSelect2Dto
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class SelectUserHandler
{
    private readonly ECommerceDbContext _context;
    public SelectUserHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<List<UserSelect2Dto>>> Handle(SelectUserQuery query, CancellationToken ct)
    {
        var baseQuery = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchMobile))
            baseQuery = baseQuery.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(query.SearchMobile));

        if (!string.IsNullOrWhiteSpace(query.SearchName))
            baseQuery = baseQuery.Where(u => u.FirstName.Contains(query.SearchName));

        if (!string.IsNullOrWhiteSpace(query.SearchFamily))
            baseQuery = baseQuery.Where(u => u.LastName.Contains(query.SearchFamily));

        if (!string.IsNullOrWhiteSpace(query.SearchUserName))
            baseQuery = baseQuery.Where(u => u.UserName.Contains(query.SearchUserName));

        if (!string.IsNullOrWhiteSpace(query.SearchNationalCode))
            baseQuery = baseQuery.Where(u => u.NationalCode != null && u.NationalCode.Contains(query.SearchNationalCode));

        var result = await baseQuery
            .Select(u => new UserSelect2Dto
            {
                Id = u.Id,
                Text = u.FirstName + " " + u.LastName + "-" + (u.PhoneNumber ?? "") + "-" + (u.NationalCode ?? "")
            })
            .Take(50)
            .ToListAsync(ct);

        return ResultOperation<List<UserSelect2Dto>>.ToSuccessResult(result);
    }
}
