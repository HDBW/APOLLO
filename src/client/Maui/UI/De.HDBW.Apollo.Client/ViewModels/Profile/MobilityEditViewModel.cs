// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Helper;
using De.HDBW.Apollo.Client.Models.Interactions;
using De.HDBW.Apollo.Data.Helper;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Invite.Apollo.App.Graph.Common.Models.UserProfile.Enums;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class MobilityEditViewModel : AbstractProfileEditorViewModel<Mobility>
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _willingsToTravel = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedWillingToTravel;

        [ObservableProperty]
        private ObservableCollection<SelectInteractionEntry> _driverLicenses = new ObservableCollection<SelectInteractionEntry>();

        [ObservableProperty]
        private bool _hasVehicle;

        public MobilityEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<MobilityEditViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override void RefreshCommands()
        {
            base.RefreshCommands();
            ToggleHasVehicleCommand?.NotifyCanExecuteChanged();
            foreach (var license in DriverLicenses)
            {
                license.ToggleSelectionStateCommand?.NotifyCanExecuteChanged();
            }
        }

        protected override async Task<Mobility?> LoadDataAsync(User user, string? enityId, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var mobility = user.Profile?.MobilityInfo;

            var willingsToTravel = new List<InteractionEntry>();
            willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_Yes, Willing.Yes, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_No, Willing.No, (x) => { return Task.CompletedTask; }, (x) => { return true; }));
            willingsToTravel.Add(InteractionEntry.Import(Resources.Strings.Resources.Willing_Partly, Willing.Partly, (x) => { return Task.CompletedTask; }, (x) => { return true; }));

            var driverLicenses = new List<SelectInteractionEntry>();
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.A));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.A1));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.A2));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.AM));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.B));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.BE));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.B96));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.C));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.CE));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.C1));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.C1E));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.D));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.DE));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.D1));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.D1E));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.ConstructionMachines));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.Forklift));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.TrailerDriving));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.L));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.T));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.Class1));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.Class3));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.Class2));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.InstructorASF));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.InstructorASP));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.Moped));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.PassengerTransport));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.InstructorBE));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.InstructorA));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.InstructorCE));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.InstructorDE));
            driverLicenses.Add(CreateLicenseEntry(mobility, DriversLicense.Drivercard));
            await ExecuteOnUIThreadAsync(
                () => LoadonUIThread(mobility, willingsToTravel.AsSortedList(), driverLicenses), token).ConfigureAwait(false);
            return mobility;
        }

        protected override Mobility CreateNewEntry(User user)
        {
            var entry = new Mobility();
            user.Profile!.MobilityInfo = entry;
            return entry;
        }

        protected override void DeleteEntry(User user, Mobility entry)
        {
            CreateNewEntry(user);
        }

        protected override void ApplyChanges(Mobility entry)
        {
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly
            entry.DriverLicenses = DriverLicenses.Where(x => x.IsSelected && x.Data is DriversLicense).Select(x => x.Data).OfType<DriversLicense>().Select(x => x.ToApolloListItem()!).ToList();
#pragma warning restore SA1009 // Closing parenthesis should be spaced correctly
            entry.HasVehicle = HasVehicle;
            entry.WillingToTravel = SelectedWillingToTravel?.Data != null ? ((Willing)SelectedWillingToTravel.Data).ToApolloListItem() : null;
        }

        private void LoadonUIThread(Mobility? mobility, List<InteractionEntry> willingsToTravel, List<SelectInteractionEntry> driverLicenses)
        {
            WillingsToTravel = new ObservableCollection<InteractionEntry>(willingsToTravel);
            SelectedWillingToTravel = WillingsToTravel.FirstOrDefault(x => (int?)(x.Data as Willing?) == mobility?.WillingToTravel?.ListItemId) ?? WillingsToTravel.FirstOrDefault();
            DriverLicenses = new ObservableCollection<SelectInteractionEntry>(driverLicenses);
            HasVehicle = mobility?.HasVehicle ?? false;
            IsDirty = false;
        }

        private void OnISelectedChanged(SelectInteractionEntry entry)
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
            Logger.LogInformation($"Invoked {nameof(ToggleHasVehicleCommand)} in {GetType().Name}.");
            HasVehicle = !HasVehicle;
            return Task.CompletedTask;
        }

        private bool CanToggleHasVehicle()
        {
            return !IsBusy;
        }

        private bool CanNavigate(InteractionEntry entry)
        {
            return false;
        }

        private Task OnNavigate(InteractionEntry entry)
        {
            return Task.CompletedTask;
        }

        private bool CanToggleSelection(SelectInteractionEntry entry)
        {
            return !IsBusy;
        }

        private Task OnToggleSelection(SelectInteractionEntry entry)
        {
            entry.IsSelected = !entry.IsSelected;
            return Task.CompletedTask;
        }

        private bool IsSelected(Mobility? mobility, DriversLicense license)
        {
            return mobility?.DriverLicenses?.Any(x => x.ListItemId == (int)license) ?? false;
        }

        private SelectInteractionEntry CreateLicenseEntry(Mobility? mobility, DriversLicense license)
        {
            var name = Enum.GetName(license);
            var textName = $"DriversLicense_{name}";
            var imageName = $"driverslicense{name!.ToLower()}.png";
            return SelectInteractionEntry.Import(
                Resources.Strings.Resources.ResourceManager.GetString(textName),
                license,
                IsSelected(mobility, license),
                OnNavigate,
                CanNavigate,
                OnToggleSelection,
                CanToggleSelection,
                OnISelectedChanged,
                imageName);
        }
    }
}
