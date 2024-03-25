// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
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
    public partial class VoluntaryServiceViewModel : OtherViewModel
    {
        [ObservableProperty]
        private string? _occupationName;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _voluntaryServiceTypes = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedVoluntaryServiceType;

        public VoluntaryServiceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<VoluntaryServiceViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override async Task<CareerInfo?> LoadDataAsync(User user, string? entryId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var voluntaryServiceTypes = new List<InteractionEntry>();
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.Other.GetLocalizedString(), VoluntaryServiceType.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.VoluntarySocialYear.GetLocalizedString(), VoluntaryServiceType.VoluntarySocialYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.FederalVolunteerService.GetLocalizedString(), VoluntaryServiceType.FederalVolunteerService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.VoluntaryEcologicalYear.GetLocalizedString(), VoluntaryServiceType.VoluntaryEcologicalYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.VoluntarySocialTrainingYear.GetLocalizedString(), VoluntaryServiceType.VoluntarySocialTrainingYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.VoluntaryCulturalYear.GetLocalizedString(), VoluntaryServiceType.VoluntaryCulturalYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.VoluntarySocialYearInSport.GetLocalizedString(), VoluntaryServiceType.VoluntarySocialYearInSport, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(VoluntaryServiceType.VoluntaryYearInMonumentConservation.GetLocalizedString(), VoluntaryServiceType.VoluntaryYearInMonumentConservation, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            var isDirty = IsDirty;
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData, voluntaryServiceTypes.AsSortedList(), isDirty), token).ConfigureAwait(false);
            return currentData;
        }

        protected override void ApplyChanges(CareerInfo entry)
        {
            base.ApplyChanges(entry);
            entry.VoluntaryServiceType = ((SelectedVoluntaryServiceType?.Data as VoluntaryServiceType?) ?? VoluntaryServiceType.Unknown).ToApolloListItem();
            entry.VoluntaryServiceType = SelectedVoluntaryServiceType?.Data != null ? ((VoluntaryServiceType)SelectedVoluntaryServiceType.Data).ToApolloListItem() : null;

            var hasOccupation = !string.IsNullOrWhiteSpace(OccupationName);

            // delete job if no occupation entered
            if (!hasOccupation && entry.Job != null)
            {
                entry.Job = null;
                return;
            }

            // if there was no job an no text entered. return.
            if (!hasOccupation)
            {
                return;
            }

            var occupation = new Occupation() { TaxonomyInfo = Taxonomy.Unknown, PreferedTerm = new List<string>() { OccupationName! } };

            // if there was no job. apply
            if (entry.Job == null)
            {
                entry.Job = occupation;
                return;
            }

            var currentName = entry.Job.PreferedTerm?.FirstOrDefault();

            // if name of job did not change. return
            if (currentName == OccupationName)
            {
                return;
            }

            // if current job has an id, apply new occupation.
            if (entry.Job.Id != null)
            {
                entry.Job = occupation;
                return;
            }

            if (currentName == null)
            {
                entry.Job.PreferedTerm = new List<string>() { OccupationName! };
            }
            else
            {
                entry.Job.PreferedTerm![0] = OccupationName!;
            }
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        partial void OnSelectedVoluntaryServiceTypeChanged(InteractionEntry? value)
        {
            this.IsDirty = true;
        }

        partial void OnOccupationNameChanged(string? value)
        {
            this.IsDirty = true;
        }

        private void LoadonUIThread(CareerInfo? careerInfo, List<InteractionEntry> voluntaryServiceTypes, bool isDirty)
        {
            VoluntaryServiceTypes = new ObservableCollection<InteractionEntry>(voluntaryServiceTypes);
            SelectedVoluntaryServiceType = VoluntaryServiceTypes.FirstOrDefault(x => (x.Data as VoluntaryServiceType?) == careerInfo?.VoluntaryServiceType.AsEnum<VoluntaryServiceType>()) ?? VoluntaryServiceTypes.FirstOrDefault();
            OccupationName = careerInfo?.Job?.PreferedTerm?.FirstOrDefault();
            IsDirty = isDirty;
            ValidateCommand.Execute(null);
        }
    }
}
