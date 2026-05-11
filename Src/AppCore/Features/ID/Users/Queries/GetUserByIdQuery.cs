using AppCore.Data;
using AppCore.Features.ID.Users.Queries.Shared;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.ID.Users.Queries;

public record GetUserByIdQuery(long UserId);

public class GetUserByIdHandler
{
    private readonly AppDbContext _context;

    public GetUserByIdHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResultOperation<UserDto>> Handle(GetUserByIdQuery query)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.DomainRoles)
                .ThenInclude(dr => dr.Role)
            .Where(u => u.Id == query.UserId)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Mobile = u.Mobile,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FatherName = u.FatherName,
                PassportNumber = u.PassportNumber,
                Nationality = u.Nationality,
                NationalId =u.NationalId,
                IsActive = u.IsActive,
                IsDelete = u.IsDelete,
                BirthDate = u.BirthDate,
                Address = u.Address,
                Avatar = u.Avatar,
                // نقش‌ها
                RolesDomain = u.DomainRoles.Select(dr => new RoleDomainDto
                    {
                        Id = dr.Id,
                        UserId = dr.UserId,
                        RoleId = dr.RoleId,
                        RoleName = dr.Role!.Title
                    }).ToList()

            })
            .FirstOrDefaultAsync();

        if (user == null)
            return ResultOperation<UserDto>.ToFailedResult("کاربر یافت نشد");
        return user.ToSuccessResult();
    }
}
