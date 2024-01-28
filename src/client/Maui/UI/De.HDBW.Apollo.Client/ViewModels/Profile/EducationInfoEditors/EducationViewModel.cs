// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
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
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
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
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.Other.GetLocalizedString(), TypeOfSchool.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.HighSchool.GetLocalizedString(), TypeOfSchool.HighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.SecondarySchool.GetLocalizedString(), TypeOfSchool.SecondarySchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalCollege.GetLocalizedString(), TypeOfSchool.VocationalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.MainSchool.GetLocalizedString(), TypeOfSchool.MainSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalHighSchool.GetLocalizedString(), TypeOfSchool.VocationalHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            // typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalSchool.GetLocalizedString(), TypeOfSchool.VocationalSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.SpecialSchool.GetLocalizedString(), TypeOfSchool.SpecialSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.IntegratedComprehensiveSchool.GetLocalizedString(), TypeOfSchool.IntegratedComprehensiveSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.SchoolWithMultipleCourses.GetLocalizedString(), TypeOfSchool.SchoolWithMultipleCourses, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.TechnicalCollege.GetLocalizedString(), TypeOfSchool.TechnicalCollege, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.TechnicalHighSchool.GetLocalizedString(), TypeOfSchool.TechnicalHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.TechnicalSchool.GetLocalizedString(), TypeOfSchool.TechnicalSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.Colleague.GetLocalizedString(), TypeOfSchool.Colleague, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.EveningHighSchool.GetLocalizedString(), TypeOfSchool.EveningHighSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.VocationalTrainingSchool.GetLocalizedString(), TypeOfSchool.VocationalTrainingSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.NightSchool.GetLocalizedString(), TypeOfSchool.NightSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.EveningSchool.GetLocalizedString(), TypeOfSchool.EveningSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.WaldorfSchool.GetLocalizedString(), TypeOfSchool.WaldorfSchool, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            // typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.TechnicalAcademy.GetLocalizedString(), TypeOfSchool.TechnicalAcademy, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            // typeOfSchools.Add(InteractionEntry.Import(TypeOfSchool.UniversityOfAppliedScience.GetLocalizedString(), TypeOfSchool.UniversityOfAppliedScience, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var schoolGraduations = new List<InteractionEntry>();
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SecondarySchoolCertificate.GetLocalizedString(), SchoolGraduation.SecondarySchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.AdvancedTechnicalCollegeCertificate.GetLocalizedString(), SchoolGraduation.AdvancedTechnicalCollegeCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.HigherEducationEntranceQualificationALevel.GetLocalizedString(), SchoolGraduation.HigherEducationEntranceQualificationALevel, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.IntermediateSchoolCertificate.GetLocalizedString(), SchoolGraduation.IntermediateSchoolCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.ExtendedSecondarySchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.NoSchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.NoSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SpecialSchoolLeavingCertificate.GetLocalizedString(), SchoolGraduation.SpecialSchoolLeavingCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.SubjectRelatedEntranceQualification.GetLocalizedString(), SchoolGraduation.SubjectRelatedEntranceQualification, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            schoolGraduations.Add(InteractionEntry.Import(SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate.GetLocalizedString(), SchoolGraduation.AdvancedTechnicalCollegeWithoutCertificate, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;
            var selectedTypeOfSchool = typeOfSchools.FirstOrDefault(x => (x.Data as TypeOfSchool?) == currentData?.TypeOfSchool) ?? TypeOfSchools.FirstOrDefault();
            var selectedSchoolGraduation = schoolGraduations.FirstOrDefault(x => (x.Data as SchoolGraduation?) == currentData?.Graduation) ?? null;
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(typeOfSchools, selectedTypeOfSchool, schoolGraduations, selectedSchoolGraduation, isDirty), token);
            return currentData;
        }

        protected override void ApplyChanges(EducationInfo entry)
        {
            base.ApplyChanges(entry);
            entry.TypeOfSchool = (SelectedTypeOfSchool?.Data as TypeOfSchool?) ?? TypeOfSchool.Unknown;
            entry.Graduation = (SelectedSchoolGraduation?.Data as SchoolGraduation?) ?? SchoolGraduation.Unknown;
        }

        private void LoadonUIThread(List<InteractionEntry> typeOfSchools, InteractionEntry? selectedTypeOfSchool, List<InteractionEntry> schoolGraduations, InteractionEntry? selectedSchoolGraduation, bool isDirty)
        {
            TypeOfSchools = new ObservableCollection<InteractionEntry>(typeOfSchools);
            SelectedTypeOfSchool = selectedTypeOfSchool;
            SchoolGraduations = new ObservableCollection<InteractionEntry>(schoolGraduations);
            SelectedSchoolGraduation = selectedSchoolGraduation;
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }

        partial void OnSelectedSchoolGraduationChanged(InteractionEntry? value)
        {
            ValidateProperty(value, nameof(SelectedSchoolGraduation));
            IsDirty = true;
        }

        partial void OnSelectedTypeOfSchoolChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }
    }
}
