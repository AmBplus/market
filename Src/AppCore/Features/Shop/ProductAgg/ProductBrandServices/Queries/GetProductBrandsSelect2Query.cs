using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ProductBrandServices.Queries.GetProductBrandsSelect2Query
{
    public class GetProductBrandsSelect2Query
    {
        public string? SearchName { get; set; }
    }

    public class ProductBrandSelect2Dto
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }

    public class GetProductBrandsSelect2QueryHandler
    {
        private readonly AppDbContext _context;

        public GetProductBrandsSelect2QueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<List<ProductBrandSelect2Dto>>> Handle(GetProductBrandsSelect2Query query, CancellationToken cancellationToken)
        {
            var baseQuery = _context.ProductBrands
                .AsNoTracking()
                .Where(b => !b.IsDelete && b.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchName))
            {
                baseQuery = baseQuery.Where(b => b.Name.Contains(query.SearchName));
            }

            var brands = await baseQuery.Select(b => new ProductBrandSelect2Dto
            {
                Id = b.Id,
                Text = b.Name
            }).ToListAsync(cancellationToken);

           
            return ResultOperation<List<ProductBrandSelect2Dto>>.ToSuccessResult(brands);
        }
    }
}
