using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Settings;

public class EnumValue : BaseEntity
{
    public string EnumType { get; set; } = string.Empty;
    public string EnumKey { get; set; } = string.Empty;
    public int EnumId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
