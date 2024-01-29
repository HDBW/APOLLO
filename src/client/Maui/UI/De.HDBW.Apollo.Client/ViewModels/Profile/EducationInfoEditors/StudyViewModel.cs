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
    public partial class StudyViewModel : BasicEducationInfoViewModel
    {
        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private string? _description;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _univerityDegrees = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        [Required(ErrorMessageResourceType = typeof(Resources.Strings.Resources), ErrorMessageResourceName = nameof(Resources.Strings.Resources.GlobalError_PropertyRequired))]
        private InteractionEntry? _selectedUniverityDegree;

        public StudyViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<StudyViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override async Task<EducationInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

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

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;
            var description = currentData?.Description;
            var selectedUniverityDegrees = univerityDegrees.FirstOrDefault(x => (x.Data as UniversityDegree?) == currentData?.UniversityDegree);
            await ExecuteOnUIThreadAsync(
                () => LoadonUIThread(description, univerityDegrees.AsSortedList(), selectedUniverityDegrees, isDirty), token);
            return currentData;
        }

        protected override void ApplyChanges(EducationInfo entry)
        {
            base.ApplyChanges(entry);
            entry.UniversityDegree = SelectedUniverityDegree?.Data as UniversityDegree?;
            entry.Description = Description;
        }

        partial void OnDescriptionChanged(string? value)
        {
            IsDirty = true;
            ValidateProperty(value, nameof(Description));
        }

        partial void OnSelectedUniverityDegreeChanged(InteractionEntry? value)
        {
            IsDirty = true;
            ValidateProperty(value, nameof(SelectedUniverityDegree));
        }

        private void LoadonUIThread(string? description, List<InteractionEntry> univerityDegrees, InteractionEntry? selectedUniverityDegree, bool isDirty)
        {
            UniverityDegrees = new ObservableCollection<InteractionEntry>(univerityDegrees);
            SelectedUniverityDegree = selectedUniverityDegree;
            Description = description;
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }

    }
}
