using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.DatatableModels;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ProductBrandServices.Queries.GetProductBrandsDataTableQuery
{
    public class GetProductBrandsDataTableQuery : DatatableFullRequest
    {
        public string? SearchName { get; set; }
    }

    public class ProductBrandListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetProductBrandsDataTableHandler
    {
        private readonly AppDbContext _context;

        public GetProductBrandsDataTableHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<DataTableResponse<ProductBrandListDto>>> Handle(GetProductBrandsDataTableQuery query, CancellationToken cancellationToken)
        {
            var baseQuery = _context.ProductBrands
                .AsNoTracking()
                .Where(b => !b.IsDelete && b.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchName))
            {
                baseQuery = baseQuery.Where(b => b.Name.Contains(query.SearchName));
            }

            var filteredCount = await baseQuery.CountAsync(cancellationToken);

            var dtoQuery = baseQuery.Select(b => new ProductBrandListDto
            {
                Id = b.Id,
                Name = b.Name,
                Slug = b.Slug,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt
            });

            var orderedQuery = dtoQuery.ApplyDataTableOrdering(query);

            var brands = await orderedQuery
                .Skip(query.start)
                .Take(query.length)
                .ToListAsync(cancellationToken);

            var response = new DataTableResponse<ProductBrandListDto>
            {
                Draw = query.draw,
                RecordsTotal = filteredCount,
                RecordsFiltered = filteredCount,
                Data = brands
            };
            return  response.ToSuccessResult();
        }
    }
}
