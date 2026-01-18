using System.Reflection;
using CommunityToolkit.Maui;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace FitMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit().ConfigureSyncfusionCore().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        Assembly a = typeof(App).Assembly;
        using Stream? stream = a.GetManifestResourceStream("FitMate.appsettings.json");
        if (stream != null)
        {
            IConfigurationRoot config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);
        }


#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddTransient<App>();

        return builder.Build();
    }
}

internal class Settings
{
    public ServerSettings Server { get; set; } = null!;
    public string SyncfusionAPI { get; set; } = null!;
    public string EncryptionKey { get; set; } = null!;
}

internal class ServerSettings
{
    public string Database { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserID { get; set; } = null!;
    public int ConnectionTimeout { get; set; } = 30;
    
    private string? connectionString = null;

    public string ConnectionString => connectionString ??= new SqlConnectionStringBuilder()
        {
            DataSource = Name,
            InitialCatalog = Database,
            UserID = UserID,
            Password = Password,
            ConnectTimeout = ConnectionTimeout,
            TrustServerCertificate = true,
            Encrypt = true
        }.ConnectionString;
}