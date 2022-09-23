namespace De.HDBW.Apollo.Client.ViewModels
{
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using Microsoft.Extensions.Logging;

    public partial class FirstTimeDialogViewModel : BaseViewModel
    {
        public FirstTimeDialogViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            this.OpenTutorialCommand?.NotifyCanExecuteChanged();
            this.ContinueCommand?.NotifyCanExecuteChanged();
        }

        private bool CanContinue()
        {
            return !this.IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanContinue))]
        private void Continue()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, false);
            this.DialogService.ClosePopup(this, result);
        }

        private bool CanOpenTutorial()
        {
            return !this.IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanOpenTutorial))]
        private void OpenTutorial()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, true);
            this.DialogService.ClosePopup(this, result);
        }
    }
}
