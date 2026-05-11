using AppCore.Data;
using AppCore.Features.Shop.ProductAgg.GuaranteeServices.Commands;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.GuaranteeServices.Queries.GetGuaranteeByIdQuery
{
    public class GuaranteeGetByIdQuery
    {
        public long Id { get; set; }
    }

    public class GuaranteeDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GuaranteeGetByIdHandler
    {
        private readonly AppDbContext _context;

        public GuaranteeGetByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<GuaranteeDetailDto>> Handle(GuaranteeGetByIdQuery query, CancellationToken cancellationToken)
        {
            var guarantee = await _context.Guarantees
                .AsNoTracking()
                .Where(g => g.Id == query.Id && !g.IsDelete)
                .Select(g => new GuaranteeDetailDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    IsActive = g.IsActive,
                    IsDelete = g.IsDelete,
                    CreatedAt = g.CreatedAt,
                    UpdatedAt = g.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (guarantee == null)
                return ResultOperation<GuaranteeDetailDto>.ToFailedResult("ضمان یافت نشد");
            
            return guarantee.ToSuccessResult();
        }
    }
}
