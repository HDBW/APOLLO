// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(
            IPreferenceService preferenceService,
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<RegistrationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(preferenceService);
            PreferenceService = preferenceService;
        }

        private IPreferenceService PreferenceService { get; }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            RegisterCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanRegister))]
        private async Task Register(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(RegisterCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var confirmedDataUsage = PreferenceService.GetValue<bool>(SharedContracts.Enums.Preference.ConfirmedDataUsage, false);
                    await NavigationService.RestartAsync(confirmedDataUsage, CancellationToken.None);
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
