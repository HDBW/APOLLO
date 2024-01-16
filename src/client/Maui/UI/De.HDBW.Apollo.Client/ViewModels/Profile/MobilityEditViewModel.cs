// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class MobilityEditViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _willingsToTravel = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedWillingToTravel;

        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _driverLicenses = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private bool _hasVehicle;

        private Mobility? _mobility;

        public MobilityEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<MobilityEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var willingsToTravel = new List<InteractionEntry>();
                    willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_Yes, Willing.Yes, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_No, Willing.No, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_Partly, Willing.Partly, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

                    var driverLicenses = new List<InteractionEntry>();
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_B, DriversLicense.B, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_BE, DriversLicense.BE, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Forklift, DriversLicense.Forklift, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_C1E, DriversLicense.C1E, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_C1, DriversLicense.C1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_L, DriversLicense.L, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_AM, DriversLicense.AM, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_A, DriversLicense.A, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_CE, DriversLicense.CE, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_C, DriversLicense.C, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_A1, DriversLicense.A1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_B96, DriversLicense.B96, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_T, DriversLicense.T, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_A2, DriversLicense.A2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Moped, DriversLicense.Moped, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Drivercard, DriversLicense.Drivercard, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_PassengerTransport, DriversLicense.PassengerTransport, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_D, DriversLicense.D, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorBE, DriversLicense.InstructorBE, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_ConstructionMachines, DriversLicense.ConstructionMachines, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_DE, DriversLicense.DE, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_D1, DriversLicense.D1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_D1E, DriversLicense.D1E, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorA, DriversLicense.InstructorA, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorCE, DriversLicense.InstructorCE, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_TrailerDriving, DriversLicense.TrailerDriving, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorDE, DriversLicense.InstructorDE, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Class1, DriversLicense.Class1, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Class3, DriversLicense.Class3, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Class2, DriversLicense.Class2, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorASF, DriversLicense.InstructorASF, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    driverLicenses.Add(InteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorASP, DriversLicense.InstructorASP, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(willingsToTravel, driverLicenses), worker.Token);
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

        private void LoadonUIThread(List<InteractionEntry> willingsToTravel, List<InteractionEntry> driverLicenses)
        {
            WillingsToTravel = new ObservableCollection<InteractionEntry>(willingsToTravel);
            SelectedWillingToTravel = WillingsToTravel.FirstOrDefault();
            DriverLicenses = new ObservableCollection<InteractionEntry>(driverLicenses);
        }
    }
}
