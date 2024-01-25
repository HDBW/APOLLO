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

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class CompanyBasedVocationalTrainingViewModel : BasicEducationInfoViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _schoolGraduations = new ObservableCollection<InteractionEntry>();

        private InteractionEntry? _selectedSchoolGraduation;

        [ObservableProperty]
        private string? _occupationName;

        public CompanyBasedVocationalTrainingViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<CompanyBasedVocationalTrainingViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        public InteractionEntry? SelectedSchoolGraduation
        {
            get
            {
                return _selectedSchoolGraduation;
            }

            set
            {
                SetProperty(ref _selectedSchoolGraduation, value);
            }
        }

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var schoolGraduations = new List<InteractionEntry>();
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_SecondarySchoolCertificate, SchoolGraduation.SecondarySchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_AdvancedTechnicalCollegeCertificate, SchoolGraduation.AdvancedTechnicalCollegeCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_HigherEducationEntranceQualificationALevel, SchoolGraduation.HigherEducationEntranceQualificationALevel, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_IntermediateSchoolCertificate, SchoolGraduation.IntermediateSchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_ExtendedSecondarySchoolLeavingCertificate, SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_NoSchoolLeavingCertificate, SchoolGraduation.NoSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_SpecialSchoolLeavingCertificate, SchoolGraduation.SpecialSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_SubjectRelatedEntranceQualification, SchoolGraduation.SubjectRelatedEntranceQualification, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_AdvancedTechnicalCollegeWithoutCertificate, SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            await ExecuteOnUIThreadAsync(
                () => LoadonUIThread(schoolGraduations), token);
            return currentData;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            SearchOccupationCommand?.NotifyCanExecuteChanged();
        }

        protected override void ApplyChanges(EducationInfo entry)
        {
            base.ApplyChanges(entry);
            entry.Graduation = (SelectedSchoolGraduation?.Data as SchoolGraduation?) ?? SchoolGraduation.Unknown;
        }

        private void LoadonUIThread(List<InteractionEntry> schoolGraduations)
        {
            SchoolGraduations = new ObservableCollection<InteractionEntry>(schoolGraduations);
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

        partial void OnSchoolGraduationsChanged(ObservableCollection<InteractionEntry> value)
        {
            IsDirty = true;
        }
    }
}
