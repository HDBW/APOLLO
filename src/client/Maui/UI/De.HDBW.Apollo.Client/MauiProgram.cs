namespace De.HDBW.Apollo.Client;

using CommunityToolkit.Maui;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Services;
using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.Client.Views;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Identity.Client;
using Serilog;
using SkiaSharp.Views.Maui.Controls.Hosting;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        SetupLogging();
        Log.Information($"---------------------------------------- Application started at {DateTime.Now} ------------------------------------------");
        Log.Debug($"Model: {DeviceInfo.Current.Model}");
        Log.Debug($"Manufacturer: {DeviceInfo.Current.Manufacturer}");
        Log.Debug($"Name: {DeviceInfo.Name}");
        Log.Debug($"OS Version: {DeviceInfo.VersionString}");
        Log.Debug($"Refresh Rate: {DeviceInfo.Current}");
        Log.Debug($"Idiom: {DeviceInfo.Current.Idiom}");
        Log.Debug($"Platform: {DeviceInfo.Current.Platform}");
        Log.Debug($"-------------------------------------------------------------------------------------------------------------------------------");
        SetupRoutes();

        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.UseSkiaSharp();
        SetupB2CLogin(builder.Services);
        SetupServices(builder.Services);
        return builder.Build();
    }

    private static void SetupB2CLogin(IServiceCollection services)
    {
        var b2cClientApplicationBuilder = PublicClientApplicationBuilder.Create(Constants.ClientId)
#if ANDROID
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
#if WINDOWS
            .WithRedirectUri("http://localhost");
#else
            .WithRedirectUri($"msal{Constants.ClientId}://auth");
#endif

        services.AddSingleton<IAuthService>(new AuthServiceB2C(
            b2cClientApplicationBuilder
                .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
                .WithB2CAuthority(Constants.AuthoritySignIn)
                .Build()));
    }

    private static void SetupLogging()
    {
        var path = Path.Combine(FileSystem.AppDataDirectory, "logs", "log-.txt");
        if (!Directory.Exists(path))
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

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

        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IPreferenceService, PreferenceService>();
        services.AddSingleton<IDispatcherService, DispatcherService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IUseCaseBuilder, UseCaseBuilder>();

        services.AddSingleton<AppShell>();
        services.AddSingleton<AppShellViewModel>();
        services.AddTransient<StartView>();
        services.AddTransient<StartViewModel>();
        services.AddTransient<ExtendedSplashScreenView>();
        services.AddTransient<ExtendedSplashScreenViewModel>();
        services.AddTransient<RegistrationView>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<UseCaseTutorialView>();
        services.AddTransient<UseCaseTutorialViewModel>();
        services.AddTransient<UseCaseSelectionView>();
        services.AddTransient<UseCaseSelectionViewModel>();
        services.AddTransient<FirstTimeDialog>();
        services.AddTransient<FirstTimeDialogViewModel>();
        services.AddTransient<EmptyView>();
        services.AddTransient<EmptyViewModel>();
    }

    private static void SetupRoutes()
    {
        Routing.RegisterRoute(Routes.ExtendedSplashScreenView, typeof(ExtendedSplashScreenView));
        Routing.RegisterRoute(Routes.Shell, typeof(AppShell));
        Routing.RegisterRoute(Routes.RegistrationView, typeof(RegistrationView));
        Routing.RegisterRoute(Routes.UseCaseTutorialView, typeof(UseCaseTutorialView));
        Routing.RegisterRoute(Routes.UseCaseSelectionView, typeof(UseCaseSelectionView));
        Routing.RegisterRoute(Routes.StartView, typeof(StartView));

        // TBD
        Routing.RegisterRoute(Routes.EmptyView, typeof(EmptyView));
        Routing.RegisterRoute(Routes.TutorialView, typeof(EmptyView));
    }
}
