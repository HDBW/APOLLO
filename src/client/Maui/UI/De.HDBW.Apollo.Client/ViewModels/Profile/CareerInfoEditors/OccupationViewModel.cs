﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class OccupationViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        private string _occupationName;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _workTimeModels = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedWorkTimeModel;

        private CareerInfo? _careers;
        private WorkingTimeModel? _workTime;

        public OccupationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<OccupationViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public bool ShowWorkTimeModelsSelection
        {
            get { return _workTime != WorkingTimeModel.PARTTIME; }
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var timeModels = new List<InteractionEntry>();
                    timeModels.Add(InteractionEntry.Import("Vollzeit", WorkingTimeModel.FULLTIME, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    timeModels.Add(InteractionEntry.Import("Teilzeit", WorkingTimeModel.PARTTIME, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    timeModels.Add(InteractionEntry.Import("Schicht-/Nacht-/Wochenendarbeit", WorkingTimeModel.SHIFT_NIGHT_WORK_WEEKEND, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    timeModels.Add(InteractionEntry.Import("Minijob", WorkingTimeModel.MINIJOB, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    timeModels.Add(InteractionEntry.Import("Minijob", WorkingTimeModel.HOME_TELEWORK, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(timeModels), worker.Token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error while {nameof(OnNavigatedToAsync)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private void LoadonUIThread(List<InteractionEntry> timeModels)
        {
            WorkTimeModels = new ObservableCollection<InteractionEntry>(timeModels);
            SelectedWorkTimeModel = _workTime != null ? WorkTimeModels.FirstOrDefault(x => ((WorkingTimeModel?)x.Data) == _workTime) : WorkTimeModels.FirstOrDefault();
        }

        protected override void OnPrepare(NavigationParameters navigationParameters)
        {
            base.OnPrepare(navigationParameters);
            _workTime = navigationParameters?.GetValue<WorkingTimeModel?>(NavigationParameter.Data);
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchOccupationCommand?.NotifyCanExecuteChanged();
        }


        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchOccupation))]
        private async Task SearchOccupation(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(OccupationName, token);
                }
                catch (OperationCanceledException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchOccupation)} in {GetType().Name}.");
                }
                catch (ObjectDisposedException)
                {
                    Logger?.LogDebug($"Canceled {nameof(SearchOccupation)} in {GetType().Name}.");
                }
                catch (Exception ex)
                {
                    Logger?.LogError(ex, $"Unknown error in {nameof(SearchOccupation)} in {GetType().Name}.");
                }
                finally
                {
                    UnscheduleWork(worker);
                }
            }
        }

        private bool CanSearchOccupation()
        {
            return !IsBusy;
        }
    }
}