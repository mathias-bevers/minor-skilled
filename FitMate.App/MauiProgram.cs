using System.Reflection;
using CommunityToolkit.Maui;
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
}

internal class ServerSettings
{
    public bool TrustedConnection { get; set; }
    public string ConnectionString =>
        $"Server={Name};Database={Database};User Id={UserID};Password={Password};MultipleActiveResultSets=true;Encrypt=false";
    public string Database { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserID { get; set; } = null!;
}