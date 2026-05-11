using AppCore.Domains.Entities.Base;

namespace AppCore.Domains.Entities.Shop.ProductAgg
{
    public class ProductBrand : BaseEntity
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}
