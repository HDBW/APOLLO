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
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_Other, VoluntaryServiceType.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialYear, VoluntaryServiceType.VoluntarySocialYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_FederalVolunteerService, VoluntaryServiceType.FederalVolunteerService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntaryEcologicalYear, VoluntaryServiceType.VoluntaryEcologicalYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialTrainingYear, VoluntaryServiceType.VoluntarySocialTrainingYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntaryCulturalYear, VoluntaryServiceType.VoluntaryCulturalYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialYearInSport, VoluntaryServiceType.VoluntarySocialYearInSport, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntaryYearInMonumentConservation, VoluntaryServiceType.VoluntaryYearInMonumentConservation, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var currentData = await base.LoadDataAsync(user, entryId, token).ConfigureAwait(false);
            await ExecuteOnUIThreadAsync(() => LoadonUIThread(currentData, voluntaryServiceTypes), token).ConfigureAwait(false);
            return currentData;
        }

        protected override void ApplyChanges(CareerInfo entity)
        {
            base.ApplyChanges(entity);
            entity.VoluntaryServiceType = (SelectedVoluntaryServiceType?.Data as VoluntaryServiceType?) ?? VoluntaryServiceType.Unknown;
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ClearEndCommand?.NotifyCanExecuteChanged();
        }

        private void LoadonUIThread(CareerInfo? careerInfo, List<InteractionEntry> voluntaryServiceTypes)
        {
            VoluntaryServiceTypes = new ObservableCollection<InteractionEntry>(voluntaryServiceTypes);
            SelectedVoluntaryServiceType = VoluntaryServiceTypes.FirstOrDefault(x => (x.Data as VoluntaryServiceType?) == careerInfo?.VoluntaryServiceType) ?? VoluntaryServiceTypes.FirstOrDefault();
        }

        partial void OnSelectedVoluntaryServiceTypeChanged(InteractionEntry? value)
        {
            this.IsDirty = true;
        }
    }
}
