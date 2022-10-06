using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
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
            OpenTutorialCommand?.NotifyCanExecuteChanged();
            ContinueCommand?.NotifyCanExecuteChanged();
        }

        private bool CanContinue()
        {
            return !IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanContinue))]
        private void Continue()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, false);
            DialogService.ClosePopup(this, result);
        }

        private bool CanOpenTutorial()
        {
            return !IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanOpenTutorial))]
        private void OpenTutorial()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, true);
            DialogService.ClosePopup(this, result);
        }
    }
}
