// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Maui;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Services;
using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.Client.Views;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Identity.Client;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace De.HDBW.Apollo.Client;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Preferences.Default.Set(Preference.AllowTelemetry.ToString(), true);
        var builder = MauiApp.CreateBuilder();
        SetupSecrets(builder.Services);
        SetupLogging();
        builder.Logging.AddSerilog(Log.Logger, dispose: true);

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
        var userSecretsService = new UserSecretsService();
        userSecretsService.LoadSecrets();
        services.AddSingleton<IUserSecretsService>(userSecretsService);
        B2CConstants.ApplySecrets(userSecretsService);
        TelemetryConstants.ApplySecrets(userSecretsService);
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

        var session = TelemetryConstants.Configuration;

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
                .WriteTo.Conditional(IsTelemetryEnable, wt => wt.UseApplicationInsights(TelemetryConstants.Configuration, TelemetryConverter.Traces))
                .CreateLogger();
    }

    private static LoggerConfiguration UseApplicationInsights(this LoggerSinkConfiguration loggerConfiguration, TelemetryConfiguration telemetryConfiguration, ITelemetryConverter telemetryConverter, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
    {
        var telemetryClient = new TelemetryClient(telemetryConfiguration);
        telemetryClient.Context.Device.OemName = DeviceInfo.Manufacturer;
        telemetryClient.Context.Device.Model = DeviceInfo.Current.Model;
        telemetryClient.Context.Device.Type = DeviceInfo.Current.DeviceType.ToString();
        telemetryClient.Context.Device.OperatingSystem = DeviceInfo.Current.Platform.ToString();
        telemetryClient.Context.Session.Id = TelemetryConstants.SessionId;
        return loggerConfiguration.Sink(new ApplicationInsightsSink(telemetryClient, telemetryConverter), restrictedToMinimumLevel, levelSwitch);
    }

    // TODO: Show dialog to get permission or if not needed return true,
    private static bool IsTelemetryEnable(LogEvent arg)
    {
        return Preferences.Default.Get(Preference.AllowTelemetry.ToString(), false);
    }

    private static void SetupServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton((s) => { return Preferences.Default; });

        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IPreferenceService, PreferenceService>();
        services.AddSingleton<IDispatcherService, DispatcherService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IUseCaseBuilder, UseCaseBuilder>();
        services.AddSingleton<IFeedbackService, FeedbackService>();

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
