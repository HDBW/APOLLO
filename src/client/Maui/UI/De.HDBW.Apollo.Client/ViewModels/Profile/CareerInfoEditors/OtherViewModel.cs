// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using CommunityToolkit.Mvvm.ComponentModel;
using De.HDBW.Apollo.Client.Contracts;
using De.HDBW.Apollo.SharedContracts.Repositories;
using De.HDBW.Apollo.SharedContracts.Services;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class OtherViewModel : BasicViewModel
    {
        [ObservableProperty]
        private string? _nameOfInstitution;

        [ObservableProperty]
        private string? _city;

        [ObservableProperty]
        private string? _country;

        public OtherViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<OtherViewModel> logger,
            IUserRepository userRepository,
            IUserService userService)
            : base(dispatcherService, navigationService, dialogService, logger, userRepository, userService)
        {
        }

        protected override void ApplyChanges(CareerInfo entity)
        {
            base.ApplyChanges(entity);
            entity.City = City;
            entity.Country = Country;
            entity.NameOfInstitution = NameOfInstitution;
        }

        partial void OnCityChanged(string? value)
        {
            this.IsDirty = true;
        }

        partial void OnCountryChanged(string? value)
        {
            this.IsDirty = true;
        }

        partial void OnNameOfInstitutionChanged(string? value)
        {
            this.IsDirty = true;
        }
    }
}
