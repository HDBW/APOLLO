// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class ServiceViewModel : BaseViewModel
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
        private ObservableCollection<InteractionEntry> _serviceTypes = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedServiceType;

        private CareerInfo? _careers;

        public ServiceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ServiceViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var serviceTypes = new List<InteractionEntry>();
                    serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_CivilianService, ServiceType.CivilianService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_MilitaryService, ServiceType.MilitaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_VoluntaryMilitaryService, ServiceType.VoluntaryMilitaryService, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    serviceTypes.Add(InteractionEntry.Import(Resources.Strings.Resources.ServiceType_MilitaryExercise, ServiceType.MilitaryExercise, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    await ExecuteOnUIThreadAsync(() => LoadonUIThread(serviceTypes), worker.Token);
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

        private void LoadonUIThread(List<InteractionEntry> serviceTypes)
        {
            ServiceTypes = new ObservableCollection<InteractionEntry>(serviceTypes);
            SelectedServiceType = ServiceTypes.FirstOrDefault();
        }
    }
}
