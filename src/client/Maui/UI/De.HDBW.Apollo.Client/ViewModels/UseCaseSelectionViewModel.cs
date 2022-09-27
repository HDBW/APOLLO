namespace De.HDBW.Apollo.Client.ViewModels
{
    using De.HDBW.Apollo.Client.Contracts;
    using Microsoft.Extensions.Logging;

    public partial class UseCaseSelectionViewModel : BaseViewModel
    {
        public UseCaseSelectionViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           ILogger<UseCaseTutorialViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
