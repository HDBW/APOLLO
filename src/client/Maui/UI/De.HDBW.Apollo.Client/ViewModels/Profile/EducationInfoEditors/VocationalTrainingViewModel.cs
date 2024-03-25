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

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class VocationalTrainingViewModel : BasicEducationInfoViewModel
    {
        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _typeOfSchools = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedTypeOfSchool;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _univerityDegrees = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedUniverityDegree;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _schoolGraduations = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedSchoolGraduation;

        [ObservableProperty]
        private string? _occupationName;

        private Occupation? _professionalTitle;

        public VocationalTrainingViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<VocationalTrainingViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var typeOfSchools = new List<InteractionEntry>();
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.Other.GetLocalizedString(), TypeOfSchool.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalCollege.GetLocalizedString(), TypeOfSchool.VocationalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.MainSchool.GetLocalizedString(), TypeOfSchool.MainSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalHighSchool.GetLocalizedString(), TypeOfSchool.VocationalHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.SchoolWithMultipleCourses.GetLocalizedString(), TypeOfSchool.SchoolWithMultipleCourses, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.TechnicalCollege.GetLocalizedString(), TypeOfSchool.TechnicalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalTrainingSchool.GetLocalizedString(), TypeOfSchool.VocationalTrainingSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.TechnicalAcademy.GetLocalizedString(), TypeOfSchool.TechnicalAcademy, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.UniversityOfAppliedScience.GetLocalizedString(), TypeOfSchool.UniversityOfAppliedScience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var univerityDegrees = new List<InteractionEntry>();
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.Master.GetLocalizedString(), UniversityDegree.Master, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.Bachelor.GetLocalizedString(), UniversityDegree.Bachelor, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.Pending.GetLocalizedString(), UniversityDegree.Pending, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.Doctorate.GetLocalizedString(), UniversityDegree.Doctorate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.StateExam.GetLocalizedString(), UniversityDegree.StateExam, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.UnregulatedUnrecognized.GetLocalizedString(), UniversityDegree.UnregulatedUnrecognized, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.RegulatedUnrecognized.GetLocalizedString(), UniversityDegree.RegulatedUnrecognized, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.PartialRecognized.GetLocalizedString(), UniversityDegree.PartialRecognized, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.EcclesiasticalExam.GetLocalizedString(), UniversityDegree.EcclesiasticalExam, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            univerityDegrees.Add(InteractionEntry.Import(UniversityDegree.Other.GetLocalizedString(), UniversityDegree.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var schoolGraduations = new List<InteractionEntry>();

            // schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SecondarySchoolCertificate.GetLocalizedString(), SchoolGraduation.SecondarySchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.AdvancedTechnicalCollegeCertificate.GetLocalizedString(), SchoolGraduation.AdvancedTechnicalCollegeCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            // schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.HigherEducationEntranceQualificationALevel.GetLocalizedString(), SchoolGraduation.HigherEducationEntranceQualificationALevel, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            // schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.IntermediateSchoolCertificate.GetLocalizedString(), SchoolGraduation.IntermediateSchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            // schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            // schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.NoSchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.NoSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            // schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SpecialSchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.SpecialSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SubjectRelatedEntranceQualification.GetLocalizedString(), SchoolGraduation.SubjectRelatedEntranceQualification, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            // schoolGraduations.Add(InteractionEntry.ImportSchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate.GetLocalizedString(), SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;

            var description = currentData?.Description;
            var selectedTypeOfSchool = typeOfSchools.FirstOrDefault(x => (x.Data as TypeOfSchool?) == currentData?.TypeOfSchool.AsEnum<TypeOfSchool>()) ?? TypeOfSchools.FirstOrDefault();
            var selectedSchoolGraduation = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == currentData?.Graduation.AsEnum<SchoolGraduation>());
            var selectedUniverityDegree = univerityDegrees.FirstOrDefault(x => (x.Data as UniversityDegree?) == currentData?.UniversityDegree.AsEnum<UniversityDegree>());
            var occupation = currentData?.ProfessionalTitle;
            if (EditState != null)
            {
                description = EditState.Description;
                selectedTypeOfSchool = typeOfSchools.FirstOrDefault(x => (x.Data as TypeOfSchool?) == EditState.TypeOfSchool.AsEnum<TypeOfSchool>());
                selectedSchoolGraduation = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == EditState.Graduation.AsEnum<SchoolGraduation>());
                selectedUniverityDegree = univerityDegrees.FirstOrDefault(x => (x.Data as UniversityDegree?) == EditState.UniversityDegree.AsEnum<UniversityDegree>());
                occupation = EditState.ProfessionalTitle;
            }

            if (!string.IsNullOrWhiteSpace(SelectionResult))
            {
                occupation = SelectionResult.Deserialize<Occupation>();
                isDirty = true;
            }

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(description, occupation, typeOfSchools.AsSortedList(), selectedTypeOfSchool, schoolGraduations.AsSortedList(), selectedSchoolGraduation, univerityDegrees.AsSortedList(), selectedUniverityDegree, isDirty), token);
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
            entry.Description = Description;
            entry.TypeOfSchool = SelectedTypeOfSchool?.Data != null ? ((TypeOfSchool)SelectedTypeOfSchool.Data).ToApolloListItem() : null;
            entry.UniversityDegree = SelectedUniverityDegree?.Data != null ? ((UniversityDegree)SelectedUniverityDegree.Data).ToApolloListItem() : null;
            entry.Graduation = entry.UniversityDegree == null ? SchoolGraduation.AdvancedTechnicalCollegeCertificate.ToApolloListItem() : null;
            entry.ProfessionalTitle = _professionalTitle;
        }

        private void LoadonUIThread(string? description, Occupation? occupation, List<InteractionEntry> typeOfSchools, InteractionEntry? selectedTypeOfSchool, List<InteractionEntry> schoolGraduations, InteractionEntry? selectedSchoolGraduation, List<InteractionEntry> univerityDegrees, InteractionEntry? selectedUniverityDegree, bool isDirty)
        {
            _professionalTitle = occupation;
            OccupationName = _professionalTitle?.PreferedTerm?.FirstOrDefault();
            TypeOfSchools = new ObservableCollection<InteractionEntry>(typeOfSchools);
            SelectedTypeOfSchool = selectedTypeOfSchool;

            SchoolGraduations = new ObservableCollection<InteractionEntry>(schoolGraduations);
            SelectedSchoolGraduation = selectedSchoolGraduation;

            UniverityDegrees = new ObservableCollection<InteractionEntry>(univerityDegrees);
            SelectedUniverityDegree = selectedUniverityDegree;

            Description = description;
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }

        partial void OnDescriptionChanged(string? value)
        {
            IsDirty = true;
        }

        partial void OnSelectedTypeOfSchoolChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        partial void OnSelectedUniverityDegreeChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        partial void OnSelectedSchoolGraduationChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSearchOccupation))]
        private async Task SearchOccupation(CancellationToken token)
        {
            Logger.LogInformation($"Invoked {nameof(SearchOccupationCommand)} in {GetType().Name}.");
            using (var worker = ScheduleWork(token))
            {
                try
                {
                    var parameter = new NavigationParameters();
                    parameter.AddValue(NavigationParameter.SavedState, GetCurrentState());
                    await NavigationService.NavigateAsync(Routes.OccupationSearchView, worker.Token, parameter);
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
