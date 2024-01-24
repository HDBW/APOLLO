// (c) Licensed to the HDBW under one or more agreements.
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

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class StudyViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        private string? _nameOfInstitution;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _completionStates = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedCompletionState;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _univerityDegrees = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedUniverityDegree;

        [ObservableProperty]
        private string _occupationName;

        private EducationInfo? _education;

        public StudyViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StudyViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public bool HasEnd
        {
            get
            {
                return End.HasValue;
            }
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var completionStates = new List<InteractionEntry>();
                    completionStates.Add(InteractionEntry.Import(Resources.Strings.Resources.CompletionState_Completed, CompletionState.Completed, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    completionStates.Add(InteractionEntry.Import(Resources.Strings.Resources.CompletionState_Failed, CompletionState.Failed, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    completionStates.Add(InteractionEntry.Import(Resources.Strings.Resources.CompletionState_Ongoning, CompletionState.Ongoning, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

                    var univerityDegrees = new List<InteractionEntry>();
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_Master, UniversityDegree.Master, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_Bachelor, UniversityDegree.Bachelor, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_Pending, UniversityDegree.Pending, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_Doctorate, UniversityDegree.Doctorate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_StateExam, UniversityDegree.StateExam, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_UnregulatedUnrecognized, UniversityDegree.UnregulatedUnrecognized, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_RegulatedUnrecognized, UniversityDegree.RegulatedUnrecognized, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_PartialRecognized, UniversityDegree.PartialRecognized, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_EcclesiasticalExam, UniversityDegree.EcclesiasticalExam, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    univerityDegrees.Add(InteractionEntry.Import(Resources.Strings.Resources.UniversityDegree_Other, UniversityDegree.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(completionStates, univerityDegrees), worker.Token);
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

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchOccupationCommand?.NotifyCanExecuteChanged();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        partial void OnEndChanged(DateTime? value)
        {
            OnPropertyChanged(nameof(HasEnd));
            RefreshCommands();
        }

        [RelayCommand(CanExecute = nameof(CanClearEnd))]
        private void ClearEnd()
        {
            End = null;
        }

        private bool CanClearEnd()
        {
            return !IsBusy && HasEnd;
        }

        private void LoadonUIThread(List<InteractionEntry> completionStates, List<InteractionEntry> univerityDegrees)
        {
            CompletionStates = new ObservableCollection<InteractionEntry>(completionStates);
            SelectedCompletionState = CompletionStates.FirstOrDefault();
            UniverityDegrees = new ObservableCollection<InteractionEntry>(univerityDegrees);
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
    }
}
