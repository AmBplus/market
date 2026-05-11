using AppCore.Data;
using AppCore.Domains.Entities.ID;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.ID.Users.Queries
{
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
        public string Text{ get; set; }
    }

    public class SelectUserQueryHandler
    {
        private readonly AppDbContext _context;

        public SelectUserQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<List<UserSelect2Dto>>> Handle(SelectUserQuery query, CancellationToken cancellationToken)
        {
            var baseQuery = _context.Users
                .AsNoTracking()
                .Where(u => !u.IsDelete && u.IsActive)
                .AsQueryable();

            // Search filters
            if (!string.IsNullOrWhiteSpace(query.SearchName))
                baseQuery = baseQuery.Where(u => u.FirstName!=null && u.FirstName.Contains(query.SearchName));

            if (!string.IsNullOrWhiteSpace(query.SearchFamily))
                baseQuery = baseQuery.Where(u => u.LastName != null && u.LastName!.Contains(query.SearchFamily));

            if (!string.IsNullOrWhiteSpace(query.SearchUserName))
                baseQuery = baseQuery.Where(u => u.UserName.Contains(query.SearchUserName));

            if (!string.IsNullOrWhiteSpace(query.SearchMobile))
                baseQuery = baseQuery.Where(u => u.Mobile != null && u.Mobile!.Contains(query.SearchMobile));

            if (!string.IsNullOrWhiteSpace(query.SearchNationalCode))
                baseQuery = baseQuery.Where(u => u.NationalId != null && u.NationalId!.Contains(query.SearchNationalCode));

            // Get all matching records (Select2 needs full list)
            var users = await baseQuery.ToListAsync(cancellationToken);

            var result = users.Select(u => new UserSelect2Dto
            {
                Id = u.Id,
                Text = $"{u.FirstName} {u.LastName}-{u.Mobile}-{u.NationalId}",
            }).ToList();

            return ResultOperation<List<UserSelect2Dto>>.ToSuccessResult(result);
        }
    }
}
