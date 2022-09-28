namespace De.HDBW.Apollo.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.Input;
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
            this.UseCaseBuilder = builder;
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

        private IUseCaseBuilder UseCaseBuilder { get; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCreateUseCase))]
        public async Task CreateUseCase(CancellationToken token)
        {
            try
            {
                this.IsBusy = true;
                await this.UseCaseBuilder.BuildAsync(this.UseCases.First(u => u.IsSelected).UseCase, token);
            }
            catch (OperationCanceledException)
            {
                this.Logger?.LogDebug($"Canceled CreateUseCase in {this.GetType()}.");
            }
            catch (ObjectDisposedException)
            {
                this.Logger?.LogDebug($"Canceled CreateUseCase in {this.GetType()}.");
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex, $"Unknown Error in CreateUseCase in {this.GetType()}.");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    this.IsBusy = false;
                }
            }
        }

        public bool CanCreateUseCase()
        {
            return !this.IsBusy && this.UseCases.Any(u => u.IsSelected);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            this.CreateUseCaseCommand?.NotifyCanExecuteChanged();
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

            this.RefreshCommands();
        }
    }
}