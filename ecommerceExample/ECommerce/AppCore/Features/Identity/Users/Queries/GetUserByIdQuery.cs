using ECommerce.AppCore.Data;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.AppCore.Features.Identity.Users.Queries;

public class GetUserByIdQuery
{
    public long Id { get; set; }
}

public class UserDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? NationalCode { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetUserByIdHandler
{
    private readonly ECommerceDbContext _context;
    public GetUserByIdHandler(ECommerceDbContext context) => _context = context;

    public async Task<ResultOperation<UserDto>> Handle(GetUserByIdQuery query, CancellationToken ct)
    {
        if (query.Id <= 0)
            return ResultOperation<UserDto>.ToFailedResult("شناسه کاربر نامعتبر است");

        var user = await _context.Users
            .Where(u => u.Id == query.Id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                FirstName = u.FirstName,
                LastName = u.LastName,
                NationalCode = u.NationalCode,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (user is null)
            return ResultOperation<UserDto>.ToFailedResult("کاربر یافت نشد");

        return ResultOperation<UserDto>.ToSuccessResult(user);
    }
}
