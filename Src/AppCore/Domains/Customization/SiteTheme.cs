using AppCore.Domains.Common;

namespace AppCore.Domains.Customization;

public class SiteTheme : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#3B82F6";
    public string SecondaryColor { get; set; } = "#10B981";
    public string BackgroundColor { get; set; } = "#FFFFFF";
    public string TextColor { get; set; } = "#1F2937";
    public string AccentColor { get; set; } = "#F59E0B";
    public string? FontFamily { get; set; }
    public string? ExtendedSettingsJson { get; set; }
    public bool IsActive { get; set; }
}
