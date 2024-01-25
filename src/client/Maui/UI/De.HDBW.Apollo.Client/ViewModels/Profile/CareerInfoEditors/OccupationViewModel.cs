// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class OccupationViewModel : OtherViewModel
    {
        [ObservableProperty]
        private string? _occupationName;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _workTimeModels = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedWorkTimeModel;

        private CareerInfo? _career;

        private WorkingTimeModel? _workTime;

        public OccupationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<OccupationViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        public bool ShowWorkTimeModelsSelection
        {
            get { return _workTime != WorkingTimeModel.MINIJOB; }
        }

        protected override async Task<CareerInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var timeModels = new List<InteractionEntry>();
            timeModels.Add(InteractionEntry.Import(Resources.Strings.Resources.WorkingTimeModel_FullTime, WorkingTimeModel.FULLTIME, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(Resources.Strings.Resources.WorkingTimeModel_PartTime, WorkingTimeModel.PARTTIME, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(Resources.Strings.Resources.WorkingTimeModel_ShiftNightWorkWeekend, WorkingTimeModel.SHIFT_NIGHT_WORK_WEEKEND, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(Resources.Strings.Resources.WorkingTimeModel_Minijob, WorkingTimeModel.MINIJOB, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(Resources.Strings.Resources.WorkingTimeModel_HomeTeleWork, WorkingTimeModel.HOME_TELEWORK, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData, timeModels), token).ConfigureAwait(false);
            return currentData;
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
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(CareerInfo? careerInfo, List<InteractionEntry> timeModels)
        {
            WorkTimeModels = new ObservableCollection<InteractionEntry>(timeModels);
            SelectedWorkTimeModel = _workTime != null ? WorkTimeModels.FirstOrDefault(x => ((WorkingTimeModel?)x.Data) == _workTime) : (WorkTimeModels.FirstOrDefault(x => (x.Data as WorkingTimeModel?) == careerInfo?.WorkingTimeModel) ?? WorkTimeModels.FirstOrDefault());
            OnPropertyChanged(nameof(ShowWorkTimeModelsSelection));
            IsDirty = false;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchOccupation))]
        private async Task SearchOccupation(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    await NavigationService.NavigateAsync(Routes.OccupationSearchView, token);
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

        partial void OnSelectedWorkTimeModelChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }
    }
}
