﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;
using UserProfile = Invite.Apollo.App.Graph.Common.Models.UserProfile.Profile;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class MobilityEditViewModel : AbstractSaveDataViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _willingsToTravel = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedWillingToTravel;

        [ObservableProperty]
        private ObservableCollection<SelectInteractionEntry> _driverLicenses = new ObservableCollection<SelectInteractionEntry>();

        [ObservableProperty]
        private bool _hasVehicle;

        private Mobility? _mobility;

        private User? _user;

        public MobilityEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<MobilityEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
            UserRepository = userRepository;
            UserService = userService;
        }

        private IUserRepository UserRepository { get; }

        private IUserService UserService { get; }

        public override async Task OnNavigatedToAsync()
        {
            using (var worker = ScheduleWork())
            {
                try
                {
                    var user = await UserRepository.GetItemAsync(worker.Token).ConfigureAwait(false);
                    var mobility = user?.Profile?.MobilityInfo;

                    var willingsToTravel = new List<InteractionEntry>();
                    willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_Yes, Willing.Yes, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_No, Willing.No, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
                    willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_Partly, Willing.Partly, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

                    var driverLicenses = new List<SelectInteractionEntry>();

                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_B, DriversLicense.B, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.B) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_BE, DriversLicense.BE, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.BE) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Forklift, DriversLicense.Forklift, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.Forklift) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_C1E, DriversLicense.C1E, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.C1E) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_C1, DriversLicense.C1, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.C1) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_L, DriversLicense.L, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.L) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_AM, DriversLicense.AM, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.AM) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_A, DriversLicense.A, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.A) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_CE, DriversLicense.CE, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.CE) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_C, DriversLicense.C, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.C) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_A1, DriversLicense.A1, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.A1) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_B96, DriversLicense.B96, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.B96) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_T, DriversLicense.T, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.T) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_A2, DriversLicense.A2, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.A2) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Moped, DriversLicense.Moped, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.Moped) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Drivercard, DriversLicense.Drivercard, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.Drivercard) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_PassengerTransport, DriversLicense.PassengerTransport, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.PassengerTransport) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_D, DriversLicense.D, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.D) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorBE, DriversLicense.InstructorBE, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.InstructorBE) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_ConstructionMachines, DriversLicense.ConstructionMachines, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.ConstructionMachines) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_DE, DriversLicense.DE, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.DE) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_D1, DriversLicense.D1, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.D1) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_D1E, DriversLicense.D1E, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.D1E) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorA, DriversLicense.InstructorA, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.InstructorA) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorCE, DriversLicense.InstructorCE, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.InstructorCE) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_TrailerDriving, DriversLicense.TrailerDriving, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.TrailerDriving) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorDE, DriversLicense.InstructorDE, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.InstructorDE) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Class1, DriversLicense.Class1, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.Class1) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Class3, DriversLicense.Class3, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.Class3) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_Class2, DriversLicense.Class2, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.Class2) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorASF, DriversLicense.InstructorASF, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.InstructorASF) == true));
                    driverLicenses.Add(SelectInteractionEntry.Import(Resources.Strings.Resources.DriversLicense_InstructorASP, DriversLicense.InstructorASP, (x) => { return Task.CompletedTask; }, (x) => { return true; }, OnISelectedChanged, mobility?.DriverLicenses?.Contains(DriversLicense.InstructorASP) == true));

                    await ExecuteOnUIThreadAsync(
                        () => LoadonUIThread(user, mobility, willingsToTravel, driverLicenses), worker.Token);
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

        protected override async Task<bool> SaveAsync(CancellationToken token)
        {
            if (_user == null || !IsDirty)
            {
                return !IsDirty;
            }

            token.ThrowIfCancellationRequested();
            _user.Profile = _user.Profile ?? new UserProfile();
            var mobility = _mobility ?? new Mobility();

            mobility.DriverLicenses = DriverLicenses.Where(x => x.IsSelected && x.Data is DriversLicense).Select(x => x.Data).OfType<DriversLicense>().ToList();
            mobility.HasVehicle = HasVehicle;
            mobility.WillingToTravel = SelectedWillingToTravel?.Data as Willing?;

            if (_user.Profile.MobilityInfo?.WillingToTravel == null)
            {
                _user.Profile.MobilityInfo = mobility;
            }

            var response = await UserService.SaveAsync(_user, token).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(response))
            {
                Logger.LogError($"Unable to save mobility remotely {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            _user.Id = response;
            var userResult = await UserService.GetUserAsync(_user.Id, token).ConfigureAwait(false);
            if (userResult == null || !await UserRepository.SaveAsync(userResult, CancellationToken.None).ConfigureAwait(false))
            {
                Logger.LogError($"Unable to save mobility locally {nameof(SaveAsync)} in {GetType().Name}.");
                return !IsDirty;
            }

            IsDirty = false;
            return !IsDirty;
        }

        private void LoadonUIThread(User? user, Mobility? mobility, List<InteractionEntry> willingsToTravel, List<SelectInteractionEntry> driverLicenses)
        {
            _mobility = mobility;
            _user = user;
            WillingsToTravel = new ObservableCollection<InteractionEntry>(willingsToTravel);
            SelectedWillingToTravel = WillingsToTravel.FirstOrDefault(x => (x.Data as Willing?) == mobility?.WillingToTravel) ?? WillingsToTravel.FirstOrDefault();
            DriverLicenses = new ObservableCollection<SelectInteractionEntry>(driverLicenses);
            HasVehicle = mobility?.HasVehicle ?? false;
            IsDirty = false;
        }

        private void OnISelectedChanged()
        {
            IsDirty = true;
        }

        partial void OnHasVehicleChanged(bool value)
        {
            IsDirty = true;
        }

        partial void OnSelectedWillingToTravelChanged(InteractionEntry? value)
        {
            IsDirty = true;
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanToggleHasVehicle))]
        private Task ToggleHasVehicle(CancellationToken token)
        {
            HasVehicle = !HasVehicle;
            return Task.CompletedTask;
        }

        private bool CanToggleHasVehicle()
        {
            return !IsBusy;
        }
    }
}