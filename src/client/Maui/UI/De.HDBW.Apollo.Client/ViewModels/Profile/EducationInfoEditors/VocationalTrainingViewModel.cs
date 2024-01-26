// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
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
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_Other, TypeOfSchool.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalCollege, TypeOfSchool.VocationalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_MainSchool, TypeOfSchool.MainSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalHighSchool, TypeOfSchool.VocationalHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_SchoolWithMultipleCourses, TypeOfSchool.SchoolWithMultipleCourses, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_TechnicalCollege, TypeOfSchool.TechnicalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalTrainingSchool, TypeOfSchool.VocationalTrainingSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_TechnicalAcademy, TypeOfSchool.TechnicalAcademy, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_UniversityOfAppliedScience, TypeOfSchool.UniversityOfAppliedScience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

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
            var selection = SelectionResult.Deserialize<Occupation>();
            var isDirty = IsDirty;
            if (EditState != null)
            {
                EditState.ProfessionalTitle = selection;
                isDirty = true;
            }

            await ExecuteOnUIThreadAsync(() => LoadonUIThread(EditState ?? currentData, typeOfSchools, schoolGraduations, univerityDegrees, isDirty), token);
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
            entry.TypeOfSchool = (SelectedTypeOfSchool?.Data as TypeOfSchool?) ?? TypeOfSchool.Unknown;
            entry.UniversityDegree = (SelectedUniverityDegree?.Data as UniversityDegree?) ?? null;
            entry.Graduation = entry.UniversityDegree == null ? SchoolGraduation.AdvancedTechnicalCollegeCertificate : null;
            entry.ProfessionalTitle = _professionalTitle;
        }

        private void LoadonUIThread(EducationInfo? educationInfo, List<InteractionEntry> typeOfSchools, List<InteractionEntry> schoolGraduations, List<InteractionEntry> univerityDegrees, bool isDirty)
        {
            _professionalTitle = educationInfo?.ProfessionalTitle;
            OccupationName = _professionalTitle?.PreferedTerm?.FirstOrDefault();
            TypeOfSchools = new ObservableCollection<InteractionEntry>(typeOfSchools);
            SelectedTypeOfSchool = TypeOfSchools.FirstOrDefault(x => (x.Data as TypeOfSchool?) == educationInfo?.TypeOfSchool) ?? TypeOfSchools.FirstOrDefault();

            SchoolGraduations = new ObservableCollection<InteractionEntry>(schoolGraduations);
            SelectedSchoolGraduation = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == educationInfo?.Graduation) ?? null;

            UniverityDegrees = new ObservableCollection<InteractionEntry>(univerityDegrees);
            SelectedUniverityDegree = univerityDegrees.FirstOrDefault(x => (x.Data as UniversityDegree?) == educationInfo?.UniversityDegree) ?? null;

            Description = educationInfo?.Description ?? string.Empty;
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
    }
}
