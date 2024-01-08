// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile
{
    public partial class MobilityEditViewModel : BaseViewModel
    {
        //[ObservableProperty]
        //public Willing? _willingToTravel;

        //[ObservableProperty]
        //public List<DriversLicense> _driverLicenses;

        [ObservableProperty]
        public bool _hasVehicle;

        public MobilityEditViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<MobilityEditViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
