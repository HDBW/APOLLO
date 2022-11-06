﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
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
                    await NavigationService.PushToRootAsnc(Routes.UseCaseSelectionView, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Skip)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Skip)} in {GetType()}.");
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
                try
                {
                    var authentication = await AuthService.AcquireTokenSilent(worker.Token);
                    SessionService.UpdateRegisteredUser(authentication?.Account != null);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(UnRegister)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(UnRegister)} in {GetType()}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while unregistering user in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(UnRegister)} in {GetType()}.");
                }
                finally
                {
                    OnPropertyChanged(nameof(HasRegisterdUser));
                    RefreshCommands();
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
                try
                {
                    var authentication = await AuthService.SignInInteractively(worker.Token);
                    SessionService.UpdateRegisteredUser(authentication?.Account != null);

                    await NavigationService.PushToRootAsnc(Routes.UseCaseSelectionView, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Register)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(Register)} in {GetType()}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while registering user in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(Register)} in {GetType()}.");
                }
                finally
                {
                    OnPropertyChanged(nameof(HasRegisterdUser));
                    RefreshCommands();
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
