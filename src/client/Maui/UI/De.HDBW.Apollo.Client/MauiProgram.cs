// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.ComponentModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Services;
using De.HDBW.Apollo.Client.ViewModels;
using De.HDBW.Apollo.Client.ViewModels.Profile;
using De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors;
using De.HDBW.Apollo.Client.ViewModels.Profile.ContactInfoEditors;
using De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors;
using De.HDBW.Apollo.Client.ViewModels.Profile.LanguageEditors;
using De.HDBW.Apollo.Client.ViewModels.Profile.LicenseEditors;
using De.HDBW.Apollo.Client.ViewModels.Profile.QualificationEditors;
using De.HDBW.Apollo.Client.ViewModels.Profile.WebReferenceEditors;
using De.HDBW.Apollo.Client.ViewModels.Training;
using De.HDBW.Apollo.Client.Views;
using De.HDBW.Apollo.Client.Views.Profile;
using De.HDBW.Apollo.Client.Views.Profile.CareerInfo;
using De.HDBW.Apollo.Client.Views.Profile.EducationInfo;
using De.HDBW.Apollo.Client.Views.Profile.Language;
using De.HDBW.Apollo.Client.Views.Profile.License;
using De.HDBW.Apollo.Client.Views.Profile.Qualification;
using De.HDBW.Apollo.Client.Views.Profile.WebReference;
using De.HDBW.Apollo.Client.Views.Training;
using De.HDBW.Apollo.Data;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.Data.Repositories;
using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Models;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using FedericoNembrini.Maui.CustomDatePicker;
using GrpcClient.Service;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Handlers;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;
using SkiaSharp.Views.Maui.Controls.Hosting;
using The49.Maui.BottomSheet;

namespace De.HDBW.Apollo.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Preferences.Default.Set(Preference.AllowTelemetry.ToString(), true);
            var builder = MauiApp.CreateBuilder();
            var secretsService = SetupSecrets(builder.Services);
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
            SetupHandler();
            var authenticationResult = SetupB2CLogin(builder.Services);
            SetupServices(builder.Services, secretsService, authenticationResult);
            SetupDataBaseTableProvider(builder);
            SetupRepositories(builder.Services);
            SetupViewsAndViewModels(builder.Services);
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCompatibility()
                .UseCustomDatePicker()
                .UseSkiaSharp()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("NotoSans-Regular.ttf", "NotoSansRegular");
                    fonts.AddFont("NotoSans-SemiBold.ttf", "NotoSansSemiBold");
                    fonts.AddFont("NotoSans-Bold.ttf", "NotoSansBold");
                    fonts.AddFont("NotoSerif-Regular.ttf", "NotoSerifRegular");
                    fonts.AddFont("NotoSerif-Bold.ttf", "NotoSerifBold");
                })
            .UseBottomSheet();
            builder.ConfigureMauiHandlers(SetupHandlers);
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

        private static AuthenticationResult? SetupB2CLogin(IServiceCollection services)
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
            IAuthService authService;
            try
            {
                authService = new AuthServiceB2C(
                b2cClientApplicationBuilder
                    .WithIosKeychainSecurityGroup(B2CConstants.IosKeychainSecurityGroups)
                    .WithB2CAuthority(B2CConstants.AuthoritySignIn)
                    .Build());
            }
            catch (Exception ex)
            {
                Log.Error($"Unknow Error while registering B2C Auth in {nameof(MauiProgram)}. ClientId is {B2CConstants.ClientId}. AuthoritySignIn is {B2CConstants.AuthoritySignIn}.Error was Message:{ex.Message} Stacktrace:{ex.StackTrace}.");
                authService = new AuthServiceB2C(null!);
            }

            AuthenticationResult? authenticationResult = null;
            services.AddSingleton<IAuthService>(authService);
            try
            {
                var task = Task.Run(() => authService.AcquireTokenSilent(CancellationToken.None));
                authenticationResult = task.GetAwaiter().GetResult();
                try
                {
                    Log.Debug("Token :" + SerializationHelper.Serialize(authenticationResult));
                }
                catch
                {
                    Log.Debug($"Token? :{authenticationResult?.AccessToken}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Unknow Error while AcquireTokenSilent in {nameof(MauiProgram)}. Error was Message:{ex.Message} Stacktrace:{ex.StackTrace}.");
            }

            return authenticationResult;
        }

        private static void SetupDataBaseTableProvider(MauiAppBuilder builder)
        {
            var dbFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "db.sqlite");
            var flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;
            var localEntities = new List<Type>()
            {
                typeof(SearchHistory),
            };
            builder.Services.AddSingleton<IDataBaseConnectionProvider>(new DataBaseConnectionProvider(dbFilePath, flags, IocServiceHelper.ServiceProvider?.GetService<ILogger<DataBaseConnectionProvider>>(), localEntities));
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
                    .MinimumLevel.Verbose()
                    .Enrich.WithMemoryUsage()
                    .Enrich.WithThreadId()
                    .WriteTo.Debug()
                    .WriteTo.File(path, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, retainedFileCountLimit: 5)
