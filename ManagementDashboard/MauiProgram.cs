using Blazorise;
using Blazorise.FluentUI2;
using Microsoft.Extensions.Logging;

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
                });

            builder.Services.AddMauiBlazorWebView();

            // Register Blazorise Fluent 2
            builder.Services
                .AddBlazorise(options => { options.Immediate = true; })
                .AddFluentUI2Providers();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
