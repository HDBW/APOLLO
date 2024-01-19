// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        public RegistrationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            IAuthService authService,
            ISessionService sessionService,
            IPreferenceService preferenceService,
            IUserRepository userRepository,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(authService);
            ArgumentNullException.ThrowIfNull(sessionService);
            ArgumentNullException.ThrowIfNull(preferenceService);
            ArgumentNullException.ThrowIfNull(userRepository);
            AuthService = authService;
            SessionService = sessionService;
            PreferenceService = preferenceService;
            UserRepository = userRepository;
        }

        public bool HasRegisterdUser
        {
            get
            {
                return SessionService.HasRegisteredUser;
            }
        }

        private IAuthService AuthService { get; }

        private ISessionService SessionService { get; }

        private IPreferenceService PreferenceService { get; }

        private IUserRepository UserRepository { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
            RegisterCommand?.NotifyCanExecuteChanged();
            UnRegisterCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    SessionService.UpdateRegisteredUser(null);
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
            return !IsBusy;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanUnRegister))]
        private async Task UnRegister(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                AuthenticationResult? authentication = null;
                try
                {
#if !DEBUG
                    await AuthService.SignInInteractively(worker.Token);
#endif
                    await UserRepository.DeleteUserAsync(CancellationToken.None).ConfigureAwait(false);
                    PreferenceService.SetValue<string?>(Preference.RegisteredUserId, null);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(UnRegister)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(UnRegister)} in {GetType().Name}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while unregistering user in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(UnRegister)} in {GetType().Name}.");
                }
                finally
                {
                    SessionService.UpdateRegisteredUser(authentication?.Account?.HomeAccountId);
                    OnPropertyChanged(nameof(HasRegisterdUser));
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanUnRegister()
        {
            return !IsBusy && HasRegisterdUser;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                AuthenticationResult? authentication = null;
                try
                {
                    var result = await DialogService.ShowPopupAsync<ConfirmDataUsageDialog, NavigationParameters>(worker.Token);
                    if (result?.GetValue<bool?>(NavigationParameter.Result) ?? false)
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
                    SessionService.UpdateRegisteredUser(authentication?.Account.HomeAccountId);
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
            return !IsBusy && !HasRegisterdUser;
        }
    }

#if DEBUG
#pragma warning disable SA1402 // File may only contain a single type
    public class DummyAccount : IAccount
    {
        public string Username { get; } = "Dummy";

        public string Environment { get; } = "Dummy";

        public AccountId HomeAccountId { get; } = new AccountId("Dummy", "Dummy", "Dummy");
    }
#pragma warning restore SA1402 // File may only contain a single type
#endif
}
