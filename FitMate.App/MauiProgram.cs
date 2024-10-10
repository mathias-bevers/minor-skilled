using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FitMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
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

internal class ServerSettings
{
    public string ConnectionString =>
        $"Server={Name};Database={Database};User Id={UserID};Password={Password};MultipleActiveResultSets=true;Encrypt=false";

    public bool TrustedConnection { get; set; }
    public string Database { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string UserID { get; set; } = null!;
    public string Password { get; set; } = null!;
}