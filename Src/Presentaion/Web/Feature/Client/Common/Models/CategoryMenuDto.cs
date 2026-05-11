namespace Feature.Client.Common.Models
{
    public class CategoryMenuDto
    {
        public int Id { get; set; }
        public string? SeoUrl { get; set; }
        public string? Title { get; set; }
        public int LanguageId { get; set; }
        public int? ParentId { get; set; }
        public string? IconUrl { get; set; }
        public string? IconContent { get; set; }
    }


}
