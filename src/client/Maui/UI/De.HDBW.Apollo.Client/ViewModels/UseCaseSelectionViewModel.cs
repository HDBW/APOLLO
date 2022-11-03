// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
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
           ILogger<UseCaseDescriptionViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
        }

        private async void OnUseCaseSelectionChanged(UseCaseEntry useCase)
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

            var selectedUseCase = UseCases.FirstOrDefault(u => u.IsSelected);
            if (selectedUseCase == null)
            {
                return;
            }

            selectedUseCase.UpdateSelectedState(false);
            var parameters = new NavigationParameters();
            parameters.AddValue(NavigationParameter.Id, selectedUseCase.UseCase);
            await NavigationService.NavigateAsnc(Routes.UseCaseDescriptionView, CancellationToken.None, parameters);
        }
    }
}
