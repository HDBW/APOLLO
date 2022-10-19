// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    // TODO: Clear UsecaseData when navigating to this model.
    public partial class UseCaseSelectionViewModel : BaseViewModel
    {
        private readonly ObservableCollection<UseCaseEntry> _useCases = new ObservableCollection<UseCaseEntry>();

        public UseCaseSelectionViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           IUseCaseBuilder builder,
           ILogger<UseCaseTutorialViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            UseCaseBuilder = builder;
            UseCases.Add(UseCaseEntry.Import(UseCase.A, OnUseCaseSelectionChanged));
            UseCases.Add(UseCaseEntry.Import(UseCase.B, OnUseCaseSelectionChanged));
            UseCases.Add(UseCaseEntry.Import(UseCase.C, OnUseCaseSelectionChanged));
        }

        public ObservableCollection<UseCaseEntry> UseCases
        {
            get
            {
                return _useCases;
            }
        }

        private IUseCaseBuilder UseCaseBuilder { get; }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanCreateUseCase))]
        public async Task CreateUseCase(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    if (!await UseCaseBuilder.BuildAsync(UseCases.First(u => u.IsSelected).UseCase, worker.Token))
                    {
                        return;
                    }

                    await NavigationService.PushToRootAsnc(Routes.Shell, token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType()}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(CreateUseCase)} in {GetType()}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(CreateUseCase)} in {GetType()}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        public bool CanCreateUseCase()
        {
            return !IsBusy && UseCases.Any(u => u.IsSelected);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            CreateUseCaseCommand?.NotifyCanExecuteChanged();
        }

        private void OnUseCaseSelectionChanged(UseCaseEntry useCase)
        {
            foreach (var item in UseCases)
            {
                if (item == useCase)
                {
                    continue;
                }

                item.UpdateSelectedState(false);
            }

            RefreshCommands();
        }
    }
}
