namespace De.HDBW.Apollo.Client.ViewModels
{
    using CommunityToolkit.Mvvm.Input;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using Microsoft.Extensions.Logging;

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
            this.SkipCommand?.NotifyCanExecuteChanged();
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSkip))]
        private async Task Skip(CancellationToken token)
        {
            this.IsBusy = true;
            try
            {
                await this.NavigationService.PushToRootAsnc(Routes.UseCaseSelectionView, token);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled Skip in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled Skip in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in Skip in {this.GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    this.IsBusy = false;
                }
            }
        }

        private bool CanSkip()
        {
            return !this.IsBusy;
        }
    }
}
