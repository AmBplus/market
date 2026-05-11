namespace Framework.Settings.AppSettings
{
    public class CultureSettings : object
    {
        public CultureSettings() : base()
        {
        }

        // **********
        public string? DefaultCultureName { get; set; }
        // **********

        // **********
        public string[]? SupportedCultureNames { get; set; }
        // **********
    }
}
