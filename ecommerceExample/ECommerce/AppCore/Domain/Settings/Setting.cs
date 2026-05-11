using ECommerce.AppCore.Domain.Common;

namespace ECommerce.AppCore.Domain.Settings;

public class Setting : AuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ValueType { get; set; } = "String";
    public bool IsEditable { get; set; } = true;
}
