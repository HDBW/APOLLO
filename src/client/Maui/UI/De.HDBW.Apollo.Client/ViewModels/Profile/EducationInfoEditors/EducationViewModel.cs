// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.EducationInfoEditors
{
    public partial class EducationViewModel : BasicEducationInfoViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _typeOfSchools = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedTypeOfSchool;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _schoolGraduations = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedSchoolGraduation;

        public EducationViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<EducationViewModel> logger,
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
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_HighSchool, TypeOfSchool.HighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_SecondarySchool, TypeOfSchool.SecondarySchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalCollege, TypeOfSchool.VocationalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_MainSchool, TypeOfSchool.MainSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalHighSchool, TypeOfSchool.VocationalHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            // typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalSchool, TypeOfSchool.VocationalSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_SpecialSchool, TypeOfSchool.SpecialSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_IntegratedComprehensiveSchool, TypeOfSchool.IntegratedComprehensiveSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_SchoolWithMultipleCourses, TypeOfSchool.SchoolWithMultipleCourses, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_TechnicalCollege, TypeOfSchool.TechnicalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_TechnicalHighSchool, TypeOfSchool.TechnicalHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_TechnicalSchool, TypeOfSchool.TechnicalSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_Colleague, TypeOfSchool.Colleague, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_EveningHighSchool, TypeOfSchool.EveningHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_VocationalTrainingSchool, TypeOfSchool.VocationalTrainingSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_NightSchool, TypeOfSchool.NightSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_EveningSchool, TypeOfSchool.EveningSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_WaldorfSchool, TypeOfSchool.WaldorfSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            // typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_TechnicalAcademy, TypeOfSchool.TechnicalAcademy, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            // typeOfSchools.Add(InteractionEntry.Import(Resources.Strings.Resources.TypeOfSchool_UniversityOfAppliedScience, TypeOfSchool.UniversityOfAppliedScience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var schoolGraduations = new List<InteractionEntry>();
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_SecondarySchoolCertificate, SchoolGraduation.SecondarySchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_AdvancedTechnicalCollegeCertificate, SchoolGraduation.AdvancedTechnicalCollegeCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_HigherEducationEntranceQualificationALevel, SchoolGraduation.HigherEducationEntranceQualificationALevel, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_IntermediateSchoolCertificate, SchoolGraduation.IntermediateSchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_ExtendedSecondarySchoolLeavingCertificate, SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_NoSchoolLeavingCertificate, SchoolGraduation.NoSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_SpecialSchoolLeavingCertificate, SchoolGraduation.SpecialSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_SubjectRelatedEntranceQualification, SchoolGraduation.SubjectRelatedEntranceQualification, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(Resources.Strings.Resources.SchoolGraduation_AdvancedTechnicalCollegeWithoutCertificate, SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData, typeOfSchools, schoolGraduations), token);
            return currentData;
        }

        protected override void ApplyChanges(EducationInfo entity)
        {
            base.ApplyChanges(entity);
            entity.TypeOfSchool = (SelectedTypeOfSchool?.Data as TypeOfSchool?) ?? TypeOfSchool.Unknown;
            entity.Graduation = (SelectedSchoolGraduation?.Data as SchoolGraduation?) ?? SchoolGraduation.Unknown;
        }

        private void LoadonUIThread(EducationInfo? educationInfo, List<InteractionEntry> typeOfSchools, List<InteractionEntry> schoolGraduations)
        {
            TypeOfSchools = new ObservableCollection<InteractionEntry>(typeOfSchools);
            SelectedTypeOfSchool = TypeOfSchools.FirstOrDefault(x => (x.Data as TypeOfSchool?) == educationInfo?.TypeOfSchool) ?? TypeOfSchools.FirstOrDefault();
            SchoolGraduations = new ObservableCollection<InteractionEntry>(schoolGraduations);
            SelectedSchoolGraduation = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == educationInfo?.Graduation) ?? null;
        }

        partial void OnSelectedSchoolGraduationChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        partial void OnSelectedTypeOfSchoolChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }
    }
}
