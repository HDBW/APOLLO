// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.Client.Models.Interactions;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.ContactInfoEditors
{
    public partial class ContactViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<InteractionEntry> _contactTypes = new ObservableCollection<InteractionEntry>();

        [ObservableProperty]
        private InteractionEntry? _selectedContactType;

        [ObservableProperty]
        private string? _address;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _zipCode;

        [ObservableProperty]
        private string? _region;

        [ObservableProperty]
        private string? _country;

        [ObservableProperty]
        private string? _email;

        [ObservableProperty]
        private string? _phone;

        private ContactInfo? _contact;

        public ContactViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<ContactViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}
