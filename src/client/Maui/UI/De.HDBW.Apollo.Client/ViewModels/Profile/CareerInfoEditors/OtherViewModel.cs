// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class OtherViewModel : BaseViewModel
    {
        [ObservableProperty]
        private DateTime _start = DateTime.Today;

        [ObservableProperty]
        private DateTime? _end;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private string? _nameOfInstitution;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _country;

        private CareerInfo? _career;

        public OtherViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<OtherViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
