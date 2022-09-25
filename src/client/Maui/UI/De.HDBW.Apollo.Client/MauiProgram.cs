namespace De.HDBW.Apollo.Client;

using CommunityToolkit.Maui;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Services;
using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.Client.Views;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Services;
using Serilog;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        SetupLogging();
        SetupRoutes();
        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        SetupServices(builder.Services);
        return builder.Build();
    }

    private static void SetupLogging()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "logs", "log-.txt");

        Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
                .Enrich.WithMemoryUsage()
                .Enrich.WithThreadId()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, retainedFileCountLimit: 5)
#else
                .MinimumLevel.Debug()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, retainedFileCountLimit: 20)
#endif
                .CreateLogger();
    }

    private static void SetupServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton((s) => { return Preferences.Default; });
        services.AddSingleton<IPreferenceService, PreferenceService>();
        services.AddSingleton<IDispatcherService, DispatcherService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<AppShell>();
        services.AddSingleton<AppShellViewModel>();
        services.AddTransient<StartView>();
        services.AddTransient<StartViewModel>();
        services.AddTransient<ExtendedSplashScreenView>();
        services.AddTransient<ExtendedSplashScreenViewModel>();
        services.AddTransient<FirstTimeDialog>();
        services.AddTransient<FirstTimeDialogViewModel>();
        services.AddTransient<EmptyView>();
        services.AddTransient<EmptyViewModel>();
    }

    private static void SetupRoutes()
    {
        Routing.RegisterRoute(Routes.Shell, typeof(AppShell));
        Routing.RegisterRoute(Routes.EmptyView, typeof(EmptyView));
        Routing.RegisterRoute(Routes.StartView, typeof(StartView));
        Routing.RegisterRoute(Routes.TutorialView, typeof(EmptyView));
        Routing.RegisterRoute(Routes.ExtendedSplashScreenView, typeof(ExtendedSplashScreenView));
    }
}
