namespace De.HDBW.Apollo.Client.ViewModels
{
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using Microsoft.Extensions.Logging;

    public partial class EmptyViewModel : BaseViewModel
    {
        public EmptyViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StartViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
        }
    }
}
