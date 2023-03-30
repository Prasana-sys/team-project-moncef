using Microsoft.Extensions.Configuration;

public class Settings
{
    public string? ClientId { get; set; }
    public string? TenantId { get; set; }
    public string[]? GraphUserScopes { get; set; }

    public static Settings LoadSettings()
    {
        // Load settings
        // IConfiguration config = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     // appsettings.json is required
        //     .AddJsonFile("msgraph/msgraph_appsettings.json", optional: false)
        //     // appsettings.Development.json" is optional, values override appsettings.json
        //     .AddJsonFile($"appsettings.Development.json", optional: true)
        //     // User secrets are optional, values override both JSON files
        //     .AddUserSecrets<MonCal.Program>()
        //     .Build();

        // return config.GetRequiredSection("Settings").Get<Settings>() ??
        //     throw new Exception("Could not load app settings.");

        Settings _settings = new Settings
        {
            ClientId = "942fa698-32d1-4650-a74e-c1843804dd3c",
            TenantId = "common",
            GraphUserScopes = new string[] {"user.read", "calendars.readwrite.shared"}
        };

        return _settings;

    }
}