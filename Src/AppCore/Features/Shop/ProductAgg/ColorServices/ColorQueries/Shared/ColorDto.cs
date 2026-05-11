namespace AppCore.Features.Shop.ProductAgg.ColorServices.Queries.Shared
{
    public class ColorDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }

        // Lifecycle fields
        public long CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        
        public long UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsUpdate { get; set; }

        // Soft-delete flags (consistent with AppUser, ProductBrand patterns)
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    }
}
