// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
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
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            AuthService = authService;
        }

        private IAuthService AuthService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.PushToRootAsnc(Routes.UseCaseTutorialView, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled Skip in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled Skip in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown Error in Skip in {GetType()}.");
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

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var x = await AuthService.AcquireTokenSilent(worker.Token);
                    if (x != null)
                    {
                        await AuthService.LogoutAsync(worker.Token);
                    }

                    var result = await AuthService.SignInInteractively(worker.Token);
                    await NavigationService.PushToRootAsnc(Routes.UseCaseTutorialView, worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled Register in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled Register in {GetType()}.");
                }
                catch (MsalException ex)
                {
                    Logger?.LogWarning(ex, $"Error while registering user in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown Error in Register in {GetType()}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanRegister()
        {
            return !IsBusy;
        }
    }
}
