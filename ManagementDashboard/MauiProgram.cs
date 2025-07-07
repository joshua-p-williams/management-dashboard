using Microsoft.Extensions.Logging;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Services;

namespace ManagementDashboard
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IAppPreferences, MauiPreferences>();
            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddSingleton<ISettingsService>(sp => sp.GetRequiredService<SettingsService>());

            return builder.Build();
        }
    }
}
