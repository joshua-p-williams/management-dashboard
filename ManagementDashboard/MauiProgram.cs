using Microsoft.Extensions.Logging;
using ManagementDashboard.Core.Services;
using ManagementDashboard.Core.Contracts;
using ManagementDashboard.Services;
using ManagementDashboard.Data.Migrations;
using ManagementDashboard.Data.Repositories;
using Microsoft.Extensions.Configuration;

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

            // Register EisenhowerTaskRepository as scoped
            builder.Services.AddScoped<IEisenhowerTaskRepository, EisenhowerTaskRepository>();

            // Register WorkCaptureNoteRepository as scoped
            builder.Services.AddScoped<IWorkCaptureNoteRepository, WorkCaptureNoteRepository>();

            // Register TaskService and ITaskService as scoped
            builder.Services.AddScoped<ITaskService, TaskService>();

            // Register configuration for SQLite connection string
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "management-dashboard.db");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:DefaultConnection", $"Data Source={dbPath}" }
            });
            var configuration = configBuilder.Build();
            builder.Services.AddSingleton<IConfiguration>(configuration);

            // Run migrations (synchronously)
            var migrationsPath = Path.Combine(AppContext.BaseDirectory, "Migrations");
            var runner = new MigrationRunner(migrationsPath, () => new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={dbPath}"));
            runner.RunMigrations();

            return builder.Build();
        }
    }
}
