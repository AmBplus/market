using AppCore.Data;
using AppCore.Domains.Entities.Shop.ProductAgg;
using Framework.ResultHelper;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Features.Shop.ProductAgg.ProductBrandServices.Queries.GetProductBrandByIdQuery
{
    public class GetProductBrandByIdQuery
    {
        public required long Id { get; set; }
    }

    public class ProductBrandDetailDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetProductBrandByIdQueryHandler
    {
        private readonly AppDbContext _context;

        public GetProductBrandByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultOperation<ProductBrandDetailDto>> Handle(GetProductBrandByIdQuery query, CancellationToken cancellationToken)
        {
            var brand = await _context.ProductBrands.FindAsync(query.Id);
            if (brand == null)
            {
                return ResultOperation<ProductBrandDetailDto>.ToFailedResult("Product brand not found");
            }
            var brandDto = new ProductBrandDetailDto
            {
                Id = brand.Id,
                Name = brand.Name,
                Slug = brand.Slug,
                Description = brand.Description,
                LogoUrl = brand.LogoUrl,
                WebsiteUrl = brand.WebsiteUrl,
                IsActive = brand.IsActive
            };
            return brandDto.ToSuccessResult();
        }
    }
}
