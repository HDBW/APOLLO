// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Helper;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

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
           IUseCaseBuilder builder,
           ILogger<UseCaseSelectionViewModel> logger)
           : base(dispatcherService, navigationService, dialogService, logger)
        {
            ArgumentNullException.ThrowIfNull(builder);
            SessionService = sessionService;
            Builder = builder;
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

        private IUseCaseBuilder Builder { get; }

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
            using (var worker = ScheduleWork())
            {
                try
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
                            await Builder.BuildAsync(UseCase.A, CancellationToken.None);
                            if (IsUseCaseSelectionFromShell == true)
                            {
                                await NavigationService.PushToRootAsnc(CancellationToken.None);
                            }
                            else
                            {
                                await NavigationService.PushToRootAsnc(Routes.Shell, CancellationToken.None);
                            }

                            break;
                        default:
                            parameters.AddValue(NavigationParameter.Data, IsUseCaseSelectionFromShell);
                            await NavigationService.NavigateAsnc(Routes.UseCaseDescriptionView, CancellationToken.None, parameters);
                            break;
                    }
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnUseCaseSelectionChanged)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnUseCaseSelectionChanged)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(OnUseCaseSelectionChanged)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }
    }
}
