namespace AppCore.Features.Shop.ProductAgg.ColorServices.Queries.Shared;

public class ColorDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
