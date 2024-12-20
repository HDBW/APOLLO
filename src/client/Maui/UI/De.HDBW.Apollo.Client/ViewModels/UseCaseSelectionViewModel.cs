﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels
{
    public partial class UseCaseSelectionViewModel : BaseViewModel
    {
        private readonly ObservableCollection<UseCaseEntry> _useCases = new ObservableCollection<UseCaseEntry>();

        public UseCaseSelectionViewModel(
           IDispatcherService dispatcherService,
           INavigationService navigationService,
           IDialogService dialogService,
           ISessionService sessionService,
           ILogger<UseCaseDescriptionViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            SessionService = sessionService;

            // UseCases.Add(UseCaseEntry.Import(UseCase.A, OnUseCaseSelectionChanged));
            // UseCases.Add(UseCaseEntry.Import(UseCase.B, OnUseCaseSelectionChanged));
            // UseCases.Add(UseCaseEntry.Import(UseCase.C, OnUseCaseSelectionChanged));
            UseCases.Add(UseCaseEntry.Import(UseCase.D, OnUseCaseSelectionChanged));
        }

        public ObservableCollection<UseCaseEntry> UseCases
        {
            get
            {
                return _useCases;
            }
        }

        private ISessionService SessionService { get; }

        private bool? IsUseCaseSelectionFromShell { get; set; }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            IsUseCaseSelectionFromShell = navigationParameters?.GetValue<bool?>(NavigationParameter.Data);
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
            SessionService.UpdateUseCase(selectedUseCase?.UseCase);
            if (selectedUseCase == null)
            {
                return;
            }

            selectedUseCase.UpdateSelectedState(false);

            var parameters = new NavigationParameters();
            switch (selectedUseCase.UseCase)
            {
                case UseCase.D:
                    if (IsUseCaseSelectionFromShell == true)
                    {
                        await NavigationService.PushToRootAsync(CancellationToken.None);
                    }
                    else
                    {
                        await NavigationService.PushToRootAsync(Routes.Shell, CancellationToken.None);
                    }

                    break;
                default:
                    parameters.AddValue(NavigationParameter.Data, IsUseCaseSelectionFromShell);
                    await NavigationService.NavigateAsync(Routes.UseCaseDescriptionView, CancellationToken.None, parameters);
                    break;
            }
        }
    }
}
