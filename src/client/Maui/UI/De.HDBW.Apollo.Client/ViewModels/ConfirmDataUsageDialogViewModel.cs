// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class ConfirmDataUsageDialogViewModel : BaseViewModel
    {
        public ConfirmDataUsageDialogViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ConfirmDataUsageDialogViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CancelCommand?.NotifyCanExecuteChanged();
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
            result.AddValue(NavigationParameter.Result, true);
            DialogService.ClosePopup(this, result);
        }

        private bool CanCancel()
        {
            return !IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanCancel))]
        private void Cancel()
        {
            var result = new NavigationParameters();
            result.AddValue(NavigationParameter.Result, false);
            DialogService.ClosePopup(this, result);
        }
    }
}
