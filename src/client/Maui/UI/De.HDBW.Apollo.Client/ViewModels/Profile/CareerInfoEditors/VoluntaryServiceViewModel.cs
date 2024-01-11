// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class VoluntaryServiceViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        private string _occupationName;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private string? _nameOfInstitution;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _voluntaryServiceTypes = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedVoluntaryServiceType;

        private CareerInfo? _careers;

        public VoluntaryServiceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<VoluntaryServiceViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var voluntaryServiceTypes = new List<InteractionEntry>();
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_Other, VoluntaryServiceType.Other, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialYear, VoluntaryServiceType.VoluntarySocialYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_FederalVolunteerService, VoluntaryServiceType.FederalVolunteerService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntaryEcologicalYear, VoluntaryServiceType.VoluntaryEcologicalYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialTrainingYear, VoluntaryServiceType.VoluntarySocialTrainingYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntaryCulturalYear, VoluntaryServiceType.VoluntaryCulturalYear, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntarySocialYearInSport, VoluntaryServiceType.VoluntarySocialYearInSport, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    voluntaryServiceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.VoluntaryServiceType_VoluntaryYearInMonumentConservation, VoluntaryServiceType.VoluntaryYearInMonumentConservation, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(voluntaryServiceTypes), worker.Token);
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

        private void LoadonUIThread(List<InteractionEntry> voluntaryServiceTypes)
        {
            VoluntaryServiceTypes = new ObservableCollection<InteractionEntry>(voluntaryServiceTypes);
            SelectedVoluntaryServiceType = VoluntaryServiceTypes.FirstOrDefault();
        }
    }
}
