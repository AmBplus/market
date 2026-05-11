using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Customization;

public class Slider : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? SubTitle { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? MobileImageUrl { get; set; }
    public string? LinkUrl { get; set; }
    public string? LinkText { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}
