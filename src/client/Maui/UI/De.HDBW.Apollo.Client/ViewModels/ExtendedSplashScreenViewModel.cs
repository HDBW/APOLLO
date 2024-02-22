// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class ExtendedSplashScreenViewModel : BaseViewModel
    {
        private bool _confirmedDataUsage;

        private bool _confirmedDSGVO;

        public ExtendedSplashScreenViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ExtendedSplashScreenViewModel> logger,
            IUserService userService,
            IPreferenceService preferenceService,
            ISessionService sessionService,
            IAuthService authService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(userService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(authService);
            UserService = userService;
            PreferenceService = preferenceService;
            SessionService = sessionService;
            AuthService = authService;
        }

        public bool ConfirmedDataUsage
        {
            get
            {
                return _confirmedDataUsage;
            }

            set
            {
                if (SetProperty(ref _confirmedDataUsage, value))
                {
                    PreferenceService.SetValue(Preference.ConfirmedDataUsage, value);
                    RefreshCommands();
                }
            }
        }

        public bool ConfirmedDSGVO
        {
            get
            {
                return _confirmedDSGVO;
            }

            set
            {
                if (SetProperty(ref _confirmedDSGVO, value))
                {
                    if (!ConfirmedDSGVO)
                    {
                        ConfirmedDataUsage = false;
                    }

                    RefreshCommands();
                }
            }
        }

        public bool HasRegisterdUser
        {
            get
            {
                return SessionService.HasRegisteredUser;
            }
        }

        private IUserService UserService { get; }

        private IPreferenceService PreferenceService { get; }

        private ISessionService SessionService { get; }

        private IAuthService AuthService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
            RegisterCommand?.NotifyCanExecuteChanged();
            OpenPrivacyCommand?.NotifyCanExecuteChanged();
            OpenDataUsageDialogCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenPrivacy))]
        private Task OpenPrivacy(CancellationToken token)
        {
            return OpenUrlAsync(Resources.Strings.Resources.SettingsView_PrivacyUri, token);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanOpenPrivacy))]
        private async Task OpenDataUsageDialog(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await DialogService.ShowPopupAsync<ConfirmDataUsageDialog, NavigationParameters>(worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenDataUsageDialogCommand)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OpenDataUsageDialogCommand)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OpenDataUsageDialogCommand)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanOpenPrivacy()
        {
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.PushToRootAsync(Routes.Shell, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Skip)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSkip()
        {
            return !IsBusy && ConfirmedDSGVO && ConfirmedDataUsage;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                AuthenticationResult? authentication = null;
                try
                {
#if !DEBUG
                        authentication = await AuthService.SignInInteractively(worker.Token);
#else
                    authentication = new AuthenticationResult(
                          accessToken: "Mock",
                          isExtendedLifeTimeToken: true,
                          uniqueId: "Mock",
                          expiresOn: DateTimeOffset.MaxValue,
                          extendedExpiresOn: DateTimeOffset.MaxValue,
                          tenantId: "Mock",
                          account: new DummyAccount(),
                          idToken: "Mock",
                          scopes: new List<string>(),
                          correlationId: Guid.Empty,
                          tokenType: "Bearer",
                          authenticationResultMetadata: null,
                          claimsPrincipal: null,
                          spaAuthCode: null,
                          additionalResponseParameters: null);
#endif
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Register)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Register)} in {GetType().Name}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while registering user in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Register)} in {GetType().Name}.");
                }
                finally
                {
                    UserService?.UpdateAuthorizationHeader(authentication?.CreateAuthorizationHeader());
                    SessionService.UpdateRegisteredUser(authentication?.AccessToken, authentication?.Account.HomeAccountId);
                    if (SessionService.HasRegisteredUser)
                    {
                        await NavigationService.PushToRootAsync(Routes.PickUserNameView, worker.Token);
                    }

                    OnPropertyChanged(nameof(HasRegisterdUser));
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanRegister()
        {
            return !IsBusy && !HasRegisterdUser && ConfirmedDSGVO && ConfirmedDataUsage;
        }
    }
}
