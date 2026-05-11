namespace Framework.Settings.AppSettings;


public class ApplicationSettings
{
  public static string KeyName { get => nameof(ApplicationSettings); }

  public ApplicationSettings() : base()
  {
    IconSettings =
        new IconSettings();

    ToastSettings =
        new ToastSettings();

    CultureSettings =
        new CultureSettings();


  }

  // **********
  public string? Version { get; set; }
  // **********

  // **********
  public string? MasterPassword { get; set; }
  // **********

  // **********
  public string[]? ActivationKeys { get; set; }
  // **********

  // **********
  public IconSettings IconSettings { get; set; }
  // **********

  // **********
  public ToastSettings ToastSettings { get; set; }
  // **********

  // **********
  public CultureSettings CultureSettings { get; set; }

  public string? AssetBasePath { get; set; }
  public string? ProfileBasePath { get; set; }
  public string? AvatarBaseRootPath { get; set; }
  public string? AvatarUploadUrl { get; set; }
  public string? CourseBasePathContent { get; set; }
  public string? ReportUrl { get; set; }
  public string? ContentRootPath { get; set; }

}
