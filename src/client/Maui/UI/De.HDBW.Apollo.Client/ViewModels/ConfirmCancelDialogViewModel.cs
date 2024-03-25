// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class ConfirmCancelDialogViewModel : BaseViewModel, IModalQueryAttributable
    {
        [ObservableProperty]
        private string? _message;

        [ObservableProperty]
        private string? _title;

        public ConfirmCancelDialogViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ConfirmCancelDialogViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public virtual void ApplyModalQueryAttributes(IDictionary<string, object> query)
        {
            OnPrepareModal(NavigationParameters.FromQueryDictionary(query));
        }

        protected virtual void OnPrepareModal(NavigationParameters navigationParameters)
        {
            Message = navigationParameters.GetValue<string>(NavigationParameter.Data);
            Title = navigationParameters.GetValue<string>(NavigationParameter.Title) ?? Resources.Strings.Resources.ErrorDialog_Title;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CancelCommand?.NotifyCanExecuteChanged();
            ConfirmCommand?.NotifyCanExecuteChanged();
        }

        private bool CanConfirm()
        {
            return !IsBusy;
        }

        [CommunityToolkit.Mvvm.Input.RelayCommand(CanExecute = nameof(CanConfirm))]
        private void Confirm()
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
