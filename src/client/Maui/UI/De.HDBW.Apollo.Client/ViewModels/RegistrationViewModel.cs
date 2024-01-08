// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Dialogs;
using De.HDBW.Apollo.Client.Models;
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
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(authService);
            ArgumentNullException.ThrowIfNull(sessionService);
            AuthService = authService;
            SessionService = sessionService;
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
                    await NavigationService.PushToRootAsync(Routes.UseCaseSelectionView, worker.Token);
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
                    await AuthService.SignInInteractively(worker.Token);
                    //authentication = await AuthService.AcquireTokenSilent(worker.Token);
                    //var userId = authentication.Account?.HomeAccountId;
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
                    SessionService.UpdateRegisteredUser(authentication?.Account != null);
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
                        authentication = await AuthService.SignInInteractively(worker.Token);
                        await NavigationService.PushToRootAsync(Routes.UseCaseSelectionView, worker.Token);
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
                    SessionService.UpdateRegisteredUser(authentication?.Account != null);
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
}
