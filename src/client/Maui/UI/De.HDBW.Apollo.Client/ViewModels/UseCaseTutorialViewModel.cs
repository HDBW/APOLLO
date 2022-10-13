// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class UseCaseTutorialViewModel : BaseViewModel
    {
        public UseCaseTutorialViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           ILogger<UseCaseTutorialViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            IsBusy = true;
            try
            {
                await NavigationService.PushToRootAsnc(Routes.UseCaseSelectionView, token);
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
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
        }

        private bool CanSkip()
        {
            return !IsBusy;
        }
    }
}
