using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Customization;

public class Widget : AuditableEntity
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ConfigJson { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}