#else
                    .MinimumLevel.Verbose()
                    .Enrich.WithMemoryUsage()
                    .Enrich.WithThreadId()
                    .WriteTo.File(path, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, retainedFileCountLimit: 20)
#endif
                    .WriteTo.Conditional(IsTelemetryEnable, wt => wt.UseApplicationInsights(TelemetryConstants.Configuration, TelemetryConverter.Traces))
                    .CreateLogger();
        }

        private static LoggerConfiguration UseApplicationInsights(this LoggerSinkConfiguration loggerConfiguration, TelemetryConfiguration telemetryConfiguration, ITelemetryConverter telemetryConverter, LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose, LoggingLevelSwitch? levelSwitch = null)
        {
            var telemetryClient = new TelemetryClient(telemetryConfiguration);
            telemetryClient.Context.Location.Ip = "0.0.0.0";

            telemetryClient.Context.Device.OemName = DeviceInfo.Manufacturer;
            telemetryClient.Context.Device.Model = DeviceInfo.Current.Model;
            telemetryClient.Context.Device.Type = DeviceInfo.Current.DeviceType.ToString();
            telemetryClient.Context.Device.OperatingSystem = DeviceInfo.Current.Platform.ToString();
            telemetryClient.Context.Session.Id = TelemetryConstants.SessionId;
            return loggerConfiguration.Sink(new ApplicationInsightsSink(telemetryClient, telemetryConverter), restrictedToMinimumLevel, levelSwitch);
        }

        private static bool IsTelemetryEnable(LogEvent arg)
        {
            return Preferences.Default.Get(Preference.AllowTelemetry.ToString(), false);
        }

        private static void SetupServices(IServiceCollection services, IUserSecretsService userSecretsService, AuthenticationResult? authenticationResult)
        {
            services.AddSingleton<IImageCacheService, ImageCacheService>();
            services.AddSingleton((s) => { return Preferences.Default; });
            services.AddSingleton<IPreferenceService, PreferenceService>();
            services.AddSingleton<IDispatcherService, DispatcherService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ISessionService>(new SessionService(authenticationResult?.AccessToken, authenticationResult?.Account.HomeAccountId));
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ISheetService, SheetService>();
            services.AddSingleton<IUseCaseBuilder, UseCaseBuilder>();
            services.AddSingleton<IFeedbackService, FeedbackService>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>();
            services.AddSingleton<INetworkService, NetworkService>();
            services.AddSingleton<ITouchService, TouchService>();
            services.AddSingleton<IAssessmentScoreService, AssessmentScoreService>();

            var occupationSearchUrl = userSecretsService["OccupationSearchAPIURL"] ?? string.Empty;
            services.AddSingleton<IOccupationService>((serviceProvider) =>
            {
                return new OccupationService(occupationSearchUrl, serviceProvider.GetService<ILogger<OccupationService>>());
            });

            var apiUrl = userSecretsService["SwaggerAPIURL"] ?? string.Empty;
            var apiToken = userSecretsService["SwaggerAPIToken"] ?? string.Empty;
            services.AddSingleton<ITrainingService>((serviceProvider) =>
            {
                var handler = new ContentLoggingHttpMessageHandler(serviceProvider.GetService<ILogger<ContentLoggingHttpMessageHandler>>()!);
                return new TrainingService(serviceProvider.GetService<ILogger<TrainingService>>(), apiUrl, apiToken, handler);
            });

            services.AddSingleton<IProfileService>((serviceProvider) =>
            {
                var handler = new ContentLoggingHttpMessageHandler(serviceProvider.GetService<ILogger<ContentLoggingHttpMessageHandler>>()!);
                var service = new ProfileService(serviceProvider.GetService<ILogger<ProfileService>>(), apiUrl, apiToken, handler);
                service.UpdateAuthorizationHeader(authenticationResult?.CreateAuthorizationHeader());
                return service;
            });

            services.AddSingleton<IApolloListService>((serviceProvider) =>
            {
                var handler = new ContentLoggingHttpMessageHandler(serviceProvider.GetService<ILogger<ContentLoggingHttpMessageHandler>>()!);
                var service = new ApolloListService(serviceProvider.GetService<ILogger<ApolloListService>>(), apiUrl, apiToken, handler);
                service.UpdateAuthorizationHeader(authenticationResult?.CreateAuthorizationHeader());

                return service;
            });

            var unregisterUserUrl = userSecretsService["UnRegisteUserEndpointURL"] ?? string.Empty;
            services.AddSingleton<IUnregisterUserService>((serviceProvider) =>
            {
                var handler = new ContentLoggingHttpMessageHandler(serviceProvider.GetService<ILogger<ContentLoggingHttpMessageHandler>>()!);
                var service = new UnregisterUserService(serviceProvider.GetService<ILogger<UnregisterUserService>>(), apiUrl, apiToken, handler);
                service.UpdateAuthorizationHeader(authenticationResult?.CreateAuthorizationHeader());

                return service;
            });

            services.AddSingleton<IUserService>((serviceProvider) =>
            {
#if DEBUG

                var handler = new ContentLoggingHttpMessageHandler(serviceProvider.GetService<ILogger<ContentLoggingHttpMessageHandler>>()!);
                var service = new UserService(serviceProvider.GetService<ILogger<UserService>>(), apiUrl, apiToken, serviceProvider.GetService<IProfileService>(), new MockUserHttpClientHandler(serviceProvider.GetService<IUserRepository>()));
#else
                var handler = new ContentLoggingHttpMessageHandler(serviceProvider.GetService<ILogger<ContentLoggingHttpMessageHandler>>()!);
                var service = new UserService(serviceProvider.GetService<ILogger<UserService>>(), apiUrl, apiToken, serviceProvider.GetService<IProfileService>(), handler);
#endif
                service.UpdateAuthorizationHeader(authenticationResult?.CreateAuthorizationHeader());
                return service;
            });
        }

        private static void SetupRepositories(IServiceCollection services)
        {
            services.AddSingleton<IAssessmentItemRepository, AssessmentItemRepository>();
            services.AddSingleton<IAssessmentScoreRepository, AssessmentScoreRepository>();
            services.AddSingleton<IAssessmentCategoryRepository, AssessmentCategoryRepository>();
            services.AddSingleton<IAssessmentCategoryResultRepository, AssessmentCategoryResultRepository>();
            services.AddSingleton<IQuestionItemRepository, QuestionItemRepository>();
            services.AddSingleton<IAnswerItemResultRepository, AnswerItemResultRepository>();
            services.AddSingleton<IAnswerItemRepository, AnswerItemRepository>();
            services.AddSingleton<IQuestionMetaDataRelationRepository, QuestionMetaDataRelationRepository>();
            services.AddSingleton<IAnswerMetaDataRelationRepository, AnswerMetaDataRelationRepository>();
            services.AddSingleton<IMetaDataMetaDataRelationRepository, MetaDataMetaDataRelationRepository>();
            services.AddSingleton<IMetaDataRepository, MetaDataRepository>();
            services.AddSingleton<ICourseItemRepository, CourseItemRepository>();
            services.AddSingleton<ICourseContactRepository, CourseContactRepository>();
            services.AddSingleton<ICourseAppointmentRepository, CourseAppointmentRepository>();
            services.AddSingleton<ICourseContactRelationRepository, CourseContactRelationRepository>();
            services.AddSingleton<IEduProviderItemRepository, EduProviderItemRepository>();
            services.AddSingleton<ICategoryRecomendationItemRepository, CategoryRecomendationItemRepository>();
            services.AddSingleton<IUserRepository>((serviceProvider) =>
            {
                return new UserRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"User_{Guid.Empty}.json"), serviceProvider.GetService<ILogger<UserRepository>>());
            });

            services.AddSingleton<IFavoriteRepository>((serviceProvider) =>
            {
                return new FavoriteRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"Favorite_{Guid.Empty}.json"), serviceProvider.GetService<ILogger<FavoriteRepository>>());
            });

            services.AddSingleton<ISearchHistoryRepository, SearchHistoryRepository>();
        }

        private static void SetupViewsAndViewModels(IServiceCollection services)
        {
            services.AddTransient<AppShell>();
            services.AddTransient<AppShellViewModel>();

            services.AddTransient<StartView>();
            services.AddTransient<StartViewModel>();

            services.AddTransient<ExtendedSplashScreenView>();
            services.AddTransient<ExtendedSplashScreenViewModel>();

            services.AddTransient<RegistrationView>();
            services.AddTransient<RegistrationViewModel>();

            services.AddTransient<PickUserNameView>();
            services.AddTransient<PickUserNameViewModel>();

            services.AddTransient<UseCaseDescriptionView>();
            services.AddTransient<UseCaseDescriptionViewModel>();

            services.AddTransient<UseCaseSelectionView>();
            services.AddTransient<UseCaseSelectionViewModel>();

            services.AddTransient<CancelAssessmentDialog>();
            services.AddTransient<CancelAssessmentDialogViewModel>();

            services.AddTransient<SkipQuestionDialog>();
            services.AddTransient<SkipQuestionDialogViewModel>();

            services.AddTransient<ConfirmDataUsageDialog>();
            services.AddTransient<ConfirmDataUsageDialogViewModel>();

            services.AddTransient<MessageDialog>();
            services.AddTransient<ErrorDialog>();
            services.AddTransient<MessageDialogViewModel>();

            services.AddTransient<RetryDialog>();
            services.AddTransient<ConfirmCancelDialogViewModel>();

            services.AddTransient<SelectOptionDialog>();
            services.AddTransient<SelectOptionDialogViewModel>();

            services.AddTransient<EmptyView>();
            services.AddTransient<EmptyViewModel>();

            services.AddTransient<AssessmentView>();
            services.AddTransient<AssessmentViewModel>();

            services.AddTransient<AssessmentResultView>();
            services.AddTransient<AssessmentResultViewModel>();

            services.AddTransient<AssessmentDescriptionView>();
            services.AddTransient<AssessmentDescriptionViewModel>();

            services.AddTransient<SettingsView>();
            services.AddTransient<SettingsViewModel>();

            services.AddTransient<FavoriteView>();
            services.AddTransient<FavoriteViewModel>();

            // Training
            services.AddTransient<TrainingView>();
            services.AddTransient<TrainingViewModel>();

            services.AddTransient<LoansView>();
            services.AddTransient<LoansViewModel>();

            services.AddTransient<AppointmentsView>();
            services.AddTransient<AppointmentsViewModel>();

            services.AddTransient<TrainingContentView>();
            services.AddTransient<TrainingContentViewModel>();

            // Profile
            services.AddTransient<ProfileView>();
            services.AddTransient<ProfileViewModel>();

            services.AddTransient<PersonalInformationEditView>();
            services.AddTransient<PersonalInformationEditViewModel>();

            services.AddTransient<MobilityEditView>();
            services.AddTransient<MobilityEditViewModel>();

            services.AddTransient<LanguageEditView>();
            services.AddTransient<LanguageEditViewModel>();
            services.AddTransient<LanguageView>();
            services.AddTransient<LanguageViewModel>();
            services.AddTransient<LanguageSearchView>();
            services.AddTransient<LanguageSearchViewModel>();

            services.AddTransient<WebReferenceEditView>();
            services.AddTransient<WebReferenceEditViewModel>();
            services.AddTransient<WebReferenceView>();
            services.AddTransient<WebReferenceViewModel>();

            services.AddTransient<QualificationEditView>();
            services.AddTransient<QualificationEditViewModel>();
            services.AddTransient<QualificationView>();
            services.AddTransient<QualificationViewModel>();

            services.AddTransient<ContactInfoEditView>();
            services.AddTransient<ContactInfoEditViewModel>();

            services.AddTransient<ContactView>();
            services.AddTransient<ContactViewModel>();

            services.AddTransient<LicenseEditView>();
            services.AddTransient<LicenseEditViewModel>();
            services.AddTransient<LicenseView>();
            services.AddTransient<LicenseViewModel>();

            services.AddTransient<EducationInfoEditView>();
            services.AddTransient<EducationInfoEditViewModel>();

            services.AddTransient<EducationView>();
            services.AddTransient<EducationViewModel>();

            services.AddTransient<CompanyBasedVocationalTrainingView>();
            services.AddTransient<CompanyBasedVocationalTrainingViewModel>();

            services.AddTransient<StudyView>();
            services.AddTransient<StudyViewModel>();

            services.AddTransient<VocationalTrainingView>();
            services.AddTransient<VocationalTrainingViewModel>();

            services.AddTransient<FurtherEducationView>();
            services.AddTransient<FurtherEducationViewModel>();

            services.AddTransient<CareerInfoEditView>();
            services.AddTransient<CareerInfoEditViewModel>();

            services.AddTransient<OccupationView>();
            services.AddTransient<OccupationViewModel>();

            services.AddTransient<OtherView>();
            services.AddTransient<OtherViewModel>();

            services.AddTransient<ServiceView>();
            services.AddTransient<ServiceViewModel>();

            services.AddTransient<VoluntaryServiceView>();
            services.AddTransient<VoluntaryServiceViewModel>();

            services.AddTransient<BasicView>();
            services.AddTransient<BasicViewModel>();

            services.AddTransient<OccupationSearchView>();
            services.AddTransient<OccupationSearchViewModel>();

            services.AddSingleton<GlobalErrorViewModel>();

            services.AddTransient<SearchView>();
            services.AddTransient<SearchViewModel>();

            services.AddTransient<SearchFilterSheet>();
            services.AddTransient<SearchFilterSheetViewModel>();
        }

        private static void SetupRoutes()
        {
            Routing.RegisterRoute(Routes.ExtendedSplashScreenView, typeof(ExtendedSplashScreenView));
            Routing.RegisterRoute(Routes.Shell, typeof(AppShell));
            Routing.RegisterRoute(Routes.RegistrationView, typeof(RegistrationView));
            Routing.RegisterRoute(Routes.PickUserNameView, typeof(PickUserNameView));
            Routing.RegisterRoute(Routes.UseCaseDescriptionView, typeof(UseCaseDescriptionView));
            Routing.RegisterRoute(Routes.UseCaseSelectionView, typeof(UseCaseSelectionView));
            Routing.RegisterRoute(Routes.StartView, typeof(StartView));
            Routing.RegisterRoute(Routes.AssessmentView, typeof(AssessmentView));
            Routing.RegisterRoute(Routes.AssessmentDescriptionView, typeof(AssessmentDescriptionView));
            Routing.RegisterRoute(Routes.AssessmentResultView, typeof(AssessmentResultView));
            Routing.RegisterRoute(Routes.SettingsView, typeof(SettingsView));

            Routing.RegisterRoute(Routes.TrainingView, typeof(TrainingView));
            Routing.RegisterRoute(Routes.LoansView, typeof(LoansView));
            Routing.RegisterRoute(Routes.AppointmentsView, typeof(AppointmentsView));
            Routing.RegisterRoute(Routes.TrainingContentView, typeof(TrainingContentView));

            Routing.RegisterRoute(Routes.ProfileView, typeof(ProfileView));
            Routing.RegisterRoute(Routes.PersonalInformationEditView, typeof(PersonalInformationEditView));
            Routing.RegisterRoute(Routes.MobilityEditView, typeof(MobilityEditView));

            Routing.RegisterRoute(Routes.LanguageEditView, typeof(LanguageEditView));
            Routing.RegisterRoute(Routes.LanguageView, typeof(LanguageView));
            Routing.RegisterRoute(Routes.LanguageSearchView, typeof(LanguageSearchView));

            Routing.RegisterRoute(Routes.WebReferenceEditView, typeof(WebReferenceEditView));
            Routing.RegisterRoute(Routes.WebReferenceView, typeof(WebReferenceView));

            Routing.RegisterRoute(Routes.QualificationEditView, typeof(QualificationEditView));
            Routing.RegisterRoute(Routes.QualificationView, typeof(QualificationView));

            Routing.RegisterRoute(Routes.ContactInfoEditView, typeof(ContactInfoEditView));
            Routing.RegisterRoute(Routes.ContactInfoContactView, typeof(ContactView));

            Routing.RegisterRoute(Routes.LicenseEditView, typeof(LicenseEditView));
            Routing.RegisterRoute(Routes.LicenseView, typeof(LicenseView));

            Routing.RegisterRoute(Routes.EducationInfoEditView, typeof(EducationInfoEditView));
            Routing.RegisterRoute(Routes.EducationInfoEducationView, typeof(EducationView));
            Routing.RegisterRoute(Routes.CareerInfoEditView, typeof(CareerInfoEditView));
            Routing.RegisterRoute(Routes.EducationInfoCompanyBasedVocationalTrainingView, typeof(CompanyBasedVocationalTrainingView));
            Routing.RegisterRoute(Routes.EducationInfoStudyView, typeof(StudyView));
            Routing.RegisterRoute(Routes.EducationInfoVocationalTrainingView, typeof(VocationalTrainingView));
            Routing.RegisterRoute(Routes.EducationInfoFurtherEducationView, typeof(FurtherEducationView));

            Routing.RegisterRoute(Routes.CareerInfoOccupationView, typeof(OccupationView));
            Routing.RegisterRoute(Routes.CareerInfoOtherView, typeof(OtherView));
            Routing.RegisterRoute(Routes.CareerInfoServiceView, typeof(ServiceView));
            Routing.RegisterRoute(Routes.CareerInfoBasicView, typeof(BasicView));
            Routing.RegisterRoute(Routes.CareerInfoVoluntaryServiceView, typeof(VoluntaryServiceView));
            Routing.RegisterRoute(Routes.OccupationSearchView, typeof(OccupationSearchView));

            Routing.RegisterRoute(Routes.SearchView, typeof(SearchView));
            Routing.RegisterRoute(Routes.SearchFilterSheet, typeof(SearchFilterSheet));

            // TBD
            Routing.RegisterRoute(Routes.EmptyView, typeof(EmptyView));
            Routing.RegisterRoute(Routes.TutorialView, typeof(EmptyView));
            Routing.RegisterRoute(Routes.FavoritesView, typeof(EmptyView));
        }

        private static void SetupHandler()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
#if ANDROID
                // remove underline from entry;
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
                // remove outline from entry;
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping(nameof(DatePicker), (handler, view) =>
            {
#if ANDROID
                // remove underline from datepicker;
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS

                // remove outline from datepicker;
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
            {
#if ANDROID
                // remove underline from datepicker;
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS
                // remove outline from datepicker;
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            Microsoft.Maui.Handlers.ButtonHandler.Mapper.AppendToMapping("Text", (handler, view) =>
            {
#if IOS

#elif ANDROID
#endif
            });

            Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping(nameof(SearchBarHandler), (handler, view) =>
            {
#if ANDROID
#elif IOS
#endif
            });
        }
 
        private static void SetupHandlers(IMauiHandlersCollection handlers)
        {
#if IOS
            handlers.AddHandler(typeof(SearchBar), typeof(De.HDBW.Apollo.Client.Platforms.iOS.CustomSearchbarHandler));
            handlers.AddHandler<Shell, Platforms.CustomShellHandler>();
#elif ANDROID
            handlers.AddHandler<Shell, Platforms.CustomShellHandler>();
#endif
        }
    }
    }
