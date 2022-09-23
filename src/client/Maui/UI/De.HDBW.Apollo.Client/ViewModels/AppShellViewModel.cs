namespace De.HDBW.Apollo.Client.ViewModels
{
    using De.HDBW.Apollo.Client.Contracts;
    using Microsoft.Extensions.Logging;

    public class AppShellViewModel : BaseViewModel
    {
        public AppShellViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}