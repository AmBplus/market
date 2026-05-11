using AppCore.Data;
using AppCore.Features.Shop.ProductAgg.ColorServices.Queries.Shared;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ColorServices.Queries
{
    public record ColorGetByIdQuery(long Id);
    public class ColorGetByIdHandler
    {
        private readonly AppDbContext _context;

        public ColorGetByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<ColorDto>> Handle(ColorGetByIdQuery query, CancellationToken cancellationToken)
        {
            var color = await _context.Colors
                .AsNoTracking()
                .Where(c => c.Id == query.Id && !c.IsDelete)
                .Select(c => new ColorDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    ColorCode = c.ColorCode,
                    CreateBy = c.CreateBy,
                    CreateAt = c.CreateAt,
                    UpdateBy = c.UpdateBy,
                    UpdateAt = c.UpdateAt,
                    IsUpdate = c.IsUpdate,
                    IsActive = c.IsActive,
                    IsDelete = c.IsDelete
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (color == null)
                return ResultOperation<ColorDto>.ToFailedResult("رنگ یافت نشد");

            return color.ToSuccessResult();
        }
    }
}
