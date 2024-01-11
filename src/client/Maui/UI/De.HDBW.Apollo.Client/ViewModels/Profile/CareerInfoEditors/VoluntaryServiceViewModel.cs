﻿// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Client.Contracts;
using Invite.Apollo.App.Graph.Common.Models.UserProfile;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Client.ViewModels.Profile.CareerInfoEditors
{
    public partial class VoluntaryServiceViewModel : BaseViewModel
    {
        private CareerInfo? _careers;

        public VoluntaryServiceViewModel(
            IDispatcherService dispatcherService,
            INavigationService navigationService,
            IDialogService dialogService,
            ILogger<VoluntaryServiceViewModel> logger)
            : base(dispatcherService, navigationService, dialogService, logger)
        {
        }
    }
}