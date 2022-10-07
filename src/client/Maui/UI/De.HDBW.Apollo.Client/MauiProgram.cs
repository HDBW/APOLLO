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
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Serilog;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace De.HDBW.Apollo.Client;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        SetupLogging();
        builder.Logging.AddSerilog(dispose: true);
        SetupSecrets(builder.Services);
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

        SetupB2CLogin(builder.Services);
        SetupServices(builder.Services);

        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        return builder.Build();
    }

    private static IUserSecretsService SetupSecrets(IServiceCollection services)
    {
        var userSecretsService = new UserSecretsService(services.BuildServiceProvider().GetService<ILogger<UserSecretsService>>());
        userSecretsService.LoadSecrets();
        services.AddSingleton<IUserSecretsService>(userSecretsService);
        B2CConstants.ApplySecrets(userSecretsService);
        return userSecretsService;
    }

    private static void SetupB2CLogin(IServiceCollection services)
    {
        var b2cClientApplicationBuilder = PublicClientApplicationBuilder.Create(B2CConstants.ClientId)
#if ANDROID
            .WithParentActivityOrWindow(() => Platform.CurrentActivity)
#endif
#if WINDOWS
            .WithRedirectUri("http://localhost");
#else
            .WithRedirectUri($"msal{B2CConstants.ClientId}://auth");
#endif

        services.AddSingleton<IAuthService>(new AuthServiceB2C(
            b2cClientApplicationBuilder
                .WithIosKeychainSecurityGroup(B2CConstants.IosKeychainSecurityGroups)
                .WithB2CAuthority(B2CConstants.AuthoritySignIn)
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
