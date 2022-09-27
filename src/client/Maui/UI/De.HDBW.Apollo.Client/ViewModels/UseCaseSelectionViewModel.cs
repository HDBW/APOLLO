namespace De.HDBW.Apollo.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using De.HDBW.Apollo.Client.Contracts;
    using De.HDBW.Apollo.Client.Models;
    using De.HDBW.Apollo.SharedContracts.Enums;
    using De.HDBW.Apollo.SharedContracts.Helper;
    using Microsoft.Extensions.Logging;

    public partial class UseCaseSelectionViewModel : BaseViewModel
    {
        private readonly ObservableCollection<UseCaseEntry> useCases = new ObservableCollection<UseCaseEntry>();

        public UseCaseSelectionViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           IUseCaseBuilder builder,
           ILogger<UseCaseTutorialViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            this.UseCases.Add(UseCaseEntry.Import(UseCase.A, this.OnUseCaseSelectionChanged));
            this.UseCases.Add(UseCaseEntry.Import(UseCase.B, this.OnUseCaseSelectionChanged));
            this.UseCases.Add(UseCaseEntry.Import(UseCase.C, this.OnUseCaseSelectionChanged));
        }

        public ObservableCollection<UseCaseEntry> UseCases
        {
            get
            {
                return this.useCases;
            }
        }

        public Task CreateUseCase(CancellationToken token)
        {
            return Task.CompletedTask;
        }

        private void OnUseCaseSelectionChanged(UseCaseEntry useCase)
        {
            foreach (var item in this.UseCases)
            {
                if (item == useCase)
                {
                    continue;
                }

                item.UpdateSelectedState(false);
            }
        }
    }
}