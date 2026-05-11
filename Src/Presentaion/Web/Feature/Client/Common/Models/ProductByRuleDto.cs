namespace Feature.Client.Common.Models
{
    public class ProductByRuleDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? SeoUrl { get; set; }
        public string? LandingCode { get; set; }
    }


}
