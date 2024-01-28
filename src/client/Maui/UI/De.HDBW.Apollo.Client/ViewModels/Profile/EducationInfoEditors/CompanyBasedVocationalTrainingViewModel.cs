// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class CompanyBasedVocationalTrainingViewModel : BasicEducationInfoViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _schoolGraduations = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private InteractionEntry? _selectedSchoolGraduation;

        [ObservableProperty]
        private string? _occupationName;

        private Occupation? _job;

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

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var schoolGraduations = new List<InteractionEntry>();
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SecondarySchoolCertificate.GetLocalizedString(), SchoolGraduation.SecondarySchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.AdvancedTechnicalCollegeCertificate.GetLocalizedString(), SchoolGraduation.AdvancedTechnicalCollegeCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.HigherEducationEntranceQualificationALevel.GetLocalizedString(), SchoolGraduation.HigherEducationEntranceQualificationALevel, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.IntermediateSchoolCertificate.GetLocalizedString(), SchoolGraduation.IntermediateSchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.NoSchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.NoSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SpecialSchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.SpecialSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SubjectRelatedEntranceQualification.GetLocalizedString(), SchoolGraduation.SubjectRelatedEntranceQualification, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            //schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate.GetLocalizedString(), SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;
            var selectedSchoolGraduations = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == currentData?.Graduation);
            var occupation = currentData?.ProfessionalTitle;
            var selection = SelectionResult.Deserialize<Occupation>();
            if (EditState != null)
            {
                selectedSchoolGraduations = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == EditState?.Graduation);
                occupation = EditState?.ProfessionalTitle;
            }

            if (!string.IsNullOrWhiteSpace(SelectionResult))
            {
                occupation = SelectionResult.Deserialize<Occupation>();
                isDirty = true;
            }

            await ExecuteOnUIThreadAsync(
                () => LoadonUIThread(occupation, schoolGraduations, selectedSchoolGraduations, isDirty), token);
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

        partial void OnSelectedSchoolGraduationChanged(InteractionEntry? value)
        {
            ValidateProperty(value, nameof(SelectedSchoolGraduation));
            IsDirty = true;
        }

        private void LoadonUIThread(Occupation? occupation, List<InteractionEntry> schoolGraduations, InteractionEntry? selectedSchoolGraduations, bool isDirty)
        {
            _job = occupation;
            OccupationName = occupation?.PreferedTerm?.FirstOrDefault();
            SchoolGraduations = new ObservableCollection<InteractionEntry>(schoolGraduations);
            SelectedSchoolGraduation = selectedSchoolGraduations;
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
