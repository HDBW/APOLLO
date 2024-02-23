// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.Taxonomy;
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

        private WorkingTimeModel? _workTime;

        private Occupation? _job;

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
            timeModels.Add(InteractionEntry.Import(WorkingTimeModel.FULLTIME.GetLocalizedString(), WorkingTimeModel.FULLTIME, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(WorkingTimeModel.PARTTIME.GetLocalizedString(), WorkingTimeModel.PARTTIME, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(WorkingTimeModel.SHIFT_NIGHT_WORK_WEEKEND.GetLocalizedString(), WorkingTimeModel.SHIFT_NIGHT_WORK_WEEKEND, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(WorkingTimeModel.MINIJOB.GetLocalizedString(), WorkingTimeModel.MINIJOB, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            timeModels.Add(InteractionEntry.Import(WorkingTimeModel.HOME_TELEWORK.GetLocalizedString(), WorkingTimeModel.HOME_TELEWORK, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;
            var occupation = currentData?.Job;
            var selectedTimeModel = _workTime != null ? timeModels.FirstOrDefault(x => ((WorkingTimeModel?)x.Data) == _workTime) : (timeModels.FirstOrDefault(x => (x.Data as WorkingTimeModel?) == currentData?.WorkingTimeModel.AsEnum<WorkingTimeModel>()) ?? timeModels.FirstOrDefault());

            if (EditState != null)
            {
                occupation = EditState?.Job;
                selectedTimeModel = timeModels.FirstOrDefault(x => (x.Data as WorkingTimeModel?) == EditState?.WorkingTimeModel.AsEnum<WorkingTimeModel>());
            }

            var selection = SelectionResult.Deserialize<Occupation>();
            if (!string.IsNullOrWhiteSpace(SelectionResult))
            {
                occupation = SelectionResult.Deserialize<Occupation>();
                isDirty = true;
            }

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(occupation, timeModels.AsSortedList(), selectedTimeModel, isDirty), token).ConfigureAwait(false);
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

        protected override void ApplyChanges(CareerInfo entry)
        {
            base.ApplyChanges(entry);
            if (ShowWorkTimeModelsSelection)
            {
                entry.WorkingTimeModel = SelectedWorkTimeModel?.Data != null ? ((WorkingTimeModel)SelectedWorkTimeModel.Data).ToApolloListItem() : null;
            }
            else
            {
                entry.WorkingTimeModel = _workTime != null ? _workTime?.ToApolloListItem() : null;
            }

            entry.Job = _job;
        }

        partial void OnSelectedWorkTimeModelChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        private void LoadonUIThread(Occupation? occupation, List<InteractionEntry> timeModels, InteractionEntry? selectedWorkTimeModel, bool isDirty)
        {
            _job = occupation;
            OccupationName = occupation?.PreferedTerm?.FirstOrDefault();
            WorkTimeModels = new ObservableCollection<InteractionEntry>(timeModels);
            SelectedWorkTimeModel = selectedWorkTimeModel;
            OnPropertyChanged(nameof(ShowWorkTimeModelsSelection));
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchOccupation))]
        private async Task SearchOccupation(CancellationToken token)
        {
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameter = new NavigationParameters();
                    parameter.AddValue(NavigationParameter.SavedState, GetCurrentState());
                    await NavigationService.NavigateAsync(Routes.OccupationSearchView, token, parameter);
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
