using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Extensions.Msal;

public class Settings
{
    public const string ClientId = "942fa698-32d1-4650-a74e-c1843804dd3c";
    public const string TenantId = "common";
    public static readonly string[] GraphUserScopes = {"user.read", "calendars.readwrite.shared", "offline_access"};
    public const string apiUri = "http://localhost";
    public const string CacheFileName = "MonCal_msal_cache.txt";
    public readonly static string CacheDir = MsalCacheHelper.UserRootDirectory;
    public const string Authority = "https://login.microsoftonline.com/common";

    // public static Settings LoadSettings()
    // {
    //     // //Load settings
    //     // IConfiguration config = new ConfigurationBuilder()
    //     //     .SetBasePath(Directory.GetCurrentDirectory())
    //     //     // appsettings.json is required
    //     //     .AddJsonFile("msgraph/msgraph_appsettings.json", optional: false)
    //     //     // appsettings.Development.json" is optional, values override appsettings.json
    //     //     .AddJsonFile($"appsettings.Development.json", optional: true)
    //     //     // User secrets are optional, values override both JSON files
    //     //     .AddUserSecrets<MonCal.Program>()
    //     //     .Build();

    //     // return config.GetRequiredSection("Settings").Get<Settings>() ??
    //     //     throw new Exception("Could not load app settings.");

    //     Settings _settings = new Settings();
    //     return _settings;

    // }
}